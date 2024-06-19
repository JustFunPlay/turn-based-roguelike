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
        if(positionData.character != null)
            highlightTarget.transform.localPosition = new Vector3(0, positionData.character.centreOfMassOffset, 0);
        highlightTarget.SetActive(enable);
    }
}
