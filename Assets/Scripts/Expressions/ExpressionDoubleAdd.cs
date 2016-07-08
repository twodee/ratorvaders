using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionDoubleAdd : ExpressionInfixBinaryOperator {
  public ExpressionDoubleAdd(Expression left,
                             Expression right) : base(left, right, Precedence.ADDITIVE, "+") {
  }

  override public Expression Evaluate() {
    float l = ((ExpressionNumber) left.Evaluate()).ToDouble();
    float r = ((ExpressionNumber) right.Evaluate()).ToDouble();
    return new ExpressionDouble(l + r);
  }
}

/* ------------------------------------------------------------------------- */

}
