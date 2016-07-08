using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionStringSubstring2 : ExpressionMethodCall {
  public ExpressionStringSubstring2(Expression invoker,
                                    Expression start,
                                    Expression end) : base(invoker, new List<Expression>() {start, end}, "substring") {
  }

  override public Expression Evaluate() {
    string s = ((ExpressionString) invokerExpression.Evaluate()).ToRawString();
    int i = ((ExpressionInteger) parameterExpressions[0].Evaluate()).ToInt();
    int j = ((ExpressionInteger) parameterExpressions[1].Evaluate()).ToInt();
    return new ExpressionString(s.Substring(i, j - i));
  }
}

/* ------------------------------------------------------------------------- */

}
