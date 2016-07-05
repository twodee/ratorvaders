using UnityEngine;
using UnityEngine.UI;

public class GuessBox : InputField {
  private bool isDeselectPending = false;

  override protected void LateUpdate() {
    base.LateUpdate();

    if (isDeselectPending) {
      MoveTextEnd(false);
      isDeselectPending = false;
    }
  }

  public void AppendCursor() {
    ActivateInputField();
    isDeselectPending = true;
  }

  public void Hide() {
    text = "";
    gameObject.SetActive(false);
  }
}
