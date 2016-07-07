using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionStringIsEmpty : ExpressionMethodCall {
  public ExpressionStringIsEmpty(Expression invoker) : base(invoker, new List<Expression>() {}, "isEmpty") {
  }

  override public Expression Evaluate() {
    string s = ((ExpressionString) invokerExpression.Evaluate()).ToRawString();
    return new ExpressionBoolean(s.Length == 0);
  }
}

/* ------------------------------------------------------------------------- */

}
