using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionAnd : ExpressionInfixBinaryOperator {
  public ExpressionAnd(Expression left,
                       Expression right) : base(left, right, Precedence.AND, "&&") {
  }

  override public Expression Evaluate() {
    bool l = ((ExpressionBoolean) left.Evaluate()).ToBool();
    bool r = ((ExpressionBoolean) right.Evaluate()).ToBool();
    return new ExpressionBoolean(l && r);
  }
}

/* ------------------------------------------------------------------------- */

}
