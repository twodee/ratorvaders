using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionNot : ExpressionPrefixUnaryOperator {
  public ExpressionNot(Expression right) : base(right, Precedence.NOT, "!") {
  }

  override public Expression Evaluate() {
    bool r = ((ExpressionBoolean) right.Evaluate()).ToBool();
    return new ExpressionBoolean(!r);
  }
}

/* ------------------------------------------------------------------------- */

}
