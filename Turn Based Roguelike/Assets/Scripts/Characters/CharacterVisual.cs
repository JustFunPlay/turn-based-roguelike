using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Cinemachine.AxisState;

public class CharacterVisual : MonoBehaviour
{
    public CharacterData characterData;

    public float centreOfMassOffset;
    [SerializeField] private Animator animator;
    public Transform[] abilityPoints;

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

    private List<StatusEffect> effects = new List<StatusEffect>();

    public void InitializeCharacter(CharacterData characterData)
    {
        Level = 1;
        this.characterData = characterData;
        CurrentHP = MaxHP = characterData.baseHealth + (Level - 1) * characterData.healthPerLevel;
        Armor = characterData.baseArmor + (Level - 1) * characterData.armorPerLevel;
        MagicResist = characterData.baseMagicResist + (Level - 1) * characterData.magicResistPerLevel;
        Attack = characterData.baseAttack + (Level -1) * characterData.attackPerLevel;
        Magic = characterData.baseMagic + (Level - 1) * characterData.magicPerLevel;
        MaxMana = characterData.baseResource + (Level - 1) * characterData.resourcePerLevel;
        CurrentMana = MaxMana;
        ManaRegen = characterData.baseResourceRegen + (Level - 1) * characterData.resourceRegenPerLevel;
        CritRate = characterData.baseCritChance;
        if (MaxMana == 0)
            SkillPoints = 5;
        Speed = characterData.baseSpeed;
        StartCoroutine(DoAltIdle());
    }

    private IEnumerator DoAltIdle()
    {
        while (CurrentHP > 0)
        {
            yield return new WaitForSeconds(Random.Range(5, 30));
            PlayAnimation("AltIdle");
        }
    }

    public void StartTurn()
    {
        Debug.Log($"{gameObject.name} starts their turn");

        for (int i = 0; i < effects.Count; i++)
        {
            if (effects[i] != null)
                effects[i].OnStartTurn();
            if (effects[i] == null)
            {
                effects.RemoveAt(i);
                i--;
            }
        }
        if (CombatManager.instance.enemyTeam.Contains(CombatManager.instance.GetOwnCombatPosition(this)))
        {
            UseRandomSkill();
        }
        else
        {

        }
    }

    public void EndTurn()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            if (effects[i] != null)
                effects[i].OnEndTurn();
            if (effects[i] == null)
            {
                effects.RemoveAt(i);
                i--;
            }
        }
        CombatManager.instance.PerformEndOfActionChecks();
    }

    public void AddStatusEffect(StatusEffect newEffect)
    {
        effects.Add(newEffect);
    }

    public void IncreaseStat(float modifier, StatVar buff)
    {
        modifier = Mathf.Abs(modifier);
        switch (buff)
        {
            case StatVar.MaxHealth:
                MaxHP += (int)(modifier * (characterData.baseHealth + (Level - 1) * characterData.healthPerLevel));
                break;
            case StatVar.Armor:
                Armor += modifier * (characterData.baseArmor + (Level - 1) * characterData.armorPerLevel);
                break;
            case StatVar.MagicResist:
                MagicResist += modifier * (characterData.baseMagicResist + (Level - 1) * characterData.magicResistPerLevel);
                break;
            case StatVar.Attack:
                Attack += modifier * (characterData.baseAttack + (Level - 1) * characterData.attackPerLevel);
                break;
            case StatVar.Crit:
                CritRate += modifier;
                break;
            case StatVar.Magic:
                Magic += modifier * (characterData.baseMagic + (Level - 1) * characterData.magicPerLevel);
                break;
            case StatVar.ManaRegen:
                ManaRegen += modifier * (characterData.baseResourceRegen + (Level - 1) * characterData.resourceRegenPerLevel);
                break;
            case StatVar.Speed:
                Speed += modifier * characterData.baseSpeed;
                break;
        }
    }
    public void DecreaseStat(float modifier, StatVar buff)
    {
        modifier = Mathf.Abs(modifier);
        switch (buff)
        {
            case StatVar.MaxHealth:
                MaxHP -= (int)(modifier * (characterData.baseHealth + (Level - 1) * characterData.healthPerLevel));
                break;
            case StatVar.Armor:
                Armor -= modifier * (characterData.baseArmor + (Level - 1) * characterData.armorPerLevel);
                break;
            case StatVar.MagicResist:
                MagicResist -= modifier * (characterData.baseMagicResist + (Level - 1) * characterData.magicResistPerLevel);
                break;
            case StatVar.Attack:
                Attack -= modifier * (characterData.baseAttack + (Level - 1) * characterData.attackPerLevel);
                break;
            case StatVar.Crit:
                CritRate -= modifier;
                break;
            case StatVar.Magic:
                Magic -= modifier * (characterData.baseMagic + (Level - 1) * characterData.magicPerLevel);
                break;
            case StatVar.ManaRegen:
                ManaRegen -= modifier * (characterData.baseResourceRegen + (Level - 1) * characterData.resourceRegenPerLevel);
                break;
            case StatVar.Speed:
                Speed -= modifier * characterData.baseSpeed;
                break;
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
            animator.SetTrigger(animationName);
    }

    public void OnCrit()
    {
            AddSkillPoints();
    }
    public void AddResource(int value)
    {
        if (MaxMana > 0)
            CurrentMana = Mathf.Min(CurrentMana + value, MaxMana);
        else
            AddSkillPoints(value);
    }
    public bool TakeDamage(float damageToDo, DamageType damageType, out float damageDealt, float resistIgnore = 0)
    {
        PlayAnimation("TakeDamage");
        switch (damageType)
        {
            case DamageType.Physical:
                damageToDo *= 100 / (100 + Armor * Mathf.Clamp01(1 - resistIgnore));
                break;
            case DamageType.Magic:
                damageToDo *= 100 / (100 + MagicResist * Mathf.Clamp01(1 - resistIgnore));
                break;
        }
        CurrentHP -= damageToDo;
        damageDealt = damageToDo;
        Debug.Log($"{gameObject.name} took {damageDealt} {damageType} damage");
        if (CurrentHP <= 0)
            return true;
        return false;
    }
    public bool CheckForDeath()
    {
        if (CurrentHP <= 0)
        {
            PlayAnimation("Death");
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
        if (characterData.abilities.Count > abilityIndex)
            characterData.abilities[abilityIndex].GetTarget(this);
    }
    public void ToggleVisuals(bool enable)
    {
        foreach(SkinnedMeshRenderer renderer in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            renderer.enabled = enable;
        }
    }
}
