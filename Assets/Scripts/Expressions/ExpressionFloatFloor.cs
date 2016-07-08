using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionFloatFloor : ExpressionFunctionCall {
  public ExpressionFloatFloor(Expression e) : base(new List<Expression>() {e}, "floor") {
  }

  override public Expression Evaluate() {
    float a = Mathf.Floor(((ExpressionFloat) parameterExpressions[0].Evaluate()).ToFloat());
    return new ExpressionFloat(a);
  }
}

/* ------------------------------------------------------------------------- */

}
