using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionAdd : ExpressionInfixBinaryOperator {
  public ExpressionAdd(Expression left,
                       Expression right) : base(left, right, Precedence.ADDITIVE, "+") {
  }
}

/* ------------------------------------------------------------------------- */

}
