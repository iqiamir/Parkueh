using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider healthbarSlider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxHealth(int health)
    {
        healthbarSlider.maxValue = health;
        healthbarSlider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        healthbarSlider.value = health;

        fill.color = gradient.Evaluate(healthbarSlider.normalizedValue);
    }
}
