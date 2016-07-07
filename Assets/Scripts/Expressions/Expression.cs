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
    /* return new ExpressionIndexOf(new ExpressionConcat(new ExpressionString("cat"), new ExpressionAnd(new ExpressionBoolean(true), new ExpressionBoolean(false))), */
                                 /* new ExpressionChar('h')); */

    int integerWeight = ExpressionController.singleton.additiveWeight + ExpressionController.singleton.multiplicativeWeight + ExpressionController.singleton.stringIndexOfWeight + ExpressionController.singleton.minMaxWeight + ExpressionController.singleton.stringLengthWeight;
    int booleanWeight = ExpressionController.singleton.logicalWeight + ExpressionController.singleton.relationalWeight + ExpressionController.singleton.equalityWeight;
    int stringWeight = ExpressionController.singleton.stringConcatWeight + ExpressionController.singleton.stringToCaseWeight;
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
      return GenerateExpressionIntegerLiteral();
    } else {
      int totalWeight = ExpressionController.singleton.additiveWeight + ExpressionController.singleton.multiplicativeWeight + ExpressionController.singleton.stringIndexOfWeight + ExpressionController.singleton.minMaxWeight + ExpressionController.singleton.stringLengthWeight;
      int w = Random.Range(0, totalWeight);
      int nLevelsLeft = Random.Range(0, nLevels);
      int nLevelsRight = Random.Range(0, nLevels);

      if (w < ExpressionController.singleton.additiveWeight) {
        int operation = Random.Range(0, 2);
        if (operation == 0) {
          return new ExpressionAdd(GenerateExpressionInteger(nLevelsLeft),
                                   GenerateExpressionInteger(nLevelsRight));  
        } else {
          return new ExpressionSubtract(GenerateExpressionInteger(nLevelsLeft),
                                        GenerateExpressionInteger(nLevelsRight));  
        }
      } else if (w < ExpressionController.singleton.additiveWeight + ExpressionController.singleton.multiplicativeWeight) {
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
      } else if (w < ExpressionController.singleton.additiveWeight + ExpressionController.singleton.multiplicativeWeight + ExpressionController.singleton.stringIndexOfWeight) {
        return new ExpressionIndexOf(GenerateExpressionString(nLevelsLeft), GenerateExpressionChar(nLevelsRight - 1));
      } else if (w < ExpressionController.singleton.additiveWeight + ExpressionController.singleton.multiplicativeWeight + ExpressionController.singleton.stringIndexOfWeight + ExpressionController.singleton.minMaxWeight) {
        int operation = Random.Range(0, 2);
        if (operation == 0) {
          return new ExpressionMax(GenerateExpressionInteger(nLevelsLeft - 1), GenerateExpressionInteger(nLevelsRight - 1));
        } else {
          return new ExpressionMin(GenerateExpressionInteger(nLevelsLeft - 1), GenerateExpressionInteger(nLevelsRight - 1));
        }
      } else if (w < ExpressionController.singleton.additiveWeight + ExpressionController.singleton.multiplicativeWeight + ExpressionController.singleton.stringIndexOfWeight + ExpressionController.singleton.minMaxWeight + ExpressionController.singleton.stringLengthWeight) {
        return new ExpressionStringLength(GenerateExpressionString(nLevelsLeft));
      } else {
        return GenerateExpressionIntegerLiteral();
      }
    }
  }

  public static Expression GenerateExpressionIntegerLiteral() {
    int i = Random.Range(1, 10);
    return new ExpressionInteger(i);
  }

  public static Expression GenerateExpressionBoolean(int nLevels) {
    if (nLevels <= 0) {
      return GenerateExpressionBooleanLiteral();
    } else {
      int totalWeight = ExpressionController.singleton.logicalWeight + ExpressionController.singleton.relationalWeight + ExpressionController.singleton.equalityWeight;
      int w = Random.Range(0, totalWeight);
      int nLevelsLeft = Random.Range(0, nLevels);
      int nLevelsRight = Random.Range(0, nLevels);

      if (w < ExpressionController.singleton.logicalWeight) {
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
      } else if (w < ExpressionController.singleton.logicalWeight + ExpressionController.singleton.relationalWeight) {
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
      } else if (w < ExpressionController.singleton.logicalWeight + ExpressionController.singleton.relationalWeight + ExpressionController.singleton.equalityWeight) {
        int operation = Random.Range(0, 2);
        if (operation == 0) {
          return new ExpressionEqual(GenerateExpressionInteger(nLevelsLeft),
                                     GenerateExpressionInteger(nLevelsRight));  
        } else {
          return new ExpressionNotEqual(GenerateExpressionInteger(nLevelsLeft),
                                        GenerateExpressionInteger(nLevelsRight));  
        }
      } else {
        return GenerateExpressionBooleanLiteral();
      }
    }
  }

  public static Expression GenerateExpressionBooleanLiteral() {
    bool b = Random.Range(0, 2) == 0 ? false : true;
    return new ExpressionBoolean(b);
  }

  public static Expression GenerateExpressionString(int nLevels) {
    if (nLevels <= 0) {
      return GenerateExpressionStringLiteral();
    } else {
      int totalWeight = ExpressionController.singleton.stringConcatWeight + ExpressionController.singleton.stringToCaseWeight;
      int w = Random.Range(0, totalWeight);
      int nLevelsLeft = Random.Range(0, nLevels);
      int nLevelsRight = Random.Range(0, nLevels);

      if (w < ExpressionController.singleton.stringConcatWeight) {
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
      } else if (w < ExpressionController.singleton.stringConcatWeight + ExpressionController.singleton.stringToCaseWeight) {
        Expression s = GenerateExpressionString(nLevelsLeft);
        int operation = Random.Range(0, 2);
        if (operation == 0) {
          return new ExpressionStringToUpper(s);
        } else {
          return new ExpressionStringToLower(s);
        }
      } else {
        return GenerateExpressionStringLiteral();
      }
    }
  }

  public static Expression GenerateExpressionStringLiteral() {
    string[] words = {"dog", "cat", "zebra", "fish", "seven", "Bean"};
    int i = Random.Range(0, words.Length);
    return new ExpressionString(words[i]);
  }

  public static Expression GenerateExpressionChar(int nLevels) {
    if (nLevels <= 0) {
      return GenerateCharLiteral();
    } else {
      int totalWeight = ExpressionController.singleton.stringCharAtWeight;
      int w = Random.Range(0, totalWeight);
      int nLevelsLeft = Random.Range(0, nLevels);
      int nLevelsRight = Random.Range(0, nLevels - 1);

      if (w < ExpressionController.singleton.stringCharAtWeight) {
        return new ExpressionCharAt(GenerateExpressionString(nLevelsLeft), GenerateExpressionInteger(nLevelsRight));
      } else {
        return GenerateCharLiteral();
      }
    }
  }

  public static Expression GenerateCharLiteral() {
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
