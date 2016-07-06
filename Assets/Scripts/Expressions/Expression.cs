using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatorVaders {

/* ------------------------------------------------------------------------- */

public abstract class Expression {
  private Precedence _precedence;
  public GameObject gameObject;

  public static int additiveWeight = ExpressionController.singleton.additiveWeight;
  public static int multiplicativeWeight = ExpressionController.singleton.multiplicativeWeight;
  public static int logicalWeight = ExpressionController.singleton.logicalWeight;
  public static int relationalWeight = ExpressionController.singleton.relationalWeight;
  public static int equalityWeight = ExpressionController.singleton.equalityWeight;
  public static int concatWeight = ExpressionController.singleton.concatWeight;
  public static int indexOfWeight = ExpressionController.singleton.indexOfWeight;
  public static int charAtWeight = ExpressionController.singleton.charAtWeight;
  public static int minMaxWeight = ExpressionController.singleton.minMaxWeight;

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

  public abstract void Relayout(bool isAnimated);

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
    int integerWeight = additiveWeight + multiplicativeWeight + indexOfWeight + minMaxWeight;
    int booleanWeight = logicalWeight + relationalWeight + equalityWeight;
    int stringWeight = concatWeight;
    int totalWeight = integerWeight + booleanWeight + stringWeight;

    int w = Random.Range(0, totalWeight);

    if (w < integerWeight) {
      return GenerateExpressionInteger(nLevels);
    } else if (w < integerWeight + stringWeight) {
      return GenerateExpressionString(nLevels);
    } else {
      return GenerateExpressionBoolean(nLevels);
    }
  }

  public static Expression GenerateExpressionInteger(int nLevels) {
    if (nLevels <= 0) {
      int i = Random.Range(1, 10);
      return new ExpressionInteger(i);
    } else {
      int totalWeight = additiveWeight + multiplicativeWeight + indexOfWeight + minMaxWeight;
      int w = Random.Range(0, totalWeight);
      int nLevelsLeft = Random.Range(0, nLevels);
      int nLevelsRight = Random.Range(0, nLevels);

      if (w < additiveWeight) {
        int operation = Random.Range(0, 2);
        if (operation == 0) {
          return new ExpressionAdd(GenerateExpressionInteger(nLevelsLeft),
                                   GenerateExpressionInteger(nLevelsRight));  
        } else {
          return new ExpressionSubtract(GenerateExpressionInteger(nLevelsLeft),
                                        GenerateExpressionInteger(nLevelsRight));  
        }
      } else if (w < additiveWeight + multiplicativeWeight) {
        int operation = Random.Range(0, 3);
        if (operation == 0) {
          return new ExpressionMultiply(GenerateExpressionInteger(nLevelsLeft),
                                        GenerateExpressionInteger(nLevelsRight));  
        } else if (operation == 1) {
          return new ExpressionDivide(GenerateExpressionInteger(nLevelsLeft),
                                      GenerateExpressionInteger(nLevelsRight));  
        } else {
          return new ExpressionMod(GenerateExpressionInteger(nLevelsLeft),
                                   GenerateExpressionInteger(nLevelsRight));  
        }
      } else if (w < additiveWeight + multiplicativeWeight + indexOfWeight) {
        List<Expression> parameters = new List<Expression>();
        parameters.Add(GenerateExpressionChar(nLevelsRight));
        return new ExpressionIndexOf(GenerateExpressionString(nLevelsLeft), parameters);
      } else {
        int operation = Random.Range(0, 2);
        if (operation == 0) {
          List<Expression> parameters = new List<Expression>();
          parameters.Add(GenerateExpressionInteger(nLevelsLeft));
          parameters.Add(GenerateExpressionInteger(nLevelsRight));
          return new ExpressionMax(parameters);
        } else {
          List<Expression> parameters = new List<Expression>();
          parameters.Add(GenerateExpressionInteger(nLevelsLeft));
          parameters.Add(GenerateExpressionInteger(nLevelsRight));
          return new ExpressionMin(parameters);
        }
      }
    }
  }

  public static Expression GenerateExpressionBoolean(int nLevels) {
    if (nLevels <= 0) {
      bool b = Random.Range(0, 2) == 0 ? false : true;
      return new ExpressionBoolean(b);
    } else {
      int totalWeight = logicalWeight + relationalWeight + equalityWeight;
      int w = Random.Range(0, totalWeight);
      int nLevelsLeft = Random.Range(0, nLevels);
      int nLevelsRight = Random.Range(0, nLevels);

      if (w < logicalWeight) {
        int operation = Random.Range(0, 3);
        if (operation == 0) {
          return new ExpressionAnd(GenerateExpressionBoolean(nLevelsLeft),
                                   GenerateExpressionBoolean(nLevelsRight));  
        } else if (operation == 1) {
          return new ExpressionOr(GenerateExpressionBoolean(nLevelsLeft),
                                  GenerateExpressionBoolean(nLevelsRight));  
        } else {
          return new ExpressionNot(GenerateExpressionBoolean(nLevelsRight));  
        }
      } else if (w < logicalWeight + relationalWeight) {
        int operation = Random.Range(0, 4);
        if (operation == 0) {
          return new ExpressionGreaterThan(GenerateExpressionInteger(nLevelsLeft),
                                           GenerateExpressionInteger(nLevelsRight));  
        } else if (operation == 1) {
          return new ExpressionGreaterThanOrEqualTo(GenerateExpressionInteger(nLevelsLeft),
                                                    GenerateExpressionInteger(nLevelsRight));  
        } else if (operation == 2) {
          return new ExpressionLessThan(GenerateExpressionInteger(nLevelsLeft),
                                        GenerateExpressionInteger(nLevelsRight));  
        } else {
          return new ExpressionLessThanOrEqualTo(GenerateExpressionInteger(nLevelsLeft),
                                                 GenerateExpressionInteger(nLevelsRight));  
        }
      } else {
        int operation = Random.Range(0, 2);
        if (operation == 0) {
          return new ExpressionEqual(GenerateExpressionInteger(nLevelsLeft),
                                     GenerateExpressionInteger(nLevelsRight));  
        } else {
          return new ExpressionNotEqual(GenerateExpressionInteger(nLevelsLeft),
                                        GenerateExpressionInteger(nLevelsRight));  
        }
      }
    }
  }

  public static Expression GenerateExpressionString(int nLevels) {
    if (nLevels <= 0) {
      string[] words = {"dog", "cat", "zebra", "fish", "seven", "Bean"};
      int i = Random.Range(0, words.Length);
      return new ExpressionString(words[i]);
    } else {
      int totalWeight = concatWeight;
      int w = Random.Range(0, totalWeight);
      int nLevelsLeft = Random.Range(0, nLevels);
      int nLevelsRight = Random.Range(0, nLevels);

      if (true || w < concatWeight) {
        Expression l, r;
        int whichIsString = Random.Range(0, 2);
        if (whichIsString == 0) {
          l = GenerateExpression(nLevelsLeft);
          r = GenerateExpressionString(nLevelsRight);
        } else {
          l = GenerateExpressionString(nLevelsLeft);
          r = GenerateExpression(nLevelsRight);
        }
        return new ExpressionConcat(l, r);
      }
    }
  }

  public static Expression GenerateExpressionChar(int nLevels) {
    string chars = "abcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+:<>[]{}(),./?\\|~";
    int i = Random.Range(0, chars.Length);
    return new ExpressionChar(chars[i]);
  }

  virtual public string ToRawString() {
    return ToString();
  }
}

/* ------------------------------------------------------------------------- */

}
