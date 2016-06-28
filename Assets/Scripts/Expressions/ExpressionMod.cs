using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionMod : ExpressionInfixBinaryOperator {
  public ExpressionMod(Expression left,
                       Expression right) : base(left, right, Precedence.MULTIPLICATIVE, "%") {
  }

  override public Expression Evaluate() {
    int l = ((ExpressionInteger) left.Evaluate()).ToInt();
    int r = ((ExpressionInteger) right.Evaluate()).ToInt();
    return new ExpressionInteger(l % r);
  }
}

/* ------------------------------------------------------------------------- */

}
