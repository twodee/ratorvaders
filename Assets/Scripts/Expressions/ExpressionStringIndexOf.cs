using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionStringIndexOf : ExpressionMethodCall {
  public ExpressionStringIndexOf(Expression invoker,
                           Expression c) : base(invoker, new List<Expression>() {c}, "indexOf") {
  }

  override public Expression Evaluate() {
    string s = ((ExpressionString) invokerExpression.Evaluate()).ToRawString();
    char c = ((ExpressionChar) parameterExpressions[0].Evaluate()).ToChar();
    return new ExpressionInteger(s.IndexOf(c));
  }
}

/* ------------------------------------------------------------------------- */

}
