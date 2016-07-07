using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionStringEndsWith : ExpressionMethodCall {
  public ExpressionStringEndsWith(Expression invoker,
                                  Expression suffix) : base(invoker, new List<Expression>() {suffix}, "endsWith") {
  }

  override public Expression Evaluate() {
    string s = ((ExpressionString) invokerExpression.Evaluate()).ToRawString();
    string suffix = ((ExpressionString) parameterExpressions[0].Evaluate()).ToRawString();
    return new ExpressionBoolean(s.EndsWith(suffix));
  }
}

/* ------------------------------------------------------------------------- */

}
