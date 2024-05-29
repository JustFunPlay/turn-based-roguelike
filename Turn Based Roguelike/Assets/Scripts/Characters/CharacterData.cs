using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : ScriptableObject
{
    //public GameObject visualModel
    [Header("Base stats")]
    public int baseHealth;
    public int baseArmor;
    public int baseMagicResist;
    public int baseAttack;
    public float baseCritChance;
    public int baseMagic;
    public int baseResource;
    public float baseResourceRegen;
    public float baseSpeed;
    public float baseEvasion;

    [Header("Stats per level")]
    public int healthPerLevel;
    public float armorPerLevel;
    public float magicResistPerLevel;
    public float attackPerLevel;
    public float magicPerLevel;
    public int resourcePerLevel;
    public float resourceRegenPerLevel;

    [Header("Abilities")]
    public List<BaseAbility> abilities;
}
