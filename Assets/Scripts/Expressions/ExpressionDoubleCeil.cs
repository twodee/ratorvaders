using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionDoubleCeil : ExpressionFunctionCall {
  public ExpressionDoubleCeil(Expression e) : base(new List<Expression>() {e}, "ceil") {
  }

  override public Expression Evaluate() {
    float a = Mathf.Ceil(((ExpressionDouble) parameterExpressions[0].Evaluate()).ToDouble());
    return new ExpressionDouble(a);
  }
}

/* ------------------------------------------------------------------------- */

}
