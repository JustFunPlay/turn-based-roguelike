using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public string abilityName;
    [Multiline] public string abilityDescription;
    public AbilityType abilityType;
    public int resourceCost;
    public Targeting targetSelection;
}


public enum AbilityType
{
    Skill,
    Spell
}
public enum Targeting
{
    Self,
    Ally,
    AllyTeam,
    Enemy,
    EnemyTeam
}