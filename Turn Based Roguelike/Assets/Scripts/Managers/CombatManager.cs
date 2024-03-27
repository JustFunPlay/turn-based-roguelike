using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class CombatManager
{
    public static List<CharacterStats> playerParty = new List<CharacterStats>();
    public static List<CharacterStats> enemyWave = new List<CharacterStats>();

    private static List<CharacterStats> actedCharacters = new List<CharacterStats>();

    public static List<CharacterStats> GetOpposingParty(CharacterStats character)
    {
        if (playerParty.Contains(character))
            return enemyWave;
        else if (enemyWave.Contains(character))
            return playerParty;
        else return null;
    }
    public static List<CharacterStats> GetOwnParty(CharacterStats character)
    {
        if (playerParty.Contains(character))
            return playerParty;
        else if (enemyWave.Contains(character))
            return enemyWave;
        else return null;
    }

    public static void StartCombat()
    {

    }

    public static void ReadyNextCharacter()
    {
        CharacterStats characterToAct = null;
        if (actedCharacters.Count == playerParty.Count + enemyWave.Count)
            actedCharacters.Clear();
        for (int i = 0; i < playerParty.Count; i++)
        {
            if (!actedCharacters.Contains(playerParty[i]) && (characterToAct == null || playerParty[i].GetSpeed > characterToAct.GetSpeed))
                characterToAct = playerParty[i];
        }
        for (int i = 0;i < enemyWave.Count; i++)
        {
            if (!actedCharacters.Contains(enemyWave[i]) && (characterToAct == null || enemyWave[i].GetSpeed > characterToAct.GetSpeed))
                characterToAct = enemyWave[i];
        }
        characterToAct.StartTurn();
        actedCharacters.Add(characterToAct);
    }
    
    public static void RemoveFromCombat(CharacterStats character)
    {
        playerParty.Remove(character);
        enemyWave.Remove(character);
        actedCharacters.Remove(character);
    }
}
