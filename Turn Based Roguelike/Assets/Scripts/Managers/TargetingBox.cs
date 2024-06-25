using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingBox : MonoBehaviour
{
    [SerializeField] private GameObject highlightTarget;
    [SerializeField] private Collider selectionBox;
    public CombatPositionData positionData;
    [SerializeField] private EnemyDamageIndicatorTest hpBar;

    public void ToggleHighlight(bool enable)
    {
        highlightTarget.SetActive(enable);
    }
    public void OnInitialize(int maxHealth)
    {
        if (positionData.character == null) return;
        highlightTarget.transform.localPosition = new Vector3(0, positionData.character.centreOfMassOffset, 0);
        hpBar.gameObject.SetActive(true);
        hpBar.transform.localPosition = new Vector3(0, positionData.character.centreOfMassOffset * 2, 0);
        hpBar.SetMaxHealth(maxHealth, transform);
    }
    public void TakeDamage(float currentHealth, float value, bool isHealing = false)
    {
        if(hpBar != null)
        hpBar.TakeDamage(currentHealth, value, isHealing);
    }

}
