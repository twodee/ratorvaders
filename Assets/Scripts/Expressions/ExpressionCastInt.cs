using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionCastInt : ExpressionPrefixUnaryOperator {
  public ExpressionCastInt(Expression right) : base(right, Precedence.CAST, "(int)") {
  }

  override public Expression Evaluate() {
    int f = ((ExpressionNumber) right.Evaluate()).ToInt();
    return new ExpressionInteger(f);
  }
}

/* ------------------------------------------------------------------------- */

}
