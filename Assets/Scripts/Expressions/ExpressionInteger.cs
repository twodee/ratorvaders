using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionInteger : Expression {
  private int i;

  public ExpressionInteger(int i) : base(Precedence.LITERAL) {
    this.i = i;
  }

  override public GameObject GenerateGameObject() {
    Pair<GameObject, Text> pair = ExpressionController.GeneratePiece(i.ToString());
    ExpressionController.singleton.StartCoroutine(Test(pair.first, pair.second));
    return pair.first;
  }

  IEnumerator Test(GameObject pieceToPack, Text textToFitAround) {
    yield return null;
    Vector3[] corners = new Vector3[4];
    textToFitAround.GetComponent<RectTransform>().GetWorldCorners(corners);
    pieceToPack.GetComponent<BoxCollider2D>().size = new Vector2(corners[2].x - corners[0].x, corners[2].y - corners[0].y);
  }
}

/* ------------------------------------------------------------------------- */

}
