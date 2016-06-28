using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RatorVaders;

public class ExpressionController : MonoBehaviour {
  public static ExpressionController singleton;
  public static GameObject piece;

  public Font font;
  public GameObject piecePrefab;
  public GameObject canvas;
  public float speed = 1.0f;
  public Expression highestPrecedent;
  public Expression expr;
  public GameObject triggerThreshold;
  
  void Start() {
    singleton = this;
    piece = piecePrefab;
 
    GenerateExpression();
  }

  void GenerateExpression() {
    Regenerate();

    /* expr = new ExpressionMultiply(new ExpressionMultiply(new ExpressionInteger(5), */
                                                    /* new ExpressionInteger(4)), */
                             /* new ExpressionInteger(3)); */

    /* expr = new ExpressionMultiply(new ExpressionMultiply(new ExpressionMultiply(new ExpressionInteger(5), */
                                                    /* new ExpressionInteger(4)), */
                             /* new ExpressionInteger(3)), new ExpressionInteger(1)); */

    expr = new ExpressionMod(new ExpressionAdd(new ExpressionMultiply(new ExpressionInteger(5),
                                                                      new ExpressionInteger(4)),
                                               new ExpressionInteger(3)),
                             new ExpressionInteger(6));

    /* expr = new ExpressionMod(new ExpressionAdd(new ExpressionMultiply(new ExpressionInteger(5), */
                                                                      /* new ExpressionInteger(4)), */
                                               /* new ExpressionInteger(3)), */
                             /* new ExpressionAdd(new ExpressionInteger(6), new ExpressionInteger(4))); */

    /* expr = new ExpressionAdd(new ExpressionInteger(5), new ExpressionInteger(2)); */
    /* Expression expr = new ExpressionInteger(22); */

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

  public bool OnExpressionHit(GameObject gameObject, float projectileY) {
    if (projectileY < triggerThreshold.transform.localPosition.y) {
      return false;
    }

    PieceController piece = gameObject.GetComponent<PieceController>();
    if (piece.parentExpression == highestPrecedent) {
      if (piece.parentExpression.IsLeaf()) {
        Destroy(gameObject);
        GenerateExpression();
      } else {
        expr = expr.Resolve(piece.parentExpression);
        expr.Relayout(gameObject.transform.localPosition.y, true);

        highestPrecedent = expr.GetHighestPrecedentSubexpression();
        if (highestPrecedent == null) {
          highestPrecedent = expr;
        }
      }
    }

    return true;
  }
}
