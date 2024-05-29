using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingBox : MonoBehaviour
{
    [SerializeField] private GameObject highlightTarget;
    [SerializeField] private Collider selectionBox;
    public CombatPositionData positionData;

    public void ToggleHighlight(bool enable)
    {
        highlightTarget.SetActive(enable);
    }
}
