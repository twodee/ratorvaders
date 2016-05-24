using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionSubtract : ExpressionInfixBinaryOperator {
  public ExpressionSubtract(Expression left,
                            Expression right) : base(left, right, Precedence.ADDITIVE, "-") {
  }
}

/* ------------------------------------------------------------------------- */

}
