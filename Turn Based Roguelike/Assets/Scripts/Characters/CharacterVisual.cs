using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterVisual : MonoBehaviour
{
    public CharacterData characterData;

    public float centreOfMassOffset;
    [SerializeField] private Animator animator;

    public int Level { get; private set; }
    public int Exp { get; private set; }
    public int MaxHP { get; private set; }
    public float CurrentHP { get; private set; }
    public float Armor { get; private set; }
    public float MagicResist { get; private set; }
    public float Attack { get; private set; }
    public float Magic { get; private set; }
    public float CritRate { get; private set; }
    public int SkillPoints { get; private set; }
    public int MaxMana { get; private set; }
    public float CurrentMana { get; private set; }
    public float ManaRegen { get; private set; }
    public float Speed { get; private set; }

    public void InitializeCharacter(CharacterData characterData)
    {
        Level = 1;
        this.characterData = characterData;
        CurrentHP = MaxHP = characterData.baseHealth + (Level - 1) * characterData.healthPerLevel;
        Armor = characterData.baseArmor + (Level - 1) * characterData.armorPerLevel;
        MagicResist = characterData.baseMagicResist + (Level - 1) * characterData.magicResistPerLevel;
        Attack = characterData.baseAttack + (Level -1) * characterData.attackPerLevel;
        CritRate = characterData.baseCritChance;
        if (MaxMana == 0)
            SkillPoints = 5;
        Speed = characterData.baseSpeed;
    }

    public void StartTurn()
    {
        Debug.Log($"{gameObject.name} starts their turn");
        if (CombatManager.instance.enemyTeam.Contains(CombatManager.instance.GetOwnCombatPosition(this)))
        {
            UseRandomSkill();
        }
        else
        {

        }
    }

    protected virtual void UseRandomSkill()
    {
        if (characterData.abilities.Count > 0)
        {
            for (int i = 1; i < characterData.abilities.Count; i++)
            {
                if (CurrentMana >= characterData.abilities[i].resourceCost || SkillPoints > characterData.abilities[i].resourceCost)
                {
                    if (Random.Range(0, MaxMana > 0 ? MaxMana : 10) <= characterData.abilities[i].resourceCost)
                    {
                        characterData.abilities[i].GetTarget(this);

                        if (MaxMana > 0)
                            CurrentMana -= characterData.abilities[i].resourceCost;
                        else
                            SkillPoints -= characterData.abilities[i].resourceCost;

                        return;
                    }
                }
            }
            characterData.abilities[0].GetTarget(this);

        }
        else
        {
            CombatManager.instance.PerformEndOfActionChecks();
        }

    }

    public void AddSkillPoints(int ammount = 1)
    {
        if (MaxMana == 0)
            SkillPoints = Mathf.Clamp(SkillPoints + ammount, 0, 10);
    }

    public void PlayAnimation(string animationName)
    {
        if (animator != null)
            animator.Play(animationName);
    }

    public void OnCrit()
    {
            AddSkillPoints();
    }

    public void TakeDamage(float damageToDo, DamageType damageType, out float damageDealt)
    {
        PlayAnimation("Hurt");
        switch (damageType)
        {
            case DamageType.Physical:
                damageToDo *= 100 / (100 + Armor);
                break;
            case DamageType.Magic:
                damageToDo *= 100 / (100 + MagicResist);
                break;
        }
        CurrentHP -= damageToDo;
        damageDealt = damageToDo;
        Debug.Log($"{gameObject.name} took {damageDealt} {damageType}damage");
    }
    public bool CheckForDeath()
    {
        if (CurrentHP <= 0)
        {
            //do the die
            Destroy(gameObject, 0.1f);
            return true;
        }
        return false;
    }
    public void RestoreHealth(float healingToDo, out float healingDone)
    {
        healingDone = Mathf.Min(healingToDo, MaxHP - CurrentHP);
        CurrentHP = Mathf.Clamp(CurrentHP + healingToDo, 0, MaxHP);
    }
    public void UseAbility(int abilityIndex)
    {
        characterData.abilities[abilityIndex].GetTarget(this);
    }
}
