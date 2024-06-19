using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] private Transform cameraTransform;

    [SerializeField] private Transform[] playerFocusPositions;
    [SerializeField] private Transform[] playerFrontViewPositions;
    [SerializeField] private Transform[] enemyFocusPositions;
    [SerializeField] private Transform playerTeamFocusPosition;
    [SerializeField] private Transform enemyTeamFocusPosition;
    [SerializeField] private GameObject inverseFilter;

    private Transform currentFocusPoint;

    private void Start()
    {
        Instance = this;
        currentFocusPoint = playerTeamFocusPosition;
        cameraTransform.position = currentFocusPoint.position;
        cameraTransform.rotation = currentFocusPoint.rotation;
        inverseFilter.SetActive(false);
    }

    public void SetFocusPosition(CombatPositionData combatPositionData)
    {
        if (CombatManager.instance.playerParty.Contains(combatPositionData))
        {
            for (int i = 0; i < playerFocusPositions.Length; i++)
            {
                if (combatPositionData == CombatManager.instance.playerParty[i])
                {
                    StartCoroutine(MoveToNewFocusPoint(playerFocusPositions[i]));
                    CombatManager.instance.playerParty[i].character.ToggleVisuals(true);
                }
                else if (CombatManager.instance.playerParty[i].character != null)
                {
                    CombatManager.instance.playerParty[i].character.ToggleVisuals(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < enemyFocusPositions.Length; i++)
            {
                if (combatPositionData == CombatManager.instance.enemyTeam[i])
                {
                    StartCoroutine(MoveToNewFocusPoint(enemyFocusPositions[i]));
                }
            }
        }
    }
    public void SetTeamView(CombatPositionData combatPositionData)
    {
        if (CombatManager.instance.playerParty.Contains(combatPositionData))
        {
            StartCoroutine(MoveToNewFocusPoint(playerTeamFocusPosition));
            foreach(CombatPositionData position in CombatManager.instance.playerParty)
                position.character.ToggleVisuals(true);
        }
        else
            StartCoroutine(MoveToNewFocusPoint(enemyTeamFocusPosition));
    }

    public void SetTargetPosition(CombatPositionData combatPositionData)
    {
        if (CombatManager.instance.playerParty.Contains(combatPositionData))
        {
            for (int i = 0; i < playerFrontViewPositions.Length; i++)
            {
                if (combatPositionData == CombatManager.instance.playerParty[i])
                {
                    StartCoroutine(MoveToNewFocusPoint(playerFrontViewPositions[i]));
                    CombatManager.instance.playerParty[i].character.ToggleVisuals(true);
                }
                else if(CombatManager.instance.playerParty[i].character != null)
                {
                    CombatManager.instance.playerParty[i].character.ToggleVisuals(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < enemyFocusPositions.Length; i++)
            {
                if (combatPositionData == CombatManager.instance.enemyTeam[i])
                {
                    StartCoroutine(MoveToNewFocusPoint(enemyFocusPositions[i]));
                }
            }
        }
    }

    private IEnumerator MoveToNewFocusPoint(Transform newPoint)
    {
        for (int t = 0; t <= 10; t++)
        {
            cameraTransform.position = Vector3.Lerp(currentFocusPoint.position, newPoint.position, t * 0.1f);
            cameraTransform.rotation = Quaternion.Slerp(currentFocusPoint.rotation, newPoint.rotation, t * 0.1f);
            yield return new WaitForSeconds(0.025f);
        }
        currentFocusPoint = newPoint;
    }

    public void ToggleInverseFilter(bool active)
    {
        inverseFilter.SetActive(active);
    }
}
