﻿using UnityEngine;
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
    int integerWeight = ExpressionController.singleton.additiveWeight + ExpressionController.singleton.multiplicativeWeight + ExpressionController.singleton.stringIndexOfWeight + ExpressionController.singleton.minMaxWeight + ExpressionController.singleton.stringLengthWeight + ExpressionController.singleton.intCastWeight;
    int booleanWeight = ExpressionController.singleton.logicalWeight + ExpressionController.singleton.relationalWeight + ExpressionController.singleton.equalityWeight + ExpressionController.singleton.stringContainsWeight + ExpressionController.singleton.stringIsEmptyWeight;
    int stringWeight = ExpressionController.singleton.stringConcatWeight + ExpressionController.singleton.stringToCaseWeight + ExpressionController.singleton.stringSubstring1Weight + ExpressionController.singleton.stringSubstring2Weight;
    int doubleWeight = ExpressionController.singleton.doubleCeilFloorWeight + ExpressionController.singleton.doubleAdditiveWeight + ExpressionController.singleton.doubleCastWeight;
    int charWeight = ExpressionController.singleton.stringCharAtWeight;
    int totalWeight = integerWeight + booleanWeight + stringWeight + doubleWeight + charWeight;

    int w = Random.Range(0, totalWeight);

    if (w < integerWeight) {
      return GenerateExpressionInteger(nLevels);
    } else if (w < integerWeight + stringWeight) {
      return GenerateExpressionString(nLevels);
    } else if (w < integerWeight + stringWeight + booleanWeight) {
      return GenerateExpressionBoolean(nLevels);
    } else if (w < integerWeight + stringWeight + booleanWeight + doubleWeight) {
      return GenerateExpressionDouble(nLevels);
    } else {
      return GenerateExpressionChar(nLevels);
    }
  }

  public static Expression GenerateExpressionInteger(int nLevels) {
    if (nLevels <= 0) {
      return GenerateExpressionIntegerLiteral();
    } else {
      int totalWeight = ExpressionController.singleton.additiveWeight + ExpressionController.singleton.multiplicativeWeight + ExpressionController.singleton.stringIndexOfWeight + ExpressionController.singleton.minMaxWeight + ExpressionController.singleton.stringLengthWeight + ExpressionController.singleton.intCastWeight;
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
        int operation = Random.Range(0, 2);
        if (operation == 0) {
          return new ExpressionStringIndexOf(GenerateExpressionString(nLevelsLeft), GenerateExpressionChar(nLevelsRight - 1));
        } else {
          return new ExpressionStringLastIndexOf(GenerateExpressionString(nLevelsLeft), GenerateExpressionChar(nLevelsRight - 1));
        }
      } else if (w < ExpressionController.singleton.additiveWeight + ExpressionController.singleton.multiplicativeWeight + ExpressionController.singleton.stringIndexOfWeight + ExpressionController.singleton.minMaxWeight) {
        int operation = Random.Range(0, 2);
        if (operation == 0) {
          return new ExpressionMax(GenerateExpressionInteger(nLevelsLeft - 1), GenerateExpressionInteger(nLevelsRight - 1));
        } else {
          return new ExpressionMin(GenerateExpressionInteger(nLevelsLeft - 1), GenerateExpressionInteger(nLevelsRight - 1));
        }
      } else if (w < ExpressionController.singleton.additiveWeight + ExpressionController.singleton.multiplicativeWeight + ExpressionController.singleton.stringIndexOfWeight + ExpressionController.singleton.minMaxWeight + ExpressionController.singleton.stringLengthWeight) {
        return new ExpressionStringLength(GenerateExpressionString(nLevelsLeft));
      } else if (w < ExpressionController.singleton.additiveWeight + ExpressionController.singleton.multiplicativeWeight + ExpressionController.singleton.stringIndexOfWeight + ExpressionController.singleton.minMaxWeight + ExpressionController.singleton.stringLengthWeight + ExpressionController.singleton.intCastWeight) {
        return new ExpressionCastInt(GenerateExpressionDouble(nLevelsRight));
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
      int totalWeight = ExpressionController.singleton.logicalWeight + ExpressionController.singleton.relationalWeight + ExpressionController.singleton.equalityWeight + ExpressionController.singleton.stringIsEmptyWeight;
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
      } else if (w < ExpressionController.singleton.logicalWeight + ExpressionController.singleton.relationalWeight + ExpressionController.singleton.equalityWeight + ExpressionController.singleton.stringContainsWeight) {
        int operation = Random.Range(0, 3);
        if (operation == 0) {
          return new ExpressionStringStartsWith(GenerateExpressionString(nLevelsLeft),
                                                GenerateExpressionString(nLevelsRight));  
        } else if (operation == 1) {
          return new ExpressionStringEndsWith(GenerateExpressionString(nLevelsLeft),
                                              GenerateExpressionString(nLevelsRight));  
        } else {
          return new ExpressionStringContains(GenerateExpressionString(nLevelsLeft),
                                              GenerateExpressionString(nLevelsRight));  
        }
      } else if (w < ExpressionController.singleton.logicalWeight + ExpressionController.singleton.relationalWeight + ExpressionController.singleton.equalityWeight + ExpressionController.singleton.stringContainsWeight + ExpressionController.singleton.stringIsEmptyWeight) {
        return new ExpressionStringIsEmpty(GenerateExpressionString(nLevelsLeft));
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
      int totalWeight = ExpressionController.singleton.stringConcatWeight + ExpressionController.singleton.stringToCaseWeight + ExpressionController.singleton.stringSubstring1Weight + ExpressionController.singleton.stringSubstring2Weight;
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
      } else if (w < ExpressionController.singleton.stringConcatWeight + ExpressionController.singleton.stringToCaseWeight + ExpressionController.singleton.stringSubstring1Weight) {
        Expression stringExpression = GenerateExpressionString(nLevelsLeft);
        string stringValue = ((ExpressionString) stringExpression.Evaluate()).ToRawString();
        int index = Random.Range(0, stringValue.Length + 1);
        return new ExpressionStringSubstring1(stringExpression, new ExpressionInteger(index));
      } else if (w < ExpressionController.singleton.stringConcatWeight + ExpressionController.singleton.stringToCaseWeight + ExpressionController.singleton.stringSubstring1Weight + ExpressionController.singleton.stringSubstring2Weight) {
        Expression stringExpression = GenerateExpressionString(nLevelsLeft);
        string stringValue = ((ExpressionString) stringExpression.Evaluate()).ToRawString();
        int startIndex = Random.Range(0, stringValue.Length);
        int endIndex = Random.Range(startIndex, stringValue.Length + 1);
        return new ExpressionStringSubstring2(stringExpression, new ExpressionInteger(startIndex), new ExpressionInteger(endIndex));
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
        return new ExpressionStringCharAt(GenerateExpressionString(nLevelsLeft), GenerateExpressionInteger(nLevelsRight));
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

  public static Expression GenerateExpressionDouble(int nLevels) {
    if (nLevels <= 0) {
      return GenerateDoubleLiteral();
    } else {
      int totalWeight = ExpressionController.singleton.doubleCeilFloorWeight + ExpressionController.singleton.doubleAdditiveWeight + ExpressionController.singleton.doubleCastWeight;
      int w = Random.Range(0, totalWeight);
      int nLevelsLeft = Random.Range(0, nLevels);
      int nLevelsRight = Random.Range(0, nLevels);

      if (w < ExpressionController.singleton.doubleCeilFloorWeight) {
        int operation = Random.Range(0, 2);
        if (operation == 0) {
          return new ExpressionDoubleCeil(GenerateExpressionDouble(nLevelsLeft));
        } else {
          return new ExpressionDoubleFloor(GenerateExpressionDouble(nLevelsLeft));
        }
      } else if (w < ExpressionController.singleton.doubleCeilFloorWeight + ExpressionController.singleton.doubleAdditiveWeight) {
        Expression l, r;
        int whichIsDouble = Random.Range(0, 2);
        if (whichIsDouble == 0) {
          l = GenerateExpressionInteger(nLevelsLeft);
          r = GenerateExpressionDouble(nLevelsRight);
        } else {
          l = GenerateExpressionDouble(nLevelsLeft);
          r = GenerateExpressionInteger(nLevelsRight);
        }
        return new ExpressionDoubleAdd(l, r);
      } else if (w < ExpressionController.singleton.doubleCeilFloorWeight + ExpressionController.singleton.doubleAdditiveWeight + ExpressionController.singleton.doubleCastWeight) {
        return new ExpressionCastDouble(GenerateExpressionInteger(nLevelsRight));
      } else {
        return GenerateDoubleLiteral();
      }
    }
  }

  public static Expression GenerateDoubleLiteral() {
    float f = Random.Range(-100, 100) / 10.0f;
    return new ExpressionDouble(f);
  }

  virtual public string ToRawString() {
    return ToString();
  }
}

/* ------------------------------------------------------------------------- */

}
