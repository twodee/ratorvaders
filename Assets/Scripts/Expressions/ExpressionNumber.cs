using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public abstract class ExpressionNumber : ExpressionLiteral {
  public ExpressionNumber(string label) : base(label) {
  }

  public abstract float ToFloat();
}

/* ------------------------------------------------------------------------- */

}
