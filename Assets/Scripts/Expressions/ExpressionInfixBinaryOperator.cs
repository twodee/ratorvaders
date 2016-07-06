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

  private Pair<GameObject, Text> leftOpen = null;
  private Pair<GameObject, Text> leftClose = null;
  private Pair<GameObject, Text> rightOpen = null;
  private Pair<GameObject, Text> rightClose = null;
  private Pair<GameObject, Text> rator = null;
  private GameObject lo = null;
  private GameObject ro = null;

  public ExpressionInfixBinaryOperator(Expression left,
                                       Expression right,
                                       Precedence precedence,
                                       string label) : base(precedence) {
    this.left = left;
    this.right = right;
    this.label = label;
  }

  override public GameObject GenerateGameObject() {
    lo = left.GenerateGameObject();
    ro = right.GenerateGameObject();
    rator = ExpressionController.GeneratePiece(label, this);

    gameObject = new GameObject(ToString());
    gameObject.AddComponent<BoxCollider2D>();

    // Add left child
    if (left.precedence.CompareTo(precedence) < 0) {
      leftOpen = ExpressionController.GeneratePiece("(");
      leftClose = ExpressionController.GeneratePiece(")");
      leftOpen.first.transform.parent = gameObject.transform;
      lo.transform.parent = gameObject.transform;
      leftClose.first.transform.parent = gameObject.transform;
    } else {
      lo.transform.parent = gameObject.transform;
    }

    rator.first.transform.parent = gameObject.transform;

    // Add right child
    if (right.precedence.CompareTo(precedence) <= 0) {
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
    left.Relayout(isAnimated);
    right.Relayout(isAnimated);
    ExpressionController.singleton.StartCoroutine(PositionAndFit(isAnimated));
  }

  IEnumerator PositionAndFit(bool isAnimated) {
    yield return null;

    // How wide is the operator?
    Vector3[] corners = new Vector3[4];
    rator.second.GetComponent<RectTransform>().GetWorldCorners(corners);
    float operatorWidth = corners[2].x - corners[0].x;

    float operatorPadding = operatorWidth / label.Length * 0.5f;
    operatorWidth += operatorPadding;

    rator.first.GetComponent<BoxCollider2D>().size = new Vector2(corners[2].x - corners[0].x, corners[2].y - corners[0].y);

    // How wide are the operand expressions?
    Bounds leftBounds = lo.GetComponent<BoxCollider2D>().bounds;
    Bounds rightBounds = ro.GetComponent<BoxCollider2D>().bounds;

    // How wide are the left parentheses?
    float leftOpenParenthesisWidth = 0.0f;
    float leftCloseParenthesisWidth = 0.0f;
    if (leftClose != null) {
      leftOpen.second.GetComponent<RectTransform>().GetWorldCorners(corners);
      leftOpenParenthesisWidth = corners[2].x - corners[0].x;
      leftClose.second.GetComponent<RectTransform>().GetWorldCorners(corners);
      leftCloseParenthesisWidth = corners[2].x - corners[0].x;
    }

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
    float totalWidth = operatorWidth + leftBounds.size.x + rightBounds.size.x + leftOpenParenthesisWidth + leftCloseParenthesisWidth + rightOpenParenthesisWidth + rightCloseParenthesisWidth;

    // Now let's move everything.
    MoveTo(gameObject, new Vector3(0, 0, 0), isAnimated);
    float leftSoFar = totalWidth * -0.5f;

    // (
    if (leftOpen != null) {
      MoveTo(leftOpen.first, new Vector3(leftSoFar + leftOpenParenthesisWidth * 0.5f, 0, 0), isAnimated);
      leftSoFar += leftOpenParenthesisWidth;
    }

    // Left subexpression
    MoveTo(lo, new Vector3(leftSoFar + leftBounds.extents.x, 0, 0), isAnimated);
    leftSoFar += leftBounds.size.x;

    // )
    if (leftClose != null) {
      MoveTo(leftClose.first, new Vector3(leftSoFar + leftCloseParenthesisWidth * 0.5f, 0, 0), isAnimated);
      leftSoFar += leftCloseParenthesisWidth;
    }

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

    Vector3 least = leftBounds.min;
    least.x = -totalWidth * 0.5f;
    Vector3 most = rightBounds.max;
    most.x = totalWidth * 0.5f;

    b.SetMinMax(least, most);
    gameObject.GetComponent<BoxCollider2D>().size = b.size;
  }

  override public Expression GetHighestPrecedentSubexpression() {
    // The highest precedent subexpression is the leafmost, leftmost one. So,
    // we try the left child first. Failing that, the right child. Failing
    // that, we're it.
 
    Expression winner = left.GetHighestPrecedentSubexpression();
    if (winner == null) {
      winner = right.GetHighestPrecedentSubexpression();
      if (winner == null) {
        winner = this;
      }
    }

    return winner;
  }

  override public string ToString() {
    string s = "";

    if (left.precedence.CompareTo(precedence) < 0) {
      s += "(" + left.ToString() + ")";
    } else {
      s += left.ToString();
    }

    s += " " + label + " ";

    if (right.precedence.CompareTo(precedence) <= 0) {
      s += "(" + right.ToString() + ")";
    } else {
      s += right.ToString();
    }

    return s;
  }

  override public Expression Resolve(Expression toResolve) {
    if (toResolve == left) {
      Expression replacementExpr = toResolve.Evaluate();
      GameObject replacementObject = replacementExpr.GenerateGameObject();
      replacementObject.transform.parent = gameObject.transform;
      replacementObject.transform.localPosition = lo.transform.localPosition;

      if (leftOpen != null) {
        GameObject.Destroy(leftOpen.first);
        GameObject.Destroy(leftClose.first);
        leftOpen = leftClose = null;
      }
      GameObject.Destroy(lo);
      lo = replacementObject;
      left = replacementExpr;
      return this;
    } else if (toResolve == right) {
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
      left = left.Resolve(toResolve);
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
