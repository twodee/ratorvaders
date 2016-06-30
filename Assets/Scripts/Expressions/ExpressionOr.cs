using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionOr : ExpressionInfixBinaryOperator {
  public ExpressionOr(Expression left,
                      Expression right) : base(left, right, Precedence.OR, "||") {
  }

  override public Expression Evaluate() {
    bool l = ((ExpressionBoolean) left.Evaluate()).ToBool();
    bool r = ((ExpressionBoolean) right.Evaluate()).ToBool();
    return new ExpressionBoolean(l || r);
  }
}

/* ------------------------------------------------------------------------- */

}
