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
    [SerializeField] private CombatPositionData targetingSource;
    private Action<CombatPositionData, CombatPositionData[]> startAbilityAction;
    private List<CombatPositionData> currentSelectedTargets = new List<CombatPositionData>();

    [Header("Target selection framework")]
    [SerializeField] private InputAction clickOnTarget;
    //private void ClickOnTarget(InputAction.CallbackContext callbackContext) 
    [SerializeField] private InputAction confirmTarget;
    [SerializeField] private InputAction moveTargetLeft;
    [SerializeField] private InputAction moveTargetRight;

    public CharacterVisual currentlyActingCharacter { get; private set; }
    private void Start()
    {
        instance = this;

        for (int i = 0; i < playerParty.Length; i++)
        {
            playerParty[i].targetingBox.positionData = playerParty[i];
            playerParty[i].targetingBox.ToggleHighlight(false);

            if (playerParty[i].character != null)
            {
                playerParty[i].character.InitializeCharacter(playerParty[i].character.characterData);
                playerParty[i].Turnprogress = 10000 / playerParty[i].character.Speed;
            }
        }
        for (int i = 0; i < enemyTeam.Length; i++)
        {
            enemyTeam[i].targetingBox.positionData = enemyTeam[i];
            enemyTeam[i].targetingBox.ToggleHighlight(false);
            
            if (enemyTeam[i].character != null)
            {
                enemyTeam[i].character.InitializeCharacter(enemyTeam[i].character.characterData);
                enemyTeam[i].Turnprogress = 10000 / enemyTeam[i].character.Speed;
            }
        }
        Invoke("ReadyNextCharacter", 0.1f);
    }

    private void MoveTargetLeft(InputAction.CallbackContext callbackContext)
    {
        for (int i = 1; i < playerParty.Length; i++)
        {
            if (playerParty[i] == currentSelectedTargets[0])
            {
                for (int r = i - 1; r >= 0; r--)
                {
                    if (playerParty[r].character != null)
                    {
                        currentSelectedTargets[0].targetingBox.ToggleHighlight(false);
                        currentSelectedTargets.Clear();
                        currentSelectedTargets.Add(playerParty[r]);
                        currentSelectedTargets[0].targetingBox.ToggleHighlight(true);
                        return;
                    }
                }
                return;
            }
        }
        for (int i = 1; i < enemyTeam.Length; i++)
        {
            if (enemyTeam[i] == currentSelectedTargets[0])
            {
                for (int r = i - 1; r >= 0; r--)
                {
                    if (enemyTeam[r].character != null)
                    {
                        currentSelectedTargets[0].targetingBox.ToggleHighlight(false);
                        currentSelectedTargets.Clear();
                        currentSelectedTargets.Add(enemyTeam[r]);
                        currentSelectedTargets[0].targetingBox.ToggleHighlight(true);
                        return;
                    }
                }
                return;
            }
        }
    }

    private void MoveTargetRight(InputAction.CallbackContext callbackContext)
    {
        for (int i = 0; i < playerParty.Length - 1; i++)
        {
            if (playerParty[i] == currentSelectedTargets[0])
            {
                for (int r = i + 1; r < playerParty.Length; r++)
                {
                    if (playerParty[r].character != null)
                    {
                        currentSelectedTargets[0].targetingBox.ToggleHighlight(false);
                        currentSelectedTargets.Clear();
                        currentSelectedTargets.Add(playerParty[r]);
                        currentSelectedTargets[0].targetingBox.ToggleHighlight(true);
                        return;
                    }
                }
                return;
            }
        }
        for (int i = 0; i < enemyTeam.Length - 1; i++)
        {
            if (enemyTeam[i] == currentSelectedTargets[0])
            {
                for (int r = i + 1; r < enemyTeam.Length; r++)
                {
                    if (enemyTeam[r].character != null)
                    {
                        currentSelectedTargets[0].targetingBox.ToggleHighlight(false);
                        currentSelectedTargets.Clear();
                        currentSelectedTargets.Add(enemyTeam[r]);
                        currentSelectedTargets[0].targetingBox.ToggleHighlight(true);
                        return;
                    }
                }
                return;
            }
        }
    }

    public void PrepareTargetSelection(CharacterVisual source, Targeting targetingType, Action<CombatPositionData, CombatPositionData[]> startAbilityAction)
    {
        CancelTargetSelection();

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
                if (isPlayer) CameraManager.Instance.SetTargetPosition(targetingSource);
                break;
            case Targeting.Ally:
                currentSelectedTargets.Add(GetRandomTarget(targetingSource, targetingType));
                if (isPlayer) CameraManager.Instance.SetTeamView(targetingSource);
                break;
            case Targeting.AllyRandom:
                if (isPlayer) CameraManager.Instance.SetTeamView(targetingSource);
                SelectEntireParty(isPlayer);
                break;
            case Targeting.AllyTeam:
                if (isPlayer) CameraManager.Instance.SetTeamView(targetingSource);
                SelectEntireParty(isPlayer);
                break;
        }

        for (int i = 0; i < currentSelectedTargets.Count; i++)
        {
            currentSelectedTargets[i].targetingBox.ToggleHighlight(true);
        }

        if (isPlayer)
        {
            Debug.Log("Ability used by player");
            confirmTarget.started += ConfirmTarget;
            confirmTarget.Enable();
            if (currentTargetingType == Targeting.Enemy || currentTargetingType == Targeting.Ally)
            {
                moveTargetLeft.started += MoveTargetLeft;
                moveTargetLeft.Enable();
                moveTargetRight.started += MoveTargetRight;
                moveTargetRight.Enable();
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
        for (int i = 0; i < playerParty.Length; i++)
        {
            playerParty[i].targetingBox.ToggleHighlight(false);
        }
        for (int i = 0; i < enemyTeam.Length; i++)
        {
            enemyTeam[i].targetingBox.ToggleHighlight(false);
        }

        //targetingSource = null;
        currentTargetingType = Targeting.Self;
        startAbilityAction = null;
        currentSelectedTargets.Clear();
        confirmTarget.started -= ConfirmTarget;
        moveTargetLeft.started -= MoveTargetLeft;
        moveTargetRight.started -= MoveTargetRight;
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


    private void ReadyNextCharacter()
    {
        bool progressedTurn = false;
        for (int i = 0; i < playerParty.Length; i++)
        {
            if (playerParty[i].character != null && playerParty[i].Turnprogress <= 0.01f)
            {
                playerParty[i].Turnprogress += 10000 / playerParty[i].character.Speed;
                progressedTurn = true;
                break;
            }
        }
        if (!progressedTurn)
        {
            for (int i = 0; i < enemyTeam.Length; i++)
            {
                if (enemyTeam[i].character != null && enemyTeam[i].Turnprogress <= 0.01f)
                {
                    enemyTeam[i].Turnprogress += 10000 / enemyTeam[i].character.Speed;
                    break;
                }
            }
        }
        
        CombatPositionData nextToAct = new CombatPositionData();
        for (int i = 0; i < playerParty.Length; i++)
        {
            if (playerParty[i].character != null)
            {
                if (nextToAct.character == null || playerParty[i].Turnprogress < nextToAct.Turnprogress)
                    nextToAct = playerParty[i];
            }
        }
        Debug.Log(nextToAct);
        for (int i = 0; i < enemyTeam.Length; i++)
        {
            if (enemyTeam[i].character != null && enemyTeam[i].Turnprogress < nextToAct.Turnprogress)
                nextToAct = enemyTeam[i];
        }

        float timeToReduce = nextToAct.Turnprogress;
        for (int i = 0; i < playerParty.Length; i++)
            playerParty[i].Turnprogress -= timeToReduce;
        for (int i = 0; i < enemyTeam.Length; i++)
            enemyTeam[i].Turnprogress -= timeToReduce;
        CameraManager.Instance.SetFocusPosition(nextToAct);

        currentlyActingCharacter = nextToAct.character;
        nextToAct.character.StartTurn();
    }

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

        if (!CheckForCombatEnd())
            ReadyNextCharacter();
    }

    private bool CheckForCombatEnd()
    {
        bool playerIsAlive = false;
        bool enemyIsAlive = false;

        for (int i = 0; i < playerParty.Length; i++)
        {
            if (playerParty[i].character != null)
                playerIsAlive = true;
        }
        for (int i = 0; i < enemyTeam.Length; i++)
        {
            if (enemyTeam[i].character != null)
                enemyIsAlive = true;
        }
        if (playerIsAlive && enemyIsAlive)
            return false;

        Debug.Log("Combat End");
        return true;
    }
}
[Serializable]

public class CombatPositionData
{
    public CharacterVisual character;
    public Transform standingPosition;
    public float Turnprogress;
    public TargetingBox targetingBox;
}