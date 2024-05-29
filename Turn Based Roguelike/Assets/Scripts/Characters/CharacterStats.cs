using System.Collections;
using System.Collections.Generic;
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

    public float GetCurrentHealthPercentage { get { return currentHP / maxHP; } }
    public bool IsStunned { get; protected set; }
    public string GetName { get { return characterName; } }
    public float GetSpeed {  get { return speed; } }

    [SerializeField] protected List<Ability> learnedSkills = new List<Ability>();
    [SerializeField] protected List<Ability> learnedSpells = new List<Ability>();
    [SerializeField] protected List<EffectTrigger> onCritEffects = new List<EffectTrigger>();
    public int LearnedSkills { get { return learnedSkills.Count; } }
    public int LearnedSpells { get { return learnedSpells.Count; } }

    [Header("stat buffs/debuffs")]
    [SerializeField]protected List<StatBuff> buffs = new List<StatBuff>();
    protected List<StatBuff> debuffs = new List<StatBuff>();

    protected float healingDoneMod;
    protected float healingRecievedMod;
    protected float reflectMod;
    protected float lifeSteal;
    protected float armorPierce;
    protected float magicPierce;
    protected float accuracy;

    protected List<StatBuff> effectsOverTime = new List<StatBuff>();

    private void Start()
    {
         
    }
    public void SetUpCharacter(PlayerClass playerClass, int startingLevel)
    {
        learnedSkills.Clear();
        learnedSpells.Clear();
        onCritEffects.Clear();
        characterClass = playerClass;
        level = startingLevel;
        characterName = characterClass.names[Random.Range(0, characterClass.names.Length)];
        foreach (EffectTrigger effect in playerClass.onCritEffect)
        {
            onCritEffects.Add(effect);
        }
        RecalculateStats();
        currentHP = maxHP;
        currentMana = maxMana;
        if (characterClass.skillLibrary.Length > 0)
            learnedSkills.Add(characterClass.skillLibrary[Random.Range(0, characterClass.skillLibrary.Length)]);
        if (characterClass.spellLibrary.Length > 0)
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
        healingDoneMod = healingRecievedMod = 1;
        reflectMod = accuracy = 0;
        critChance = characterClass.baseCritChance;
        speed = characterClass.baseSpeed;
        evasion = characterClass.baseEvasion;
        float hpMod = 1;
        float armorMod = 1;
        float mrMod = 1;
        float attackMod = 1;
        float magicMod = 1;
        float manaMod = 1;
        float manaRegenMod = 1;
        float speedMod = 1;
        foreach (StatBuff buff in buffs)
        {
            switch (buff.stat)
            {
                case StatVar.MaxHealth:
                    hpMod += buff.value;
                    break;
                case StatVar.Armor:
                    armorMod += buff.value;
                    break;
                case StatVar.MagicResist:
                    mrMod += buff.value;
                    break;
                case StatVar.Attack:
                    attackMod += buff.value;
                    break;
                case StatVar.Crit:
                    critChance += buff.value;
                    break;
                case StatVar.Magic:
                    magicMod += buff.value;
                    break;
                case StatVar.ManaRegen:
                    manaRegenMod += buff.value;
                    break;
                case StatVar.Speed:
                    speedMod += buff.value;
                    break;
                case StatVar.Evasion:
                    evasion += buff.value;
                    break;
                case StatVar.Accuracy:
                    accuracy += buff.value;
                    break;
                case StatVar.HealingDone:
                    healingDoneMod += buff.value;
                    break;
                case StatVar.HealingRecieved:
                    healingRecievedMod += buff.value;
                    break;
                case StatVar.Reflect:
                    reflectMod += buff.value;
                    break;
                case StatVar.LifeSteal:
                    lifeSteal += buff.value;
                    break;
                case StatVar.ArmorPierce:
                    armorPierce += buff.value;
                    break;
                case StatVar.MagicPierce:
                    magicPierce += buff.value;
                    break;
            }
        }
        foreach (StatBuff debuff in debuffs)
        {
            switch (debuff.stat)
            {
                case StatVar.MaxHealth:
                    hpMod -= debuff.value;
                    break;
                case StatVar.Armor:
                    armorMod -= debuff.value;
                    break;
                case StatVar.MagicResist:
                    mrMod -= debuff.value;
                    break;
                case StatVar.Attack:
                    attackMod -= debuff.value;
                    break;
                case StatVar.Crit:
                    critChance -= debuff.value;
                    break;
                case StatVar.Magic:
                    magicMod -= debuff.value;
                    break;
                case StatVar.ManaRegen:
                    manaRegenMod -= debuff.value;
                    break;
                case StatVar.Speed:
                    speedMod -= debuff.value;
                    break;
                case StatVar.Evasion:
                    evasion -= debuff.value;
                    break;
                case StatVar.Accuracy:
                    accuracy -= debuff.value;
                    break;
                case StatVar.HealingDone:
                    healingDoneMod -= debuff.value;
                    break;
                case StatVar.HealingRecieved:
                    healingRecievedMod -= debuff.value;
                    break;
                case StatVar.Reflect:
                    reflectMod -= debuff.value;
                    break;
                case StatVar.LifeSteal:
                    lifeSteal -= debuff.value;
                    break;
                case StatVar.ArmorPierce:
                    armorPierce -= debuff.value;
                    break;
                case StatVar.MagicPierce:
                    magicPierce -= debuff.value;
                    break;
            }
        }
        maxHP = (int)((characterClass.baseHealth + (level - 1) * characterClass.healthPerLevel) * hpMod);
        currentHP = Mathf.Min(currentHP, maxHP);
        armor = (characterClass.baseArmor + (level - 1) * characterClass.armorPerLevel) * armorMod;
        magicResist = (characterClass.baseMagicResist + (level - 1) * characterClass.magicResistPerLevel) * mrMod;
        attack = (characterClass.baseAttack + (level -1 ) * characterClass.attackPerLevel) * attackMod;
        magic = (characterClass.baseMagic + (level - 1) * characterClass.magicPerLevel) * magicMod;
        maxMana = (characterClass.baseMana + (level - 1) * characterClass.manaPerLevel) * manaMod;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        manaRegen = (characterClass.baseManaRegen + (level - 1) * characterClass.manaRegenPerLevel) * manaRegenMod;
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
    public bool CheckToHit(float accuracy)
    {
        float totalhitChance = accuracy - evasion;
        return Random.Range(0, 1) <= totalhitChance;
    }
    public void TakeDamage(float damageToDo, DamageType damageType, CharacterStats attacker, float pierce, out float damageDealt, out bool isKilled)
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
        if (damageType != DamageType.True && reflectMod > 0 && attacker != null)
        {
            attacker.TakeDamage(damageToDo * reflectMod, DamageType.True, this, 0, out float damageReflected, out bool reflectKill);
        }
        damageDealt = damageToDo;
        Debug.Log($"{characterName} takes {damageToDo} from {attacker.GetName}");
        isKilled = currentHP <= 0;
        if (isKilled)
            OnDeath();
    }
    protected virtual void OnDeath()
    {
        //CombatManager.RemoveFromCombat(this);
        //play death animation
    }
    public void ReceiveHealing(float healingRecieved, out float healingDone)
    {
        healingDone = Mathf.Min(healingRecieved * healingRecievedMod, maxHP - currentHP);
        currentHP += healingDone;
        Debug.Log($"{characterName} is healed for {healingDone}");
    }

    public void ApplyBuff(StatVar stat, float value, int duration)
    {
        switch (stat)
        {
            case StatVar.SkillPoint:
                skillPoints = Mathf.Clamp(skillPoints + (int)value, 0, 10);
                break;
            case StatVar.Stun:
                IsStunned = false;
                break;
            case StatVar.MagicDOT:
                break;
            case StatVar.PhysicalDOT:
                break;
            case StatVar.HealthDOT:
                break;
            case StatVar.MagicHOT:
                break;
            case StatVar.AttackHOT:
                break;
            case StatVar.HealthHOT:
                break;
            case StatVar.Cleanse:
                debuffs.Clear();
                RecalculateStats();
                break;
            case StatVar.MagicHealing:
                ReceiveHealing(value, out _);
                break;
            case StatVar.AttackHealing:
                ReceiveHealing(value, out _);
                break;
            case StatVar.HealthHealing:
                ReceiveHealing(value * maxHP, out _);
                break;
            case StatVar.ManaGain:
                currentMana = Mathf.Clamp(currentMana + maxMana * value, 0, maxMana);
                break;
            default:
                buffs.Add(new StatBuff(stat, value, duration));
                RecalculateStats();
                break;
        }
    }
    public void AddDebuff(StatVar stat, float value, int duration)
    {
        switch (stat)
        {
            case StatVar.SkillPoint:
                skillPoints = Mathf.Clamp(skillPoints - (int)value, 0, 10);
                break;
            case StatVar.Stun:
                IsStunned = true;
                break;
            case StatVar.MagicDOT:
                break;
            case StatVar.PhysicalDOT:
                break;
            case StatVar.HealthDOT:
                break;
            case StatVar.MagicHOT:
                break;
            case StatVar.AttackHOT:
                break;
            case StatVar.HealthHOT:
                break;
            case StatVar.Cleanse:
                buffs.Clear();
                RecalculateStats();
                break;
            case StatVar.MagicHealing:
                //ReceiveHealing(value, out _);
                break;
            case StatVar.AttackHealing:
                //ReceiveHealing(value, out _);
                break;
            case StatVar.HealthHealing:
                //ReceiveHealing(value * maxHP, out _);
                break;
            case StatVar.ManaGain:
                currentMana = Mathf.Clamp(currentMana - maxMana * value, 0, maxMana);
                break;
            default:
                debuffs.Add(new StatBuff(stat, value, duration));
                RecalculateStats();
                break;
        }
    }


    public virtual void StartTurn()
    {
        //do DoT and HoT
        for (int i = 0; i < effectsOverTime.Count; i++)
        {
            if (effectsOverTime[i].stat == StatVar.AttackHOT || effectsOverTime[i].stat == StatVar.MagicHOT)
                ReceiveHealing(effectsOverTime[i].value, out _);
            else if (effectsOverTime[i].stat == StatVar.HealthHOT)
                ReceiveHealing(effectsOverTime[i].value * maxHP, out _);
            else if (effectsOverTime[i].stat == StatVar.PhysicalDOT)
                TakeDamage(effectsOverTime[i].value, DamageType.Physical, null, 0, out _, out _);
            else if (effectsOverTime[i].stat == StatVar.MagicDOT)
                TakeDamage(effectsOverTime[i].value, DamageType.Magic, null, 0, out _, out _);
            else if (effectsOverTime[i].stat == StatVar.HealthDOT)
                TakeDamage(effectsOverTime[i].value * maxHP, DamageType.True, null, 0, out _, out _);
            effectsOverTime[i].remainingDuration--;
            if (effectsOverTime[i].remainingDuration < 0)
            {
                effectsOverTime.RemoveAt(i);
                i--;
            }
        }
    }
    protected virtual void EndTurn()
    {
        for (int i = 0; i < buffs.Count; i++)
        {
            buffs[i].remainingDuration--;
            if (buffs[i].remainingDuration <= 0)
            {
                buffs.RemoveAt(i);
                i--;
            }
        }
        for (int i = 0; i < debuffs.Count; i++)
        {
            debuffs[i].remainingDuration--;
            if (debuffs[i].remainingDuration <= 0)
            {
                debuffs.RemoveAt(i);
                i--;
            }
        }
        RecalculateStats();
        //CombatManager.ReadyNextCharacter();
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