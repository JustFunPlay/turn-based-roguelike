using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] protected PlayerClass characterClass;
    [SerializeField] protected int level;
    [SerializeField] protected string characterName;

    [SerializeField] protected int maxHP;
    [SerializeField] protected float currentHP;
    [SerializeField] protected float armor;
    [SerializeField] protected float magicResist;
    [SerializeField] protected float attack;
    [SerializeField] protected float critChance;
    [SerializeField] protected float magic;
    [SerializeField] protected float maxMana;
    [SerializeField] protected float currentMana;
    [SerializeField] protected float manaRegen;
    [SerializeField] protected float speed;
    [SerializeField] protected float evasion;
    [SerializeField] protected int skillPoints;

    [SerializeField] protected List<Ability> learnedSkills = new List<Ability>();
    [SerializeField] protected List<Ability> learnedSpells = new List<Ability>();

    protected List<StatBuff> buffs = new List<StatBuff>();
    protected List<StatBuff> debuffs = new List<StatBuff>();

    private void Start()
    {
         
    }
    public void SetUpCharacter(PlayerClass playerClass, int startingLevel)
    {
        learnedSkills.Clear();
        learnedSpells.Clear();
        characterClass = playerClass;
        level = startingLevel;
        RecalculateStats();
        currentHP = maxHP;
        currentMana = maxMana;
        learnedSkills.Add(characterClass.skillLibrary[Random.Range(0, characterClass.skillLibrary.Length)]);
        learnedSpells.Add(characterClass.spellLibrary[Random.Range(0, characterClass.spellLibrary.Length)]);
        for (int i = 0; i < level; i++)
        {
            AddNewAbility();
        }
    }
    public void LevelUp()
    {
        level++;
        RecalculateStats();
        AddNewAbility();
        currentHP += characterClass.healthPerLevel;
        currentMana += characterClass.manaPerLevel;
    }
    public void RecalculateStats()
    {
        maxHP = characterClass.baseHealth + (level - 1) * characterClass.healthPerLevel;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        armor = characterClass.baseArmor + (level - 1) * characterClass.armorPerLevel;
        magicResist = characterClass.baseMagicResist + (level - 1) * characterClass.magicResistPerLevel;
        attack = characterClass.baseAttack + (level -1 ) * characterClass.attackPerLevel;
        critChance = characterClass.baseCritChance;
        magic = characterClass.baseMagic + (level - 1) * characterClass.magicPerLevel;
        maxMana = characterClass.baseMana + (level - 1) * characterClass.manaPerLevel;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        manaRegen = characterClass.baseManaRegen + (level - 1) * characterClass.manaRegenPerLevel;
        speed = characterClass.baseSpeed;
        evasion = characterClass.baseEvasion;
    }
    protected void AddNewAbility()
    {
        if (learnedSkills.Count == characterClass.skillLibrary.Length && learnedSpells.Count == characterClass.spellLibrary.Length)
            return;
        Ability newAbility = null;
        while (newAbility == null)
        {
            int i;
            if (learnedSkills.Count == characterClass.skillLibrary.Length)
            {
                i = Random.Range(0, characterClass.spellLibrary.Length);
                newAbility = characterClass.spellLibrary[i];
            }
            else if (learnedSpells.Count == characterClass.spellLibrary.Length)
            {
                i = Random.Range(0, characterClass.skillLibrary.Length);
                newAbility = characterClass.skillLibrary[i];
            }
            else
            {
                i = Random.Range(0, characterClass.skillLibrary.Length + characterClass.spellLibrary.Length);
                if (i < characterClass.skillLibrary.Length)
                    newAbility = characterClass.skillLibrary[i];
                else
                    newAbility = characterClass.spellLibrary[i - characterClass.skillLibrary.Length];
            }
            if (newAbility.abilityType == AbilityType.Skill && !learnedSkills.Contains(newAbility))
                learnedSkills.Add(newAbility);
            else if (newAbility.abilityType == AbilityType.Spell && !learnedSpells.Contains(newAbility))
                learnedSpells.Add(newAbility);
            else
                newAbility = null;
        }
    }
    public void TakeDamage(float damageToDo, DamageType damageType, float pierce, out bool isKilled)
    {
        if (damageType == DamageType.Physical)
        {
            damageToDo *= 100 / (100 + (armor * Mathf.Clamp01(1 - pierce)));
        }
        else if (damageType == DamageType.Magic)
        {
            damageToDo *= 100 / (100 + (magicResist * Mathf.Clamp01(1 - pierce)));
        }
        currentHP -= damageToDo;
        isKilled = false;
        if (currentHP <= 0)
        {
            OnDeath();
            isKilled = true;
        }
    }
    protected virtual void OnDeath()
    {

    }

    public void ApplyBuff(StatVar stat, float value, int duration)
    {
        switch (stat)
        {
            case StatVar.SkillPoint:
                break;
            case StatVar.Stun:
                break;
            case StatVar.DOT:
                break;
            case StatVar.HOT:
                break;
            case StatVar.Cleanse:
                break;
            case StatVar.ManaGain:
                break;
        }
    }
}

public enum DamageType
{
    Physical,
    Magic,
    True
}

[System.Serializable]
public class StatBuff
{
    public StatVar stat;
    public float value;
    public int remainingDuration;
    public StatBuff(StatVar stat, float value, int remainingDuration)
    {
        this.stat = stat;
        this.value = value;
        this.remainingDuration = remainingDuration;
    }
}