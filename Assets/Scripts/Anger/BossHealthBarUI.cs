using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarUI : MonoBehaviour
{
    public Slider slider;

    public void SetMax(int value)
    {
        slider.maxValue = value;
        slider.value = value;
    }

    public void SetValue(int value)
    {
        slider.value = value;
    }
}
