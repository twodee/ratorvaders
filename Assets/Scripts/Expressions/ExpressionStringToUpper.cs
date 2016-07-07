using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionStringToUpper : ExpressionMethodCall {
  public ExpressionStringToUpper(Expression invoker) : base(invoker, new List<Expression>() {}, "toUpper") {
  }

  override public Expression Evaluate() {
    string s = ((ExpressionString) invokerExpression.Evaluate()).ToRawString();
    return new ExpressionString(s.ToUpper());
  }
}

/* ------------------------------------------------------------------------- */

}
