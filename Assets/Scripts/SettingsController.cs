using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsController : MonoBehaviour {
  public GameObject sliderPrefab;
  public GameObject labelPrefab;
  public Transform canvas;

  void Start() {
    string[] labels = {
      "int + -",
      "int * / %",
      "boolean && || !",
      ">= > <= <",
      "== !=",
      "concatenation",
      "indexOf",
      "charAt",
      "min max",
      "length",
      "toUpper toLower",
      "contains",
      "isEmpty",
      "substring1",
      "substring2",
      "double +",
      "ceil/floor",
      "(double)",
      "(int)"
    };

    GameObject[] labelObjects = new GameObject[labels.Length];
    GameObject[] sliderObjects = new GameObject[labels.Length];

    for (int i = 0; i < labels.Length; ++i) {
      GameObject label = (GameObject) Instantiate(labelPrefab);
      RectTransform labelTransform = label.GetComponent<RectTransform>();
      label.GetComponent<Text>().text = labels[i];
      labelTransform.SetParent(canvas, false);
      labelTransform.anchoredPosition = new Vector2(-250, i * -35 + 320);

      GameObject slider = (GameObject) Instantiate(sliderPrefab);
      RectTransform sliderTransform = slider.GetComponent<RectTransform>();
      sliderTransform.SetParent(canvas, false);
      sliderTransform.anchoredPosition = new Vector2(100, i * -35 + 320);

      labelObjects[i] = label;
      sliderObjects[i] = slider;
    }

    sliderObjects[0].GetComponent<Slider>().value = ExpressionController.singleton.additiveWeight;
    sliderObjects[1].GetComponent<Slider>().value = ExpressionController.singleton.multiplicativeWeight;
    sliderObjects[2].GetComponent<Slider>().value = ExpressionController.singleton.logicalWeight;
    sliderObjects[3].GetComponent<Slider>().value = ExpressionController.singleton.relationalWeight;
    sliderObjects[4].GetComponent<Slider>().value = ExpressionController.singleton.equalityWeight;
    sliderObjects[5].GetComponent<Slider>().value = ExpressionController.singleton.stringConcatWeight;
    sliderObjects[6].GetComponent<Slider>().value = ExpressionController.singleton.stringIndexOfWeight;
    sliderObjects[7].GetComponent<Slider>().value = ExpressionController.singleton.stringCharAtWeight;
    sliderObjects[8].GetComponent<Slider>().value = ExpressionController.singleton.minMaxWeight;
    sliderObjects[9].GetComponent<Slider>().value = ExpressionController.singleton.stringLengthWeight;
    sliderObjects[10].GetComponent<Slider>().value = ExpressionController.singleton.stringToCaseWeight;
    sliderObjects[11].GetComponent<Slider>().value = ExpressionController.singleton.stringContainsWeight;
    sliderObjects[12].GetComponent<Slider>().value = ExpressionController.singleton.stringIsEmptyWeight;
    sliderObjects[13].GetComponent<Slider>().value = ExpressionController.singleton.stringSubstring1Weight;
    sliderObjects[14].GetComponent<Slider>().value = ExpressionController.singleton.stringSubstring2Weight;
    sliderObjects[15].GetComponent<Slider>().value = ExpressionController.singleton.doubleAdditiveWeight;
    sliderObjects[16].GetComponent<Slider>().value = ExpressionController.singleton.doubleCeilFloorWeight;
    sliderObjects[17].GetComponent<Slider>().value = ExpressionController.singleton.doubleCastWeight;
    sliderObjects[18].GetComponent<Slider>().value = ExpressionController.singleton.intCastWeight;

    sliderObjects[0].GetComponent<Slider>().onValueChanged.AddListener(delegate (float value) { ExpressionController.singleton.additiveWeight = (int) value; });
    sliderObjects[1].GetComponent<Slider>().onValueChanged.AddListener(delegate (float value) { ExpressionController.singleton.multiplicativeWeight = (int) value; });
    sliderObjects[2].GetComponent<Slider>().onValueChanged.AddListener(delegate (float value) { ExpressionController.singleton.logicalWeight = (int) value; });
    sliderObjects[3].GetComponent<Slider>().onValueChanged.AddListener(delegate (float value) { ExpressionController.singleton.relationalWeight = (int) value; });
    sliderObjects[4].GetComponent<Slider>().onValueChanged.AddListener(delegate (float value) { ExpressionController.singleton.equalityWeight = (int) value; });
    sliderObjects[5].GetComponent<Slider>().onValueChanged.AddListener(delegate (float value) { ExpressionController.singleton.stringConcatWeight = (int) value; });
    sliderObjects[6].GetComponent<Slider>().onValueChanged.AddListener(delegate (float value) { ExpressionController.singleton.stringIndexOfWeight = (int) value; });
    sliderObjects[7].GetComponent<Slider>().onValueChanged.AddListener(delegate (float value) { ExpressionController.singleton.stringCharAtWeight = (int) value; });
    sliderObjects[8].GetComponent<Slider>().onValueChanged.AddListener(delegate (float value) { ExpressionController.singleton.minMaxWeight = (int) value; });
    sliderObjects[9].GetComponent<Slider>().onValueChanged.AddListener(delegate (float value) { ExpressionController.singleton.stringLengthWeight = (int) value; });
    sliderObjects[10].GetComponent<Slider>().onValueChanged.AddListener(delegate (float value) { ExpressionController.singleton.stringToCaseWeight = (int) value; });
    sliderObjects[11].GetComponent<Slider>().onValueChanged.AddListener(delegate (float value) { ExpressionController.singleton.stringContainsWeight = (int) value; });
    sliderObjects[12].GetComponent<Slider>().onValueChanged.AddListener(delegate (float value) { ExpressionController.singleton.stringIsEmptyWeight = (int) value; });
    sliderObjects[13].GetComponent<Slider>().onValueChanged.AddListener(delegate (float value) { ExpressionController.singleton.stringSubstring1Weight = (int) value; });
    sliderObjects[14].GetComponent<Slider>().onValueChanged.AddListener(delegate (float value) { ExpressionController.singleton.stringSubstring2Weight = (int) value; });
    sliderObjects[15].GetComponent<Slider>().onValueChanged.AddListener(delegate (float value) { ExpressionController.singleton.doubleAdditiveWeight = (int) value; });
    sliderObjects[16].GetComponent<Slider>().onValueChanged.AddListener(delegate (float value) { ExpressionController.singleton.doubleCeilFloorWeight = (int) value; });
    sliderObjects[17].GetComponent<Slider>().onValueChanged.AddListener(delegate (float value) { ExpressionController.singleton.doubleCastWeight = (int) value; });
    sliderObjects[18].GetComponent<Slider>().onValueChanged.AddListener(delegate (float value) { ExpressionController.singleton.intCastWeight = (int) value; });
  }
  
  void Update() {
  
  }
}
