using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionMultiply : ExpressionInfixBinaryOperator {
  public ExpressionMultiply(Expression left,
                            Expression right) : base(left, right, Precedence.MULTIPLICATIVE, "*") {
  }
}

/* ------------------------------------------------------------------------- */

}
