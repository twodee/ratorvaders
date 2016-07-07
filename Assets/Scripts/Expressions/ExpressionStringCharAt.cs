using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionStringCharAt : ExpressionMethodCall {
  public ExpressionStringCharAt(Expression invoker,
                          Expression index) : base(invoker, new List<Expression>() {index}, "charAt") {
  }

  override public Expression Evaluate() {
    string s = ((ExpressionString) invokerExpression.Evaluate()).ToRawString();
    int i = ((ExpressionInteger) parameterExpressions[0].Evaluate()).ToInt();
    return new ExpressionChar(s[i]);
  }
}

/* ------------------------------------------------------------------------- */

}
