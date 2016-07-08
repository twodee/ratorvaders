using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RatorVaders;

public class ExpressionController : MonoBehaviour {
  public static ExpressionController singleton;
  public static GameObject piece;

  private bool isHit = false;
  private bool isRightHit = false;

  public PlayerController player;
  public Font font;
  public GameObject piecePrefab;
  public GameObject canvas;
  public float speed = 1.0f;
  public Expression highestPrecedent;
  public Expression expr;
  public GameObject triggerThreshold;
  public GuessBox guessBox;
  public GameObject highlighterPrefab;
  public GameObject highlighter;
  public AudioSource audioSource;
  public AudioClip hitPrecedentClip;
  public AudioClip hitNotPrecedentClip;
  public AudioClip resolvedClip;
  public int nLevels;

  public int additiveWeight;
  public int multiplicativeWeight;
  public int logicalWeight;
  public int relationalWeight;
  public int equalityWeight;
  public int stringConcatWeight;
  public int stringIndexOfWeight;
  public int stringCharAtWeight;
  public int minMaxWeight;
  public int stringLengthWeight;
  public int stringToCaseWeight;
  public int stringContainsWeight;
  public int stringIsEmptyWeight;
  public int stringSubstring1Weight;
  public int stringSubstring2Weight;
  public int doubleAdditiveWeight;
  public int doubleCeilFloorWeight;
  public int doubleCastWeight;
  public int intCastWeight;

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
    expr = Expression.GenerateExpression(nLevels);
    DetermineHighestPrecedent();
    GameObject go = expr.GenerateGameObject();
    go.transform.parent = gameObject.transform;
    go.transform.localPosition = new Vector3(0, 0, 0);
  }
  
  void Update() {
    transform.position += Vector3.up * -speed * Time.deltaTime;

    if (Input.GetKeyUp("left ctrl")) {
      Destroy(expr.gameObject);
      GenerateExpression();
    }
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

  public bool OnExpressionHit(GameObject gameObject, float projectileY, GameObject projectile) {
    if (projectileY < triggerThreshold.transform.position.y) {
      return false;
    }

    isHit = true;

    PieceController piece = gameObject.GetComponent<PieceController>();
    if (piece.parentExpression == highestPrecedent) {
      isRightHit = true;

      /* Debug.Log("piece.parentExpression: " + piece.parentExpression); */
      if (piece.parentExpression.IsLeaf()) {
        Destroy(gameObject);
        GenerateExpression();
        audioSource.PlayOneShot(resolvedClip);
      } else {
        Bounds b = highestPrecedent.gameObject.GetComponent<BoxCollider2D>().bounds;

        highlighter = (GameObject) Instantiate(highlighterPrefab, highestPrecedent.gameObject.transform.position, Quaternion.identity);
        highlighter.transform.localScale = new Vector3(b.size.x, b.size.y, 1);
        highlighter.transform.parent = highestPrecedent.gameObject.transform;
        highlighter.GetComponent<SpriteRenderer>().color = guessBox.GetComponent<Image>().color;

        // Sometimes a stray bullet can hit after the player is dead. No input
        // in such a case.
        if (!player.isDead) {
          guessBox.gameObject.SetActive(true);
          guessBox.AppendCursor();
        }
        audioSource.PlayOneShot(hitPrecedentClip);
      }
    }

    Destroy(projectile);
    return true;
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
      isParseable = guessBox.text == "true" || guessBox.text == "false";
      bool guess = guessBox.text == "true" ? true : false;
      isCorrect = guess == exprBoolean.ToBool();
    } else {
      ExpressionInteger exprInteger = answer as ExpressionInteger;
      if (exprInteger != null) {
        int guess;
        isParseable = int.TryParse(guessBox.text, out guess);
        isCorrect = guess == exprInteger.ToInt();
      } else {
        ExpressionString exprString = answer as ExpressionString;
        if (exprString != null) {
          isParseable = true;
          isCorrect = guessBox.text == exprString.ToString();
        } else {
          ExpressionChar exprChar = answer as ExpressionChar;
          if (exprChar != null) {
            isParseable = guessBox.text.Length == 1;
            isCorrect = guessBox.text[0] == exprChar.ToChar();
          } else {
            ExpressionDouble exprDouble = answer as ExpressionDouble;
            if (exprDouble != null) {
              float guess;
              isParseable = float.TryParse(guessBox.text, out guess) && guessBox.text.IndexOf('.') >= 0;
              isCorrect = Mathf.Abs(guess - exprDouble.ToDouble()) < 1.0e-3f;
            }
          }
        }
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

  public void HideInput() {
    guessBox.Hide();
    Destroy(highlighter);
  }

  private void DetermineHighestPrecedent() {
    highestPrecedent = expr.GetHighestPrecedentSubexpression();
    if (highestPrecedent == null) {
      highestPrecedent = expr;
    }
  }

  public void Resolve() {
    HideInput();

    expr = expr.Resolve(highestPrecedent);
    expr.Relayout(true);
    DetermineHighestPrecedent();

    audioSource.PlayOneShot(resolvedClip);
    highlighter = null;
  }
}
