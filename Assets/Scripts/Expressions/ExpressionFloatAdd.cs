using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionFloatAdd : ExpressionInfixBinaryOperator {
  public ExpressionFloatAdd(Expression left,
                            Expression right) : base(left, right, Precedence.ADDITIVE, "+") {
  }

  override public Expression Evaluate() {
    float l = ((ExpressionNumber) left.Evaluate()).ToFloat();
    float r = ((ExpressionNumber) right.Evaluate()).ToFloat();
    return new ExpressionFloat(l + r);
  }
}

/* ------------------------------------------------------------------------- */

}
