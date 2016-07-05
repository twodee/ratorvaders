using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public abstract class Expression {
  private Precedence _precedence;
  public GameObject gameObject;

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

  public static void MoveTo(GameObject movee, Vector3 to, bool isAnimated) {
    if (isAnimated) {
      ExpressionController.singleton.StartCoroutine(MoveToAnimated(movee, movee.transform.localPosition, to));
    } else {
      movee.transform.localPosition = to;
    }
  }

  public static IEnumerator MoveToAnimated(GameObject movee, Vector3 from, Vector3 to) {
    float startTime = Time.time;
    float endTime = startTime + 0.3f;
    float proportion = 0.0f;

    do {
      float currentTime = Time.time;
      proportion = (currentTime - startTime) / (endTime - startTime);
      movee.transform.localPosition = Vector3.Lerp(from, to, proportion);
      yield return null;
    } while (proportion <= 1.0f);
  }

  public static Expression GenerateExpression(int nLevels) {
    int ntypes = 2;
    int type = Random.Range(0, ntypes);

    if (type == 0) {
      return GenerateExpressionInteger(nLevels);
    } else {
      return GenerateExpressionBoolean(nLevels);
    }
  }

  public static Expression GenerateExpressionInteger(int nLevels) {
    if (nLevels <= 0) {
      int i = Random.Range(1, 10);
      return new ExpressionInteger(i);
    } else {
      int nOperations = 7;
      int operation = Random.Range(0, nOperations);
      int nLevelsLeft = Random.Range(0, nLevels);
      int nLevelsRight = Random.Range(0, nLevels);

      if (operation == 0) {
        return new ExpressionMultiply(GenerateExpressionInteger(nLevelsLeft),
                                      GenerateExpressionInteger(nLevelsRight));  
      } else if (operation == 1) {
        return new ExpressionDivide(GenerateExpressionInteger(nLevelsLeft),
                                    GenerateExpressionInteger(nLevelsRight));  
      } else if (operation == 2) {
        return new ExpressionMod(GenerateExpressionInteger(nLevelsLeft),
                                 GenerateExpressionInteger(nLevelsRight));  
      } else if (operation == 3) {
        return new ExpressionAdd(GenerateExpressionInteger(nLevelsLeft),
                                 GenerateExpressionInteger(nLevelsRight));  
      } else if (operation == 4) {
        return new ExpressionSubtract(GenerateExpressionInteger(nLevelsLeft),
                                      GenerateExpressionInteger(nLevelsRight));  
      } else if (operation == 5) {
        List<Expression> parameters = new List<Expression>();
        parameters.Add(GenerateExpressionInteger(nLevelsRight));
        parameters.Add(GenerateExpressionInteger(nLevelsRight));
        return new ExpressionMax(parameters);
      } else {
        List<Expression> parameters = new List<Expression>();
        parameters.Add(GenerateExpressionInteger(nLevelsRight));
        parameters.Add(GenerateExpressionInteger(nLevelsRight));
        return new ExpressionMin(parameters);
      }
    }
  }

  public static Expression GenerateExpressionBoolean(int nLevels) {
    if (nLevels <= 0) {
      bool b = Random.Range(0, 2) == 0 ? false : true;
      return new ExpressionBoolean(b);
    } else {
      int nOperations = 7;
      int operation = Random.Range(0, nOperations);
      int nLevelsLeft = Random.Range(0, nLevels);
      int nLevelsRight = Random.Range(0, nLevels);

      if (operation == 0) {
        return new ExpressionAnd(GenerateExpressionBoolean(nLevelsLeft),
                                 GenerateExpressionBoolean(nLevelsRight));  
      } else if (operation == 1) {
        return new ExpressionOr(GenerateExpressionBoolean(nLevelsLeft),
                                GenerateExpressionBoolean(nLevelsRight));  
      } else if (operation == 2) {
        return new ExpressionGreaterThan(GenerateExpressionInteger(nLevelsLeft),
                                         GenerateExpressionInteger(nLevelsRight));  
      } else if (operation == 3) {
        return new ExpressionGreaterThanOrEqualTo(GenerateExpressionInteger(nLevelsLeft),
                                                  GenerateExpressionInteger(nLevelsRight));  
      } else if (operation == 4) {
        return new ExpressionLessThan(GenerateExpressionInteger(nLevelsLeft),
                                      GenerateExpressionInteger(nLevelsRight));  
      } else if (operation == 5) {
        return new ExpressionLessThanOrEqualTo(GenerateExpressionInteger(nLevelsLeft),
                                               GenerateExpressionInteger(nLevelsRight));  
      } else {
        return new ExpressionNot(GenerateExpressionBoolean(nLevelsRight));  
      }
    }
  }
}

/* ------------------------------------------------------------------------- */

}
