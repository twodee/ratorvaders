using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionBoolean : ExpressionLiteral {
  private bool i;

  public ExpressionBoolean(bool i) : base(i ? "true" : "false") {
    this.i = i;
  }

  public bool ToBool() {
    return i;
  }
}

/* ------------------------------------------------------------------------- */

}
