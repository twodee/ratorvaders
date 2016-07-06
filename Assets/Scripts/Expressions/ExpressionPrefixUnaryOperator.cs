using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public abstract class ExpressionPrefixUnaryOperator : Expression {
  protected Expression right;
  protected string label;

  private Pair<GameObject, Text> rightOpen = null;
  private Pair<GameObject, Text> rightClose = null;
  private Pair<GameObject, Text> rator = null;
  private GameObject ro = null;

  public ExpressionPrefixUnaryOperator(Expression right,
                                       Precedence precedence,
                                       string label) : base(precedence) {
    this.right = right;
    this.label = label;
  }

  override public GameObject GenerateGameObject() {
    ro = right.GenerateGameObject();
    rator = ExpressionController.GeneratePiece(label, this);

    gameObject = new GameObject(ToString());
    gameObject.AddComponent<BoxCollider2D>();

    rator.first.transform.parent = gameObject.transform;

    // Add right child
    if (right.precedence.CompareTo(precedence) < 0) {
      rightOpen = ExpressionController.GeneratePiece("(");
      rightClose = ExpressionController.GeneratePiece(")");
      rightOpen.first.transform.parent = gameObject.transform;
      ro.transform.parent = gameObject.transform;
      rightClose.first.transform.parent = gameObject.transform;
    } else {
      ro.transform.parent = gameObject.transform;
    }

    Relayout(false);

    return gameObject;
  }

  override public void Relayout(bool isAnimated) {
    right.Relayout(isAnimated);
    ExpressionController.singleton.StartCoroutine(PositionAndFit(isAnimated));
  }

  IEnumerator PositionAndFit(bool isAnimated) {
    yield return null;

    // How wide is the operator?
    Vector3[] corners = new Vector3[4];
    rator.second.GetComponent<RectTransform>().GetWorldCorners(corners);
    float operatorWidth = corners[2].x - corners[0].x;
    rator.first.GetComponent<BoxCollider2D>().size = new Vector2(corners[2].x - corners[0].x, corners[2].y - corners[0].y);

    // How wide are the operand expressions?
    Bounds rightBounds = ro.GetComponent<BoxCollider2D>().bounds;

    // How wide are the right parentheses?
    float rightOpenParenthesisWidth = 0.0f;
    float rightCloseParenthesisWidth = 0.0f;
    if (rightClose != null) {
      rightOpen.second.GetComponent<RectTransform>().GetWorldCorners(corners);
      rightOpenParenthesisWidth = corners[2].x - corners[0].x;
      rightClose.second.GetComponent<RectTransform>().GetWorldCorners(corners);
      rightCloseParenthesisWidth = corners[2].x - corners[0].x;
    }

    // How wide is everything?
    float totalWidth = operatorWidth + rightBounds.size.x + rightOpenParenthesisWidth + rightCloseParenthesisWidth;

    // Now let's move everything.
    MoveTo(gameObject, new Vector3(0, 0, 0), isAnimated);
    float leftSoFar = totalWidth * -0.5f;

    // Operator
    MoveTo(rator.first, new Vector3(leftSoFar + operatorWidth * 0.5f, 0, 0), isAnimated);
    leftSoFar += operatorWidth;

    // (
    if (rightOpen != null) {
      MoveTo(rightOpen.first, new Vector3(leftSoFar + rightOpenParenthesisWidth * 0.5f, 0, 0), isAnimated);
      leftSoFar += rightOpenParenthesisWidth;
    }

    // Right subexpression
    MoveTo(ro, new Vector3(leftSoFar + rightBounds.extents.x, 0, 0), isAnimated);
    leftSoFar += rightBounds.size.x;

    // )
    if (rightClose != null) {
      MoveTo(rightClose.first, new Vector3(leftSoFar + rightCloseParenthesisWidth * 0.5f, 0, 0), isAnimated);
      leftSoFar += rightCloseParenthesisWidth;
    }

    Bounds b = new Bounds(Vector3.zero, new Vector3(1, 1, 0));

    Vector3 least = rightBounds.min;
    least.x = -totalWidth * 0.5f;
    Vector3 most = rightBounds.max;
    most.x = totalWidth * 0.5f;

    b.SetMinMax(least, most);
    gameObject.GetComponent<BoxCollider2D>().size = b.size;
  }

  override public Expression GetHighestPrecedentSubexpression() {
    Expression winner = right.GetHighestPrecedentSubexpression();
    if (winner == null) {
      winner = this;
    }

    return winner;
  }

  override public string ToString() {
    string s = "";

    s += label + " ";

    if (right.precedence.CompareTo(precedence) < 0) {
      s += "(" + right.ToString() + ")";
    } else {
      s += right.ToString();
    }

    return s;
  }

  override public Expression Resolve(Expression toResolve) {
    if (toResolve == right) {
      Expression replacementExpr = toResolve.Evaluate();
      GameObject replacementObject = replacementExpr.GenerateGameObject();
      replacementObject.transform.parent = gameObject.transform;
      replacementObject.transform.localPosition = ro.transform.localPosition;

      if (rightOpen != null) {
        GameObject.Destroy(rightOpen.first);
        GameObject.Destroy(rightClose.first);
      }
      GameObject.Destroy(ro);
      rightOpen = rightClose = null;
      ro = replacementObject;
      right = replacementExpr;
      return this;
    } else if (toResolve == this) {
      Expression replacementExpr = toResolve.Evaluate();
      GameObject replacementObject = replacementExpr.GenerateGameObject();
      replacementObject.transform.parent = GameObject.Find("/ExpressionRoot").transform;
      replacementObject.transform.localPosition = gameObject.transform.localPosition;
      GameObject.Destroy(this.gameObject);
      return replacementExpr;
    } else {
      right = right.Resolve(toResolve);
      return this;
    }
  }

  override public bool IsLeaf() {
    return false;
  }
}

/* ------------------------------------------------------------------------- */

}
