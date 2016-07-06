using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public class ExpressionChar : ExpressionLiteral {
  private char i;

  public ExpressionChar(char i) : base("'" + i + "'") {
    this.i = i;
  }

  public char ToChar() {
    return i;
  }
}

/* ------------------------------------------------------------------------- */

}
