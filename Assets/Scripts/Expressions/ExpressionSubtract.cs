using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionSubtract : ExpressionInfixBinaryOperator {
  public ExpressionSubtract(Expression left,
                            Expression right) : base(left, right, Precedence.ADDITIVE, "-") {
  }

  override public Expression Evaluate() {
    int l = ((ExpressionInteger) left.Evaluate()).ToInt();
    int r = ((ExpressionInteger) right.Evaluate()).ToInt();
    return new ExpressionInteger(l - r);
  }
}

/* ------------------------------------------------------------------------- */

}
