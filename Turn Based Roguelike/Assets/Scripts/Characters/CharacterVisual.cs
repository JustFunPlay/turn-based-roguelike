using System.Collections;
using System.Collections.Generic;
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
    }

    public void PlayAnimation(string animationName)
    {
        if (animator != null)
            animator.Play(animationName);
    }

    public void OnCrit()
    {
        if (MaxMana == 0)
        {
            SkillPoints++;
        }
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
    public void UseAbility(int abilityIndex)
    {
        characterData.abilities[abilityIndex].GetTarget(this);
    }
}
