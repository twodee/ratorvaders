using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionStringContains : ExpressionMethodCall {
  public ExpressionStringContains(Expression invoker,
                                  Expression needle) : base(invoker, new List<Expression>() {needle}, "contains") {
  }

  override public Expression Evaluate() {
    string s = ((ExpressionString) invokerExpression.Evaluate()).ToRawString();
    string prefix = ((ExpressionString) parameterExpressions[0].Evaluate()).ToRawString();
    return new ExpressionBoolean(s.Contains(prefix));
  }
}

/* ------------------------------------------------------------------------- */

}
