using TMPro;
using UnityEngine;

public class SliderAmountToText : MonoBehaviour
{
    [SerializeField] string _startText;
    [SerializeField] TMP_Text _UIText;
    private void Start()
    {
        OnSliderChange(1);
    }
    public void OnSliderChange(float value) // Esta funcion publica es para añadirse al Slider
    {
        float valueTimes100 = value * 100;
        int newValue = (int)valueTimes100;
        print(newValue);
        _UIText.text = _startText + ": " + newValue.ToString();
    }
}
