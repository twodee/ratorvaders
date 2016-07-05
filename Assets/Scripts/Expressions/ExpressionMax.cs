using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionMax : ExpressionFunctionCall {
  public ExpressionMax(List<Expression> parameters) : base(parameters, "max") {
  }

  override public Expression Evaluate() {
    int a = ((ExpressionInteger) parameterExpressions[0].Evaluate()).ToInt();
    int b = ((ExpressionInteger) parameterExpressions[1].Evaluate()).ToInt();
    return new ExpressionInteger(a > b ? a : b);
  }
}

/* ------------------------------------------------------------------------- */

}
