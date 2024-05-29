using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "scriptableObjects/abilities/ability")]
public class Ability : ScriptableObject
{
    public string abilityName;
    [Multiline] public string abilityDescription;
    public AbilityType abilityType;
    public int resourceCost;
    public Targeting targetSelection;
    public AbilityEffect[] effects;
}


public enum AbilityType
{
    Skill,
    Spell
}

[System.Serializable]
public class AbilityEffect
{
    public EffectTarget target;
    public ConditionalCheck conditional;
    public float damageModifier;
    public bool isHealing;
    public float critModifier;
    public float accuracy;
    public float lifeSteal;
    public float pierce;
    public EffectTrigger[] onHitEffects;
    public EffectTrigger[] onCritEffects;
    public EffectTrigger[] onKillEffects;
}
public enum EffectTarget
{
    SingleTarget = 0,
    RandomTarget = 1,
    GroupTarget = 2
}
[System.Serializable]
public class EffectTrigger
{
    public bool affectsSelf;
    public StatVar statVar;
    public float ammount;
    public int duration;
}
public enum StatVar
{
    MaxHealth,
    MagicHealing,
    AttackHealing,
    HealthHealing,
    Armor,
    MagicResist,
    Attack,
    Crit,
    Magic,
    ManaGain,
    ManaRegen,
    Speed,
    Evasion,
    Accuracy,
    SkillPoint,
    HealingDone,
    HealingRecieved,
    Reflect,
    Stun,
    Cleanse,
    MagicDOT,
    PhysicalDOT,
    HealthDOT,
    MagicHOT,
    AttackHOT,
    HealthHOT,
    LifeSteal,
    ArmorPierce,
    MagicPierce
}

[System.Serializable]
public class ConditionalCheck
{
    public ConditionalType type;
    public float value;
}
public enum ConditionalType
{
    None,
    TargetCurrentHealth,
    TargetMissingHealth,
    TargetIsStunned,
    TargetCurrentHealthModifier,
    TargetMissingHealthModifier,
    UserCurrentHealth,
    UserMissingHealth
}