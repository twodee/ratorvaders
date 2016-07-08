using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionCastDouble : ExpressionPrefixUnaryOperator {
  public ExpressionCastDouble(Expression right) : base(right, Precedence.CAST, "(double)") {
  }

  override public Expression Evaluate() {
    float f = ((ExpressionNumber) right.Evaluate()).ToDouble();
    return new ExpressionDouble(f);
  }
}

/* ------------------------------------------------------------------------- */

}
