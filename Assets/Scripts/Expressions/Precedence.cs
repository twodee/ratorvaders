namespace RatorVaders {

public class Precedence {
  public static Precedence ADDITIVE = new Precedence(1, Association.LEFT);
  public static Precedence MULTIPLICATIVE = new Precedence(2, Association.LEFT);
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
