using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionDouble : ExpressionNumber {
  private float i;

  public ExpressionDouble(float i) : base(string.Format("{0:F1}", i)) {
    this.i = i;
  }

  override public float ToDouble() {
    return i;
  }

  override public int ToInt() {
    return (int) i;
  }

  override public string ToString() {
    return string.Format("{0:F1}", i);
  }
}

/* ------------------------------------------------------------------------- */

}
