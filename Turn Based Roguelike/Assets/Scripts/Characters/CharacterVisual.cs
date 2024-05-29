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
        this.characterData = characterData;
    }

    public void PlayAnimation(string animationName)
    {
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
        animator.Play("Hurt");
        switch (damageType)
        {
            case DamageType.Physical:
                damageToDo *= 1 - Armor / (100 + Armor);
                break;
            case DamageType.Magic:
                damageToDo *= 1 - MagicResist / (100 + MagicResist);
                break;
        }
        CurrentHP -= damageToDo;
        damageDealt = damageToDo;
    }
    public bool CheckForDeath()
    {
        if (CurrentHP <= 0)
        {
            //do the die
            return true;
        }
        return false;
    }
}
