using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionString : ExpressionLiteral {
  private string i;

  public ExpressionString(string i) : base("\"" + i + "\"") {
    this.i = i;
  }

  override public string ToRawString() {
    return i;
  }
}

/* ------------------------------------------------------------------------- */

}
