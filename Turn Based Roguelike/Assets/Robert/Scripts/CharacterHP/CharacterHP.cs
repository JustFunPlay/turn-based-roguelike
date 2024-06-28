using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHP : MonoBehaviour
{
    [SerializeField] private Slider hPSlider;
    [SerializeField] private Slider resourceSlider;
    public void SetMainCharMaxSliders(int maxHealth, int maxResource)
    {
        hPSlider.maxValue = maxHealth;
        hPSlider.value = maxHealth;

        resourceSlider.maxValue = maxResource;
        resourceSlider.value = maxResource;
    }
    public void MainTakeDamage(float currentHealth)
    {
        hPSlider.value = currentHealth;
    }
    public void LoseResource(float currentResource)
    {
        resourceSlider.value = currentResource;
    }
}
