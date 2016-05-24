using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionMod : ExpressionInfixBinaryOperator {
  public ExpressionMod(Expression left,
                       Expression right) : base(left, right, Precedence.MULTIPLICATIVE, "%") {
  }
}

/* ------------------------------------------------------------------------- */

}
