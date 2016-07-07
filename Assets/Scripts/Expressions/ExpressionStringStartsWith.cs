using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionStringStartsWith : ExpressionMethodCall {
  public ExpressionStringStartsWith(Expression invoker,
                                    Expression prefix) : base(invoker, new List<Expression>() {prefix}, "startsWith") {
  }

  override public Expression Evaluate() {
    string s = ((ExpressionString) invokerExpression.Evaluate()).ToRawString();
    string prefix = ((ExpressionString) parameterExpressions[0].Evaluate()).ToRawString();
    return new ExpressionBoolean(s.StartsWith(prefix));
  }
}

/* ------------------------------------------------------------------------- */

}
