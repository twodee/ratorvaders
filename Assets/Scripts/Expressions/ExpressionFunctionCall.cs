using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public abstract class ExpressionFunctionCall : Expression {
  protected List<Expression> parameterExpressions;
  protected string label;

  private Pair<GameObject, Text> open;
  private Pair<GameObject, Text> close;
  private Pair<GameObject, Text> rator;
  private List<Pair<GameObject, Text>> commas;
  private List<GameObject> parameterObjects;

  public ExpressionFunctionCall(List<Expression> parameterExpressions,
                                string label) : base(Precedence.FUNCTION_CALL) {
    this.parameterExpressions = parameterExpressions;
    this.label = label;
    commas = new List<Pair<GameObject, Text>>();
    parameterObjects = new List<GameObject>();
  }

  override public GameObject GenerateGameObject() {
    // Make parent.
    gameObject = new GameObject(ToString());
    gameObject.AddComponent<BoxCollider2D>();

    // Add function identifier.
    rator = ExpressionController.GeneratePiece(label, this);
    rator.first.transform.parent = gameObject.transform;

    // Add (
    open = ExpressionController.GeneratePiece("(");
    open.first.transform.parent = gameObject.transform;

    // Add parameters and separating commas.
    for (int i = 0; i < parameterExpressions.Count; ++i) {
      parameterObjects.Add(parameterExpressions[i].GenerateGameObject());
      parameterObjects[i].transform.parent = gameObject.transform;
      if (i > 0) {
        commas.Add(ExpressionController.GeneratePiece(","));
        commas[commas.Count - 1].first.transform.parent = gameObject.transform;
      }
    }

    // Add )
    close = ExpressionController.GeneratePiece(")");
    close.first.transform.parent = gameObject.transform;

    Relayout(false);

    return gameObject;
  }

  override public void Relayout(bool isAnimated) {
    foreach (Expression parameterExpression in parameterExpressions) {
      parameterExpression.Relayout(isAnimated);
    }
    ExpressionController.singleton.StartCoroutine(PositionAndFit(isAnimated));
  }

  IEnumerator PositionAndFit(bool isAnimated) {
    yield return null;

    // How wide is the operator?
    Vector3[] corners = new Vector3[4];
    rator.second.GetComponent<RectTransform>().GetWorldCorners(corners);
    float operatorWidth = corners[2].x - corners[0].x;

    rator.first.GetComponent<BoxCollider2D>().size = new Vector2(corners[2].x - corners[0].x, corners[2].y - corners[0].y);

    // How wide are the parameter expressions?
    List<Bounds> bounds = new List<Bounds>();
    foreach (GameObject parameterObject in parameterObjects) {
      bounds.Add(parameterObject.GetComponent<BoxCollider2D>().bounds);
    }

    // How wide is the left parenthesis?
    float openWidth = 0.0f;
    open.second.GetComponent<RectTransform>().GetWorldCorners(corners);
    openWidth = corners[2].x - corners[0].x;

    // How wide is the right parenthesis?
    float closeWidth = 0.0f;
    close.second.GetComponent<RectTransform>().GetWorldCorners(corners);
    closeWidth = corners[2].x - corners[0].x;

    // How wide is everything?
    float totalWidth = operatorWidth + openWidth + closeWidth;
    foreach (Bounds box in bounds) {
      totalWidth += box.size.x;
    }

    float commaWidth = 0.0f;

    if (commas.Count > 0) {
      commas[0].second.GetComponent<RectTransform>().GetWorldCorners(corners);
      commaWidth = corners[2].x - corners[0].x;
    }

    totalWidth += commas.Count * commaWidth;

    // Now let's move everything.
    MoveTo(gameObject, new Vector3(0, 0, 0), isAnimated);
    float leftSoFar = totalWidth * -0.5f;

    // Identifier
    MoveTo(rator.first, new Vector3(leftSoFar + operatorWidth * 0.5f, 0, 0), isAnimated);
    leftSoFar += operatorWidth;

    // (
    MoveTo(open.first, new Vector3(leftSoFar + openWidth * 0.5f, 0, 0), isAnimated);
    leftSoFar += openWidth;

    // Parameter expressions
    for (int i = 0; i < parameterObjects.Count; ++i) {
      if (i > 0) {
        MoveTo(commas[i - 1].first, new Vector3(leftSoFar + commaWidth * 0.5f, 0, 0), isAnimated);
        leftSoFar += commaWidth;
      }
      MoveTo(parameterObjects[i], new Vector3(leftSoFar + bounds[i].extents.x, 0, 0), isAnimated);
      leftSoFar += bounds[i].size.x;
    }

    // )
    MoveTo(close.first, new Vector3(leftSoFar + closeWidth * 0.5f, 0, 0), isAnimated);
    leftSoFar += closeWidth;

    Bounds b = new Bounds(Vector3.zero, new Vector3(1, 1, 0));

    Vector3 least = rator.first.GetComponent<BoxCollider2D>().bounds.min;
    least.x = -totalWidth * 0.5f;
    Vector3 most = close.first.GetComponent<BoxCollider2D>().bounds.max;
    most.x = totalWidth * 0.5f;

    b.SetMinMax(least, most);
    gameObject.GetComponent<BoxCollider2D>().size = b.size;
  }

  override public Expression GetHighestPrecedentSubexpression() {
    // The highest precedent subexpression is the leafmost, leftmost one. So,
    // we try the left child first. Failing that, the right child. Failing
    // that, we're it.
 
    foreach (Expression parameterExpression in parameterExpressions) {
      Expression winner = parameterExpression.GetHighestPrecedentSubexpression();
      if (winner != null) {
        return winner;
      }
    }

    return this;
  }

  override public string ToString() {
    string s = label + "(";

    for (int i = 0; i < parameterExpressions.Count; ++i) {
      if (i > 0) {
        s += ", ";
      }
      s += parameterExpressions[i].ToString();
    }

    s += ")";

    return s;
  }

  override public Expression Resolve(Expression toResolve) {
    for (int i = 0; i < parameterExpressions.Count; ++i) {
      if (toResolve == parameterExpressions[i]) {
        Expression replacementExpr = toResolve.Evaluate();
        GameObject replacementObject = replacementExpr.GenerateGameObject();
        replacementObject.transform.parent = gameObject.transform;
        replacementObject.transform.localPosition = parameterObjects[i].transform.localPosition;

        GameObject.Destroy(parameterObjects[i]);
        parameterObjects[i] = replacementObject;
        parameterExpressions[i] = replacementExpr;
        return this;
      }
    }

    if (toResolve == this) {
      Expression replacementExpr = toResolve.Evaluate();
      GameObject replacementObject = replacementExpr.GenerateGameObject();
      replacementObject.transform.parent = GameObject.Find("/ExpressionRoot").transform;
      replacementObject.transform.localPosition = gameObject.transform.localPosition;
      GameObject.Destroy(this.gameObject);
      return replacementExpr;
    } else {
      for (int i = 0; i < parameterExpressions.Count; ++i) {
        parameterExpressions[i] = parameterExpressions[i].Resolve(toResolve);
      }
      return this;
    }
  }

  override public bool IsLeaf() {
    return false;
  }
}

/* ------------------------------------------------------------------------- */

}
