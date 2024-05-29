using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSetupTest : MonoBehaviour
{
    public CharacterAttacks character;
    public PlayerClass playerClass;
    public int levelToSet;

    public CharacterStats dummy;
    public PlayerClass dummyClass;

    private void Start()
    {
        dummy.SetUpCharacter(dummyClass, 100);
        //CombatManager.enemyWave.Add(dummy);
    }

    public void GoLevelUp()
    {
        character.LevelUp();
    }
    public void Initialize()
    {
        character.SetUpCharacter(playerClass, levelToSet);
        //if (!CombatManager.playerParty.Contains(character))
        //    CombatManager.playerParty.Add(character);
    }

    public void UseBasicAttack()
    {
        //character.UseBasicAttack(CombatManager.GetOpposingParty(character)[0]);
    }
    public void CastRandomSkill()
    {
        //StartCoroutine(character.ActivateSkill(Random.Range(0, character.LearnedSkills), CombatManager.GetOpposingParty(character)[0]));
    }
    public void CastRandomSpell()
    {
        //StartCoroutine(character.ActivateSpell(Random.Range(0, character.LearnedSpells), CombatManager.GetOpposingParty(character)[0]));

    }
}
