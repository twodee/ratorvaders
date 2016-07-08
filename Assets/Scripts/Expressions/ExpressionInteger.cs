using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionInteger : ExpressionNumber {
  private int i;

  public ExpressionInteger(int i) : base("" + i) {
    this.i = i;
  }

  public int ToInt() {
    return i;
  }

  override public float ToFloat() {
    return i;
  }
}

/* ------------------------------------------------------------------------- */

}
