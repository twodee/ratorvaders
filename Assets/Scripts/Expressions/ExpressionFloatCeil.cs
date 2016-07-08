using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionFloatCeil : ExpressionFunctionCall {
  public ExpressionFloatCeil(Expression e) : base(new List<Expression>() {e}, "ceil") {
  }

  override public Expression Evaluate() {
    float a = Mathf.Ceil(((ExpressionFloat) parameterExpressions[0].Evaluate()).ToFloat());
    return new ExpressionFloat(a);
  }
}

/* ------------------------------------------------------------------------- */

}
