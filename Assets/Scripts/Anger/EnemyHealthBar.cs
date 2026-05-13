using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(int value)
    {
        slider.maxValue = value;
        slider.value = value;
    }

    public void SetHealth(int value)
    {
        slider.value = value;
    }
}
