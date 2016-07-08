using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionStringSubstring1 : ExpressionMethodCall {
  public ExpressionStringSubstring1(Expression invoker,
                                    Expression start) : base(invoker, new List<Expression>() {start}, "substring") {
  }

  override public Expression Evaluate() {
    string s = ((ExpressionString) invokerExpression.Evaluate()).ToRawString();
    int i = ((ExpressionInteger) parameterExpressions[0].Evaluate()).ToInt();
    return new ExpressionString(s.Substring(i));
  }
}

/* ------------------------------------------------------------------------- */

}
