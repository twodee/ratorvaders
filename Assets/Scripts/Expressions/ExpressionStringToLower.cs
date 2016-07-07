using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionStringToLower : ExpressionMethodCall {
  public ExpressionStringToLower(Expression invoker) : base(invoker, new List<Expression>() {}, "toLower") {
  }

  override public Expression Evaluate() {
    string s = ((ExpressionString) invokerExpression.Evaluate()).ToRawString();
    return new ExpressionString(s.ToLower());
  }
}

/* ------------------------------------------------------------------------- */

}
