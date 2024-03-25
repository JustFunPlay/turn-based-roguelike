using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "New Player Class", menuName = "scriptableObjects/player/playerClass")]
public class PlayerClass : ScriptableObject
{
    public string className;
    public string[] names;

    [Header("Base stats")]
    public int baseHealth;
    public int baseArmor;
    public int baseMagicResist;
    public int baseAttack;
    public float baseCritChance;
    public int baseMagic;
    public int baseMana;
    public float baseManaRegen;
    public float baseSpeed;
    public float baseEvasion;

    [Header("Stats per level")]
    public int healthPerLevel;
    public float armorPerLevel;
    public float magicResistPerLevel;
    public float attackPerLevel;
    public float magicPerLevel;
    public int manaPerLevel;
    public float manaRegenPerLevel;

    [Header("Abilities")]
    public Ability[] skillLibrary;
    public Ability[] spellLibrary;
}
