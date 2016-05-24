using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public abstract class ExpressionInfixBinaryOperator : Expression {
  protected Expression left;
  protected Expression right;
  protected string label;

  public ExpressionInfixBinaryOperator(Expression left,
                                       Expression right,
                                       Precedence precedence,
                                       string label) : base(precedence) {
    this.left = left;
    this.right = right;
    this.label = label;
  }

  override public GameObject GenerateGameObject() {
    GameObject lo = left.GenerateGameObject();
    GameObject ro = right.GenerateGameObject();
    Pair<GameObject, Text> rator = ExpressionController.GeneratePiece(label);

    GameObject mix = new GameObject("Mix");
    mix.AddComponent<BoxCollider2D>();

    Pair<GameObject, Text> leftOpen = null;
    Pair<GameObject, Text> leftClose = null;
    if (left.precedence.CompareTo(precedence) < 0) {
      leftOpen = ExpressionController.GeneratePiece("(");
      leftClose = ExpressionController.GeneratePiece(")");
      leftOpen.first.transform.parent = mix.transform;
      lo.transform.parent = mix.transform;
      leftClose.first.transform.parent = mix.transform;
    } else {
      lo.transform.parent = mix.transform;
    }

    rator.first.transform.parent = mix.transform;

    Pair<GameObject, Text> rightOpen = null;
    Pair<GameObject, Text> rightClose = null;
    if (right.precedence.CompareTo(precedence) < 0) {
      rightOpen = ExpressionController.GeneratePiece("(");
      rightClose = ExpressionController.GeneratePiece(")");
      rightOpen.first.transform.parent = mix.transform;
      ro.transform.parent = mix.transform;
      rightClose.first.transform.parent = mix.transform;
    } else {
      ro.transform.parent = mix.transform;
    }

    ExpressionController.singleton.StartCoroutine(Test(mix, lo, ro, rator.second, leftOpen, leftClose, rightOpen, rightClose));

    return mix;
  }

  IEnumerator Test(GameObject go, GameObject lo, GameObject ro, Text text, Pair<GameObject, Text> leftOpen, Pair<GameObject, Text> leftClose, Pair<GameObject, Text> rightOpen, Pair<GameObject, Text> rightClose) {
    yield return null;

    Vector3[] corners = new Vector3[4];

    text.GetComponent<RectTransform>().GetWorldCorners(corners);
    float halfWidth = corners[2].x;

    Bounds lb = lo.GetComponent<BoxCollider2D>().bounds;
    Bounds rb = ro.GetComponent<BoxCollider2D>().bounds;
    
    float leftPush = 0.0f;
    if (leftClose != null) {
      leftClose.second.GetComponent<RectTransform>().GetWorldCorners(corners);
      leftPush = corners[2].x - corners[0].x;
      float closeRadius = corners[2].x;
      leftClose.first.transform.position -= new Vector3(halfWidth + closeRadius, 0, 0);

      leftOpen.second.GetComponent<RectTransform>().GetWorldCorners(corners);
      float openRadius = corners[2].x * 2;
      leftOpen.first.transform.position -= new Vector3(halfWidth + closeRadius + openRadius + lb.size.x, 0, 0);
    }

    float rightPush = 0.0f;
    if (rightOpen != null) {
      rightOpen.second.GetComponent<RectTransform>().GetWorldCorners(corners);
      rightPush = corners[2].x - corners[0].x;
      float openRadius = corners[2].x;
      rightOpen.first.transform.position += new Vector3(halfWidth + openRadius, 0, 0);

      rightClose.second.GetComponent<RectTransform>().GetWorldCorners(corners);
      float closeRadius = corners[2].x * 2;
      rightClose.first.transform.position += new Vector3(halfWidth + closeRadius + openRadius + rb.size.x, 0, 0);
    }

    lo.transform.position -= new Vector3(halfWidth + lb.extents.x + leftPush, 0, 0);
    ro.transform.position += new Vector3(halfWidth + rb.extents.x + rightPush, 0, 0);

    lb = lo.GetComponent<BoxCollider2D>().bounds;
    rb = ro.GetComponent<BoxCollider2D>().bounds;

    Bounds b = new Bounds(Vector3.zero, new Vector3(1, 1, 0));
    b.SetMinMax(lb.min, rb. max);
    go.GetComponent<BoxCollider2D>().offset = b.center;
    go.GetComponent<BoxCollider2D>().size = b.size;
  }
}

/* ------------------------------------------------------------------------- */

}
