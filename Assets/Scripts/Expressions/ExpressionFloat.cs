using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionFloat : ExpressionNumber {
  private float i;

  public ExpressionFloat(float i) : base(string.Format("{0:F1}", i)) {
    this.i = i;
  }

  override public float ToFloat() {
    return i;
  }

  override public string ToString() {
    return string.Format("{0:F1}", i);
  }
}

/* ------------------------------------------------------------------------- */

}
