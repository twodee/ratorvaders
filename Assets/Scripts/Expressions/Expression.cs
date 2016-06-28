using UnityEngine;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public abstract class Expression {
  private Precedence _precedence;

  public Precedence precedence {
    set {
      _precedence = value;
    }
    get {
      return _precedence;
    }
  }

  public Expression(Precedence precedence) {
    _precedence = precedence;
  }

  public abstract GameObject GenerateGameObject();

  virtual public Expression Evaluate() {
    return this;
  }
  
  virtual public Expression GetHighestPrecedentSubexpression() {
    return null;
  }

  virtual public Expression Resolve(Expression toResolve) {
    return this;
  }

  virtual public void Relayout(float height, bool isAnimated) {
  }

  public abstract bool IsLeaf();
}

/* ------------------------------------------------------------------------- */

}
