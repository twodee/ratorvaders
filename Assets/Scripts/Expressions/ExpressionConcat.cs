using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionConcat : ExpressionInfixBinaryOperator {
  public ExpressionConcat(Expression left,
                          Expression right) : base(left, right, Precedence.ADDITIVE, "+") {
  }

  override public Expression Evaluate() {
    string l = left.Evaluate().ToRawString();
    string r = right.Evaluate().ToRawString();
    Debug.Log("l + r: " + l + r);
    return new ExpressionString(l + r);
  }
}

/* ------------------------------------------------------------------------- */

}
