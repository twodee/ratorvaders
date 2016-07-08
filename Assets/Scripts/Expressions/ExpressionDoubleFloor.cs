using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionDoubleFloor : ExpressionFunctionCall {
  public ExpressionDoubleFloor(Expression e) : base(new List<Expression>() {e}, "floor") {
  }

  override public Expression Evaluate() {
    float a = Mathf.Floor(((ExpressionDouble) parameterExpressions[0].Evaluate()).ToDouble());
    return new ExpressionDouble(a);
  }
}

/* ------------------------------------------------------------------------- */

}
