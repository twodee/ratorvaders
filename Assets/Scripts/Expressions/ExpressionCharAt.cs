using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionCharAt : ExpressionMethodCall {
  public ExpressionCharAt(Expression invoker,
                          List<Expression> parameters) : base(invoker, parameters, "charAt") {
  }

  override public Expression Evaluate() {
    string s = ((ExpressionString) invokerExpression.Evaluate()).ToRawString();
    int i = ((ExpressionInteger) parameterExpressions[0].Evaluate()).ToInt();
    return new ExpressionChar(s[i]);
  }
}

/* ------------------------------------------------------------------------- */

}
