namespace RatorVaders {

public class Precedence {

  public static Precedence OR = new Precedence(9, Association.LEFT);
  public static Precedence AND = new Precedence(10, Association.LEFT);
  public static Precedence EQUALITY = new Precedence(11, Association.LEFT);
  public static Precedence RELATIONAL = new Precedence(13, Association.LEFT);
  public static Precedence ADDITIVE = new Precedence(15, Association.LEFT);
  public static Precedence MULTIPLICATIVE = new Precedence(20, Association.LEFT);
  public static Precedence NOT = new Precedence(50, Association.LEFT);
  public static Precedence CAST = new Precedence(60, Association.LEFT);
  public static Precedence FUNCTION_CALL = new Precedence(70, Association.LEFT);
  public static Precedence LITERAL = new Precedence(100, Association.LEFT);

  public enum Association {
    LEFT,
    RIGHT
  };

  private int level;
  /* private Association association; */

  private Precedence(int level, Association association) {
    this.level = level;
    /* this.association = association; */
  }

  public int CompareTo(Precedence other) {
    return level - other.level;
  }
}

}
