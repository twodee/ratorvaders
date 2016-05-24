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
  
  void Start() {
    singleton = this;
    piece = piecePrefab;

    Expression expr = new ExpressionMod(
                                        new ExpressionAdd(new ExpressionInteger(5), new ExpressionInteger(3)),
                                        new ExpressionAdd(new ExpressionInteger(6), new ExpressionInteger(4)));
                                        /* new ExpressionInteger(11)); */
    /* Expression expr = new ExpressionAdd(new ExpressionInteger(5), new ExpressionInteger(23456789)); */
    /* Expression expr = new ExpressionInteger(22); */
    GameObject go = expr.GenerateGameObject();
    go.transform.parent = gameObject.transform;
  }
  
  void Update() {
    transform.position += Vector3.up * -speed * Time.deltaTime;
  }

  public void Regenerate() {
    transform.position = new Vector3(0, Camera.main.orthographicSize, 0);
  }

  public static Pair<GameObject, Text> GeneratePiece(string label) {
    GameObject instance = (GameObject) MonoBehaviour.Instantiate(ExpressionController.piece, new Vector3(0, 0, 0), Quaternion.identity);
    Text text = instance.transform.Find("Canvas/Text").gameObject.GetComponent<Text>();
    text.text = label;
    return new Pair<GameObject, Text>(instance, text);
  }
}
