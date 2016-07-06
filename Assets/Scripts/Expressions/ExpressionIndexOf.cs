using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionIndexOf : ExpressionMethodCall {
  public ExpressionIndexOf(Expression invoker,
                           List<Expression> parameters) : base(invoker, parameters, "indexOf") {
  }

  override public Expression Evaluate() {
    string s = ((ExpressionString) invokerExpression.Evaluate()).ToUnquotedString();
    char c = ((ExpressionChar) parameterExpressions[0].Evaluate()).ToChar();
    return new ExpressionInteger(s.IndexOf(c));
  }
}

/* ------------------------------------------------------------------------- */

}
