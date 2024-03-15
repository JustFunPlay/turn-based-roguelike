using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    protected PlayerClass characterClass;
    protected int level;
    protected string characterName;

    protected int maxHP;
    protected int currentHP;
    protected float armor;
    protected float magicResist;
    protected float attack;
    protected float critChance;
    protected float magic;
    protected float maxMana;
    protected float currentMana;
    protected float manaRegen;
    protected float speed;

    private void Start()
    {
         if (characterClass != null) RecalculateStats();
    }
    public void LevelUp()
    {
        level++;
        RecalculateStats();
    }
    public void RecalculateStats()
    {
        maxHP = characterClass.baseHealth + (level - 1) * characterClass.healthPerLevel;
        armor = characterClass.baseArmor + (level - 1) * characterClass.armorPerLevel;
        magicResist = characterClass.baseMagicResist + (level - 1) * characterClass.magicResistPerLevel;
        attack = characterClass.baseAttack + (level -1 ) * characterClass.attackPerLevel;
        critChance = characterClass.baseCritChance;
        magic = characterClass.baseMagic + (level - 1) * characterClass.magicPerLevel;
        maxMana = characterClass.baseMana + (level - 1) * characterClass.manaPerLevel;
        manaRegen = characterClass.baseManaRegen + (level - 1) * characterClass.manaRegenPerLevel;
        speed = characterClass.baseSpeed;
    }
}
