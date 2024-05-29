using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;

    public CombatPositionData[] playerParty = new CombatPositionData[4];
    public CombatPositionData[] enemyTeam = new CombatPositionData[5];

    //target selection stuff
    private Targeting currentTargetingType;
    private CombatPositionData targetingSource;
    private Action<CombatPositionData, CombatPositionData[]> startAbilityAction;
    private List<CombatPositionData> currentSelectedTargets = new List<CombatPositionData>();

    [Header("Target selection framework")]
    [SerializeField] private InputAction clickOnTarget;
    //private void ClickOnTarget(InputAction.CallbackContext callbackContext) 
    [SerializeField] private InputAction confirmTarget;
    [SerializeField] private InputAction moveTargetLeft;
    [SerializeField] private InputAction moveTargetRight;

    private void Start()
    {
        instance = this;

        for (int i = 1; i < playerParty.Length; i++)
        {
            playerParty[i].targetingBox.positionData = playerParty[i];
        }
        for (int i = 1; i < enemyTeam.Length; i++)
        {
            enemyTeam[i].targetingBox.positionData = enemyTeam[i];
        }
    }

    private void MoveTargetLeft(InputAction.CallbackContext callbackContext)
    {
        for (int i = 1; i < playerParty.Length; i++)
        {
            if (playerParty[i] == currentSelectedTargets[0])
            {
                currentSelectedTargets[0].targetingBox.ToggleHighlight(false);
                currentSelectedTargets.Clear();
                currentSelectedTargets.Add(playerParty[i - 1]);
                currentSelectedTargets[0].targetingBox.ToggleHighlight(true);
                break;
            }
        }
        for (int i = 1; i < enemyTeam.Length; i++)
        {
            if (enemyTeam[i] == currentSelectedTargets[0])
            {
                currentSelectedTargets[0].targetingBox.ToggleHighlight(false);
                currentSelectedTargets.Clear();
                currentSelectedTargets.Add(enemyTeam[i - 1]);
                currentSelectedTargets[0].targetingBox.ToggleHighlight(true);
                break;
            }
        }
    }

    private void MoveTargetRight(InputAction.CallbackContext callbackContext)
    {
        for (int i = 0; i < playerParty.Length - 1; i++)
        {
            if (playerParty[i] == currentSelectedTargets[0])
            {
                currentSelectedTargets[0].targetingBox.ToggleHighlight(false);
                currentSelectedTargets.Clear();
                currentSelectedTargets.Add(playerParty[i + 1]);
                currentSelectedTargets[0].targetingBox.ToggleHighlight(true);
                break;
            }
        }
        for (int i = 0; i < enemyTeam.Length - 1; i++)
        {
            if (enemyTeam[i] == currentSelectedTargets[0])
            {
                currentSelectedTargets[0].targetingBox.ToggleHighlight(false);
                currentSelectedTargets.Clear();
                currentSelectedTargets.Add(enemyTeam[i + 1]);
                currentSelectedTargets[0].targetingBox.ToggleHighlight(true);
                break;
            }
        }
    }

    public void PrepareTargetSelection(CharacterVisual source, Targeting targetingType, Action<CombatPositionData, CombatPositionData[]> startAbilityAction)
    {
        targetingSource = GetOwnCombatPosition(source);
        currentTargetingType = targetingType;
        this.startAbilityAction = startAbilityAction;
        currentSelectedTargets.Clear();

        bool isPlayer = playerParty.Contains(targetingSource);

        switch (targetingType)
        {
            case Targeting.Enemy:
                currentSelectedTargets.Add(GetRandomTarget(targetingSource, targetingType));
                break;
            case Targeting.EnemyRandom:
                SelectEntireParty(!isPlayer);
                break;
            case Targeting.EnemyTeam:
                SelectEntireParty(!isPlayer);
                break;
            case Targeting.Self:
                currentSelectedTargets.Add(targetingSource);
                break;
            case Targeting.Ally:
                currentSelectedTargets.Add(GetRandomTarget(targetingSource, targetingType));
                break;
            case Targeting.AllyRandom:
                SelectEntireParty(isPlayer);
                break;
            case Targeting.AllyTeam:
                SelectEntireParty(isPlayer);
                break;
        }

        for (int i = 0; i < currentSelectedTargets.Count; i++)
        {
            currentSelectedTargets[i].targetingBox.ToggleHighlight(true);
        }

        if (isPlayer)
        {
            confirmTarget.performed += ConfirmTarget;
            if (currentTargetingType == Targeting.Enemy || currentTargetingType == Targeting.Ally)
            {
                moveTargetLeft.performed += MoveTargetLeft;
                moveTargetRight.performed += MoveTargetRight;
            }
        }
        else
        {
            ConfirmTargetSelection();
        }
    }

    private void SelectEntireParty(bool isPlayerParty)
    {
        if (isPlayerParty)
        {
            for (int i = 0; i < playerParty.Length; i++)
            {
                if (playerParty[i].character != null)
                    currentSelectedTargets.Add(playerParty[i]);
            }
        }
        else
        {
            for (int i = 0; i < enemyTeam.Length; i++)
            {
                if (enemyTeam[i].character != null)
                    currentSelectedTargets.Add(enemyTeam[i]);
            }
        }
    }

    public void CancelTargetSelection()
    {
        for (int i = 0; i < currentSelectedTargets.Count; i++)
        {
            currentSelectedTargets[i].targetingBox.ToggleHighlight(false);
        }

        targetingSource = null;
        currentTargetingType = Targeting.Self;
        startAbilityAction = null;
        currentSelectedTargets.Clear();
        confirmTarget.performed -= ConfirmTarget;
        moveTargetLeft.performed -= MoveTargetLeft;
        moveTargetRight.performed -= MoveTargetRight;
    }

    private void ConfirmTarget(InputAction.CallbackContext callbackContext) { ConfirmTargetSelection(); }

    public void ConfirmTargetSelection()
    {
        if (currentTargetingType == Targeting.AllyRandom || currentTargetingType == Targeting.EnemyRandom)
        {
            currentSelectedTargets.Clear();
            currentSelectedTargets.Add(GetRandomTarget(targetingSource, currentTargetingType));
        }
        startAbilityAction.Invoke(targetingSource, currentSelectedTargets.ToArray());

        CancelTargetSelection();
    }

    public CombatPositionData GetOwnCombatPosition(CharacterVisual source)
    {
        for (int i = 0; i < playerParty.Length; i++)
        {
            if (playerParty[i].character == source)
                return playerParty[i];
        }
        for (int i = 0; i < enemyTeam.Length;i++)
        {
            if (enemyTeam[i].character == source)
                return enemyTeam[i];
        }
        return null;
    }
    public CombatPositionData GetRandomTarget(CombatPositionData source, Targeting targetSide)
    {
        List<CombatPositionData> validTargets = new List<CombatPositionData>();

        bool targetAlly = true;
        if (targetSide == Targeting.EnemyRandom || targetSide == Targeting.Enemy || targetSide == Targeting.EnemyTeam)
            targetAlly = false;

        if ((playerParty.Contains(source) && targetAlly) || (enemyTeam.Contains(source) && !targetAlly))
        {
            for (int i = 0; i < playerParty.Length; i++)
            {
                if (playerParty[i].character != null)
                    validTargets.Add(playerParty[i]);
            }
        }
        else
        {
            for (int i = 0; i < enemyTeam.Length; i++)
            {
                if (enemyTeam[i].character != null)
                    validTargets.Add(enemyTeam[i]);
            }
        }

        if (validTargets.Count == 0)
            return null;
        return validTargets[UnityEngine.Random.Range(0, validTargets.Count)];
    }


    //public static void ReadyNextCharacter()
    //{
    //    CharacterStats characterToAct = null;
    //    if (actedCharacters.Count == playerParty.Count + enemyWave.Count)
    //        actedCharacters.Clear();
    //    for (int i = 0; i < playerParty.Count; i++)
    //    {
    //        if (!actedCharacters.Contains(playerParty[i]) && (characterToAct == null || playerParty[i].GetSpeed > characterToAct.GetSpeed))
    //            characterToAct = playerParty[i];
    //    }
    //    for (int i = 0;i < enemyWave.Count; i++)
    //    {
    //        if (!actedCharacters.Contains(enemyWave[i]) && (characterToAct == null || enemyWave[i].GetSpeed > characterToAct.GetSpeed))
    //            characterToAct = enemyWave[i];
    //    }
    //    characterToAct.StartTurn();
    //    actedCharacters.Add(characterToAct);
    //}

    public void PerformEndOfActionChecks()
    {
        for (int i = 0; i < playerParty.Length; i++)
        {
            if (playerParty[i].character != null && playerParty[i].character.CheckForDeath())
                playerParty[i].character = null;
        }
        for (int i = 0; i < enemyTeam.Length; i++)
        {
            if (enemyTeam[i].character != null && enemyTeam[i].character.CheckForDeath())
                enemyTeam[i].character = null;
        }
    }
}

public class CombatPositionData
{
    public CharacterVisual character;
    public Transform standingPosition;
    public float Turnprogress;
    public TargetingBox targetingBox;
}