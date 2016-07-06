using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionInteger : ExpressionLiteral {
  private int i;

  public ExpressionInteger(int i) : base("" + i) {
    this.i = i;
  }

  public int ToInt() {
    return i;
  }
}

/* ------------------------------------------------------------------------- */

}
