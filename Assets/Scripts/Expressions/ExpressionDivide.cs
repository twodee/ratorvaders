using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionDivide : ExpressionInfixBinaryOperator {
  public ExpressionDivide(Expression left,
                          Expression right) : base(left, right, Precedence.MULTIPLICATIVE, "/") {
  }
}

/* ------------------------------------------------------------------------- */

}
