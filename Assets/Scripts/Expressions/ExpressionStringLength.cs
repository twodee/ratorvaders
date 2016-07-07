using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionStringLength : ExpressionMethodCall {
  public ExpressionStringLength(Expression invoker) : base(invoker, new List<Expression>() {}, "length") {
  }

  override public Expression Evaluate() {
    string s = ((ExpressionString) invokerExpression.Evaluate()).ToRawString();
    return new ExpressionInteger(s.Length);
  }
}

/* ------------------------------------------------------------------------- */

}
