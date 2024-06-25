using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public TMP_Text damageShower;
    public void ShowDamage(float value, bool isHealing)
    {
        transform.LookAt(Camera.main.transform.position, Vector3.up);
        GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-3f, 3f), 3, 0), ForceMode.VelocityChange);
        damageShower.color = isHealing ? Color.green : Color.red;
        damageShower.text = ((int)value).ToString();
        Destroy(gameObject, 0.75f);
    }
}
