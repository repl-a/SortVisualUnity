using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {
    public SortManager sortManager;
    public Button sortButton;
    public Button newArrayButton;
    public TMP_Text arraySizeText; 
    public TMP_Text sortingSpeedText; 
    public Slider arraySizeSlider;
    public Slider speedSlider; 

    void Start() {
        sortButton.onClick.AddListener(sortManager.StartSorting);
        newArrayButton.onClick.AddListener(sortManager.InitializeArray);

        UpdateTexts();

        arraySizeSlider.onValueChanged.AddListener(value => UpdateTexts());
        speedSlider.onValueChanged.AddListener(value => UpdateTexts());
    }

    void UpdateTexts() {
        arraySizeText.text = $"Array Size: {Mathf.RoundToInt(arraySizeSlider.value)}";
        sortingSpeedText.text = $"Speed: {Mathf.RoundToInt(speedSlider.value)}";
    }
}
