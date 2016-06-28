using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionInteger : Expression {
  private int i;
  private Pair<GameObject, Text> pair = null;

  public ExpressionInteger(int i) : base(Precedence.LITERAL) {
    this.i = i;
  }

  override public GameObject GenerateGameObject() {
    pair = ExpressionController.GeneratePiece(i.ToString(), this);
    Relayout(0.0f, false);
    return pair.first;
  }

  IEnumerator Test(GameObject pieceToPack, Text textToFitAround, float height, bool isAnimated) {
    yield return null;
    Vector3[] corners = new Vector3[4];
    textToFitAround.GetComponent<RectTransform>().GetWorldCorners(corners);
    pieceToPack.GetComponent<BoxCollider2D>().size = new Vector2(corners[2].x - corners[0].x, corners[2].y - corners[0].y);
    ExpressionInfixBinaryOperator.MoveTo(pieceToPack, new Vector3(pair.first.transform.localPosition.x, height, 0), isAnimated);
  }

  override public void Relayout(float height, bool isAnimated) {
    ExpressionController.singleton.StartCoroutine(Test(pair.first, pair.second, height, isAnimated));
  }

  public int ToInt() {
    return i;
  }

  override public string ToString() {
    return "" + i;
  }

  override public bool IsLeaf() {
    return true;
  }
}

/* ------------------------------------------------------------------------- */

}
