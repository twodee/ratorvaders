using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public abstract class ExpressionLiteral : Expression {
  private Text text;
  private string token;

  public ExpressionLiteral(string token) : base(Precedence.LITERAL) {
    this.token = token;
  }

  override public GameObject GenerateGameObject() {
    Pair<GameObject, Text> pair = ExpressionController.GeneratePiece(token, this);
    gameObject = pair.first;
    text = pair.second;

    Relayout(false);
    return gameObject;
  }

  IEnumerator PositionAndFit(bool isAnimated) {
    yield return null;
    Vector3[] corners = new Vector3[4];
    text.GetComponent<RectTransform>().GetWorldCorners(corners);
    gameObject.GetComponent<BoxCollider2D>().size = new Vector2(corners[2].x - corners[0].x, corners[2].y - corners[0].y);
    MoveTo(gameObject, new Vector3(gameObject.transform.localPosition.x, 0, 0), isAnimated);
  }

  override public void Relayout(bool isAnimated) {
    ExpressionController.singleton.StartCoroutine(PositionAndFit(isAnimated));
  }

  override public string ToString() {
    return token;
  }

  override public bool IsLeaf() {
    return true;
  }
}

/* ------------------------------------------------------------------------- */

}
