using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class EnemyDamageIndicatorTest : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    [SerializeField] private float damageAmount;
    [SerializeField] private bool testHealing = false;
    [SerializeField] private DamagePopup popup;
    private Transform centerOfMass;

    public void SetMaxHealth(int maxHealth, Transform center)
    {
        centerOfMass = center;
        hpSlider.maxValue = maxHealth;
        hpSlider.value = maxHealth;
    }
    public void TakeDamage(float currentHealth, float value, bool isHealing = false)
    {
        hpSlider.value = currentHealth;
        if (popup == null) return;
        DamagePopup newpopup = Instantiate(popup, centerOfMass.position + (transform.right * Random.Range(-0.5f, 0.5f)) + (transform.up * Random.Range(-0.5f, 0.5f)), Quaternion.identity);
        newpopup.ShowDamage(value, isHealing);
    }
    private void FixedUpdate()
    {
        transform.LookAt(Camera.main.transform.position, Vector3.up);
    }
}
