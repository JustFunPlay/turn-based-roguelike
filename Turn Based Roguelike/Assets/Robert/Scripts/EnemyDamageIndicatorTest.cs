using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamageIndicatorTest : MonoBehaviour
{
    public Slider hpSlider;
    public GameObject damageShower;
    public float damageAmount;
    public void DoDamageTest()
    {
        TakeDamage(damageAmount);
    }
    public void TakeDamage(float damage)
    {
        hpSlider.value -= damage;
        StartCoroutine(DamageValueShower());
    }
    public IEnumerator DamageValueShower()
    {
        damageShower.SetActive(true);
        damageShower.GetComponent<Text>().text = "-" + damageAmount.ToString();
        yield return new WaitForSeconds(1.9f);
        damageShower.SetActive(false);
    }
}
