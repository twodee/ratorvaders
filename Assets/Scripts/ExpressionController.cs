using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RatorVaders;

public class ExpressionController : MonoBehaviour {
  public static ExpressionController singleton;
  public static GameObject piece;

  private bool isHit = false;
  private bool isRightHit = false;

  public Font font;
  public GameObject piecePrefab;
  public GameObject canvas;
  public float speed = 1.0f;
  public Expression highestPrecedent;
  public Expression expr;
  public GameObject triggerThreshold;
  public GuessBox guessBox;
  public GameObject highlighterPrefab;
  public AudioSource audioSource;
  public AudioClip hitPrecedentClip;
  public AudioClip hitNotPrecedentClip;
  public AudioClip resolvedClip;

  public bool isAnswering {
    get {
      return guessBox.gameObject.activeInHierarchy;
    }
  }
  
  void Start() {
    singleton = this;
    piece = piecePrefab;
    audioSource = GetComponent<AudioSource>();

    guessBox.onEndEdit.AddListener(delegate {
      CheckAnswer();
    });
    
    // Expand guess to fill screen. I can't do this with anchors because the shake animation won't work then.
    guessBox.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Camera.main.pixelWidth);
 
    GenerateExpression();
  }

  void GenerateExpression() {
    Regenerate();
    expr = Expression.GenerateExpression(3);
    highestPrecedent = expr.GetHighestPrecedentSubexpression();
    GameObject go = expr.GenerateGameObject();
    go.transform.parent = gameObject.transform;
  }
  
  void Update() {
    transform.position += Vector3.up * -speed * Time.deltaTime;
  }

  public void Regenerate() {
    transform.position = new Vector3(0, Camera.main.orthographicSize * 0.75f, 0);
  }

  public static Pair<GameObject, Text> GeneratePiece(string label, Expression parent = null) {
    GameObject instance = (GameObject) MonoBehaviour.Instantiate(ExpressionController.piece, new Vector3(0, 0, 0), Quaternion.identity);
    instance.name = label;
    instance.GetComponent<PieceController>().parentExpression = parent;
    Text text = instance.transform.Find("Canvas/Text").gameObject.GetComponent<Text>();
    text.text = label;
    return new Pair<GameObject, Text>(instance, text);
  }

  public void OnExpressionHit(GameObject gameObject, float projectileY, GameObject projectile) {
    if (projectileY < triggerThreshold.transform.position.y) {
      return;
    }

    isHit = true;

    PieceController piece = gameObject.GetComponent<PieceController>();
    if (piece.parentExpression == highestPrecedent) {
      isRightHit = true;

      if (piece.parentExpression.IsLeaf()) {
        Destroy(gameObject);
        GenerateExpression();
        audioSource.PlayOneShot(resolvedClip);
      } else {
        Bounds b = highestPrecedent.gameObject.GetComponent<BoxCollider2D>().bounds;

        GameObject highlighter = (GameObject) Instantiate(highlighterPrefab, highestPrecedent.gameObject.transform.position, Quaternion.identity);
        highlighter.transform.localScale = new Vector3(b.size.x, b.size.y, 1);
        highlighter.transform.parent = highestPrecedent.gameObject.transform;
        highlighter.GetComponent<SpriteRenderer>().color = guessBox.GetComponent<Image>().color;

        guessBox.gameObject.SetActive(true);
        guessBox.AppendCursor();
        audioSource.PlayOneShot(hitPrecedentClip);
      }
    }

    Destroy(projectile);
  }

  void LateUpdate() {
    if (isHit && !isRightHit) {
      audioSource.PlayOneShot(hitNotPrecedentClip);
    }
    isHit = false;
    isRightHit = false;
  }

  public void CheckAnswer() {
    Expression answer = highestPrecedent.Evaluate();
    bool isParseable = false;
    bool isCorrect = false;

    ExpressionBoolean exprBoolean = answer as ExpressionBoolean;
    if (exprBoolean != null) {
      isParseable = guessBox.text == "t" || guessBox.text == "f";
      bool guess = guessBox.text == "t" ? true : false;
      isCorrect = guess == exprBoolean.ToBool();
    } else {
      ExpressionInteger exprInteger = answer as ExpressionInteger;
      if (exprInteger != null) {
        int guess;
        isParseable = int.TryParse(guessBox.text, out guess);
        isCorrect = guess == exprInteger.ToInt();
      }
    }

    if (isParseable && isCorrect) {
      Resolve(); 
    } else {
      guessBox.gameObject.GetComponent<Animator>().SetTrigger("IsWrong");
      guessBox.AppendCursor();
      audioSource.PlayOneShot(hitNotPrecedentClip);
    }
  }

  public void Resolve() {
    guessBox.text = "";
    guessBox.gameObject.SetActive(false);

    expr = expr.Resolve(highestPrecedent);
    expr.Relayout(expr.gameObject.transform.localPosition.y, true);

    highestPrecedent = expr.GetHighestPrecedentSubexpression();
    if (highestPrecedent == null) {
      highestPrecedent = expr;
    }

    audioSource.PlayOneShot(resolvedClip);
  }
}
