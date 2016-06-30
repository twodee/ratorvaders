using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionNotEqual : ExpressionInfixBinaryOperator {
  public ExpressionNotEqual(Expression left,
                            Expression right) : base(left, right, Precedence.EQUALITY, "!=") {
  }

  override public Expression Evaluate() {
    int l = ((ExpressionInteger) left.Evaluate()).ToInt();
    int r = ((ExpressionInteger) right.Evaluate()).ToInt();
    return new ExpressionBoolean(l != r);
  }
}

/* ------------------------------------------------------------------------- */

}
