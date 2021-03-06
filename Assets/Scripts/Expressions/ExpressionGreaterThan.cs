using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionGreaterThan : ExpressionInfixBinaryOperator {
  public ExpressionGreaterThan(Expression left,
                               Expression right) : base(left, right, Precedence.RELATIONAL, ">") {
  }

  override public Expression Evaluate() {
    int l = ((ExpressionInteger) left.Evaluate()).ToInt();
    int r = ((ExpressionInteger) right.Evaluate()).ToInt();
    return new ExpressionBoolean(l > r);
  }
}

/* ------------------------------------------------------------------------- */

}
