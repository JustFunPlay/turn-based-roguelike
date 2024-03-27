using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttacks : CharacterStats
{
    public void UseBasicAttack(CharacterStats target)
    {
        if (target.CheckToHit(0.8f + accuracy))
        {
            bool crit = Random.Range(0f, 1f) <= critChance;
            float damageToDo = crit ? attack * 0.5f : attack;
            target.TakeDamage(damageToDo, DamageType.Physical, this, armorPierce, out float damageDealt, out _);
            if (lifeSteal > 0)
                ReceiveHealing(damageDealt * lifeSteal, out _);
            skillPoints = Mathf.Clamp(skillPoints++, 0, 10);
            if (crit)
            {
                for (int i = 0; i < onCritEffects.Count; i++)
                {
                    TriggerEffectOnTarget(target, onCritEffects[i]);
                }
            }
        }
        Invoke("EndTurn", 0.5f);
    }

    public IEnumerator ActivateSkill(int skillIndex, CharacterStats target)
    {
        Ability activatedSkill = learnedSkills[skillIndex];
        Debug.Log($"{characterName} uses the skill {activatedSkill.abilityName} at {target.GetName}");
        skillPoints -= activatedSkill.resourceCost;
        List<CharacterStats> targetParty;
        if (activatedSkill.targetSelection == Targeting.Enemy || activatedSkill.targetSelection == Targeting.EnemyTeam)
            targetParty = CombatManager.GetOpposingParty(this);
        else
            targetParty = CombatManager.GetOwnParty(this);
        foreach (AbilityEffect effect in activatedSkill.effects)
        {
            yield return new WaitForSeconds(0.25f);
            if ((effect.conditional.type == ConditionalType.UserMissingHealth && effect.conditional.value > 1 - GetCurrentHealthPercentage) || (effect.conditional.type == ConditionalType.UserCurrentHealth && effect.conditional.value > GetCurrentHealthPercentage))
                continue;
            float conditionalModdifier = 1;
            switch (effect.target)
            {
                case EffectTarget.SingleTarget:
                    if ((effect.conditional.type == ConditionalType.TargetMissingHealth && effect.conditional.value > 1 - target.GetCurrentHealthPercentage) || (effect.conditional.type == ConditionalType.TargetCurrentHealth && effect.conditional.value > target.GetCurrentHealthPercentage) || (effect.conditional.type == ConditionalType.TargetIsStunned && target.IsStunned))
                        continue;
                    conditionalModdifier = 1;
                    if (effect.conditional.type == ConditionalType.TargetMissingHealthModifier)
                    {
                        conditionalModdifier += effect.conditional.value * (1 - target.GetCurrentHealthPercentage);
                    }
                    else if (effect.conditional.type == ConditionalType.TargetCurrentHealthModifier)
                    {
                        conditionalModdifier += effect.conditional.value * target.GetCurrentHealthPercentage;
                    }
                    if (effect.isHealing)
                    {
                        bool crit = Random.Range(0f, 1f) <= critChance + effect.critModifier;
                        target.ReceiveHealing(effect.damageModifier * attack * healingDoneMod * (crit ? 2 : 1) * conditionalModdifier, out float healingDone);
                        for (int h = 0; h < effect.onHitEffects.Length; h++)
                        {
                            TriggerEffectOnTarget(target, effect.onHitEffects[h], true);
                        }
                        if (crit)
                        {
                            for (int c = 0; c < effect.onCritEffects.Length; c++)
                            {
                                TriggerEffectOnTarget(target, effect.onCritEffects[c], true);
                            }
                        }
                    }
                    else
                    {
                        if (target.CheckToHit(accuracy + effect.accuracy))
                        {
                            bool crit = Random.Range(0f, 1f) <= critChance + effect.critModifier;
                            target.TakeDamage(effect.damageModifier * attack * (crit ? 2 : 1) * conditionalModdifier, DamageType.Physical, this, armorPierce + effect.pierce, out float damageDealt, out bool isKilled);
                            if (lifeSteal + effect.lifeSteal > 0)
                                ReceiveHealing(damageDealt * lifeSteal + effect.lifeSteal, out _);
                            for (int h = 0; h < effect.onHitEffects.Length; h++)
                            {
                                TriggerEffectOnTarget(target, effect.onHitEffects[h]);
                            }
                            if (crit)
                            {
                                for (int c = 0; c < effect.onCritEffects.Length; c++)
                                {
                                    TriggerEffectOnTarget(target, effect.onCritEffects[c]);
                                }
                            }
                            if (isKilled)
                            {
                                for (int c = 0; c < effect.onKillEffects.Length; c++)
                                {
                                    TriggerEffectOnTarget(target, effect.onKillEffects[c]);
                                }
                            }
                        }
                    }
                    break;
                case EffectTarget.RandomTarget:
                    int rTarget = Random.Range(0, targetParty.Count);
                    if ((effect.conditional.type == ConditionalType.TargetMissingHealth && effect.conditional.value > 1 - targetParty[rTarget].GetCurrentHealthPercentage) || (effect.conditional.type == ConditionalType.TargetCurrentHealth && effect.conditional.value > targetParty[rTarget].GetCurrentHealthPercentage) || (effect.conditional.type == ConditionalType.TargetIsStunned && targetParty[rTarget].IsStunned))
                        continue;
                    conditionalModdifier = 1;
                    if (effect.conditional.type == ConditionalType.TargetMissingHealthModifier)
                    {
                        conditionalModdifier += effect.conditional.value * (1 - targetParty[rTarget].GetCurrentHealthPercentage);
                    }
                    else if (effect.conditional.type == ConditionalType.TargetCurrentHealthModifier)
                    {
                        conditionalModdifier += effect.conditional.value * targetParty[rTarget].GetCurrentHealthPercentage;
                    }
                    if (effect.isHealing)
                    {
                        bool crit = Random.Range(0f, 1f) <= critChance + effect.critModifier;
                        targetParty[rTarget].ReceiveHealing(effect.damageModifier * attack * healingDoneMod * (crit ? 2 : 1) * conditionalModdifier, out float healingDone);
                        for (int h = 0; h < effect.onHitEffects.Length; h++)
                        {
                            TriggerEffectOnTarget(targetParty[rTarget], effect.onHitEffects[h], true);
                        }
                        if (crit)
                        {
                            for (int c = 0; c < effect.onCritEffects.Length; c++)
                            {
                                TriggerEffectOnTarget(targetParty[rTarget], effect.onCritEffects[c], true);
                            }
                        }
                    }
                    else
                    {
                        if (targetParty[rTarget].CheckToHit(accuracy + effect.accuracy))
                        {
                            bool crit = Random.Range(0f, 1f) <= critChance + effect.critModifier;
                            targetParty[rTarget].TakeDamage(effect.damageModifier * attack * (crit ? 2 : 1) * conditionalModdifier, DamageType.Physical, this, armorPierce + effect.pierce, out float damageDealt, out bool isKilled);
                            if (lifeSteal + effect.lifeSteal > 0)
                                ReceiveHealing(damageDealt * lifeSteal + effect.lifeSteal, out _);
                            for (int h = 0; h < effect.onHitEffects.Length; h++)
                            {
                                TriggerEffectOnTarget(targetParty[rTarget], effect.onHitEffects[h]);
                            }
                            if (crit)
                            {
                                for (int c = 0; c < effect.onCritEffects.Length; c++)
                                {
                                    TriggerEffectOnTarget(targetParty[rTarget], effect.onCritEffects[c]);
                                }
                            }
                            if (isKilled)
                            {
                                for (int c = 0; c < effect.onKillEffects.Length; c++)
                                {
                                    TriggerEffectOnTarget(targetParty[rTarget], effect.onKillEffects[c]);
                                }
                            }
                        }
                    }
                    break;
                case EffectTarget.GroupTarget:

                    if (effect.isHealing)
                    {
                        for (int i = 0; i < targetParty.Count; i++)
                        {
                            if ((effect.conditional.type == ConditionalType.TargetMissingHealth && effect.conditional.value > 1 - targetParty[i].GetCurrentHealthPercentage) || (effect.conditional.type == ConditionalType.TargetCurrentHealth && effect.conditional.value > targetParty[i].GetCurrentHealthPercentage) || (effect.conditional.type == ConditionalType.TargetIsStunned && targetParty[i].IsStunned))
                                continue;
                            conditionalModdifier = 1;
                            if (effect.conditional.type == ConditionalType.TargetMissingHealthModifier)
                            {
                                conditionalModdifier += effect.conditional.value * (1 - targetParty[i].GetCurrentHealthPercentage);
                            }
                            else if (effect.conditional.type == ConditionalType.TargetCurrentHealthModifier)
                            {
                                conditionalModdifier += effect.conditional.value * targetParty[i].GetCurrentHealthPercentage;
                            }

                            bool crit = Random.Range(0f, 1f) <= critChance + effect.critModifier;
                            targetParty[i].ReceiveHealing(effect.damageModifier * attack * healingDoneMod * (crit ? 2 : 1) * conditionalModdifier, out float healingDone);
                            for (int h = 0; h < effect.onHitEffects.Length; h++)
                            {
                                TriggerEffectOnTarget(targetParty[i], effect.onHitEffects[h], true);
                            }
                            if (crit)
                            {
                                for (int c = 0; c < effect.onCritEffects.Length; c++)
                                {
                                    TriggerEffectOnTarget(targetParty[i], effect.onCritEffects[c], true);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < targetParty.Count; i++)
                        {
                            if ((effect.conditional.type == ConditionalType.TargetMissingHealth && effect.conditional.value > 1 - targetParty[i].GetCurrentHealthPercentage) || (effect.conditional.type == ConditionalType.TargetCurrentHealth && effect.conditional.value > targetParty[i].GetCurrentHealthPercentage) || (effect.conditional.type == ConditionalType.TargetIsStunned && targetParty[i].IsStunned))
                                continue;
                            conditionalModdifier = 1;
                            if (effect.conditional.type == ConditionalType.TargetMissingHealthModifier)
                            {
                                conditionalModdifier += effect.conditional.value * (1 - targetParty[i].GetCurrentHealthPercentage);
                            }
                            else if (effect.conditional.type == ConditionalType.TargetCurrentHealthModifier)
                            {
                                conditionalModdifier += effect.conditional.value * targetParty[i].GetCurrentHealthPercentage;
                            }

                            if (targetParty[i].CheckToHit(accuracy + effect.accuracy))
                            {
                                bool crit = Random.Range(0f, 1f) <= critChance + effect.critModifier;
                                targetParty[i].TakeDamage(effect.damageModifier * attack * (crit ? 2 : 1) * conditionalModdifier, DamageType.Physical, this, armorPierce + effect.pierce, out float damageDealt, out bool isKilled);
                                if (lifeSteal + effect.lifeSteal > 0)
                                    ReceiveHealing(damageDealt * lifeSteal + effect.lifeSteal, out _);
                                for (int h = 0; h < effect.onHitEffects.Length; h++)
                                {
                                    TriggerEffectOnTarget(targetParty[i], effect.onHitEffects[h]);
                                }
                                if (crit)
                                {
                                    for (int c = 0; c < effect.onCritEffects.Length; c++)
                                    {
                                        TriggerEffectOnTarget(targetParty[i], effect.onCritEffects[c]);
                                    }
                                }
                                if (isKilled)
                                {
                                    for (int c = 0; c < effect.onKillEffects.Length; c++)
                                    {
                                        TriggerEffectOnTarget(targetParty[i], effect.onKillEffects[c]);
                                    }
                                }
                            }
                        }
                    }
                    break;
            }
        }
        yield return new WaitForSeconds(0.5f);
        EndTurn();
    }
    public IEnumerator ActivateSpell(int spellIndex, CharacterStats target)
    {
        Ability activatedSpell = learnedSpells[spellIndex];
        Debug.Log($"{characterName} casts the spell {activatedSpell.abilityName} at {target.GetName}");
        currentMana -= activatedSpell.resourceCost;
        List<CharacterStats> targetParty;
        if (activatedSpell.targetSelection == Targeting.Enemy || activatedSpell.targetSelection == Targeting.EnemyTeam)
            targetParty = CombatManager.GetOpposingParty(this);
        else
            targetParty = CombatManager.GetOwnParty(this);
        foreach (AbilityEffect effect in activatedSpell.effects)
        {
            yield return new WaitForSeconds(0.25f);
            if ((effect.conditional.type == ConditionalType.UserMissingHealth && effect.conditional.value > 1 - GetCurrentHealthPercentage) || (effect.conditional.type == ConditionalType.UserCurrentHealth && effect.conditional.value > GetCurrentHealthPercentage))
                continue;
            float conditionalModdifier = 1;
            switch (effect.target)
            {
                case EffectTarget.SingleTarget:
                    if ((effect.conditional.type == ConditionalType.TargetMissingHealth && effect.conditional.value > 1 - target.GetCurrentHealthPercentage) || (effect.conditional.type == ConditionalType.TargetCurrentHealth && effect.conditional.value > target.GetCurrentHealthPercentage) || (effect.conditional.type == ConditionalType.TargetIsStunned && target.IsStunned))
                        continue;
                    conditionalModdifier = 1;
                    if (effect.conditional.type == ConditionalType.TargetMissingHealthModifier)
                    {
                        conditionalModdifier += effect.conditional.value * (1 - target.GetCurrentHealthPercentage);
                    }
                    else if (effect.conditional.type == ConditionalType.TargetCurrentHealthModifier)
                    {
                        conditionalModdifier += effect.conditional.value * target.GetCurrentHealthPercentage;
                    }
                    if (effect.isHealing)
                    {
                        bool crit = Random.Range(0f, 1f) <= critChance + effect.critModifier;
                        target.ReceiveHealing(effect.damageModifier * magic * healingDoneMod * (crit ? 2 : 1) * conditionalModdifier, out float healingDone);
                        for (int h = 0; h < effect.onHitEffects.Length; h++)
                        {
                            TriggerEffectOnTarget(target, effect.onHitEffects[h], true);
                        }
                        if (crit)
                        {
                            for (int c = 0; c < effect.onCritEffects.Length; c++)
                            {
                                TriggerEffectOnTarget(target, effect.onCritEffects[c], true);
                            }
                        }
                    }
                    else
                    {
                        if (target.CheckToHit(accuracy + effect.accuracy))
                        {
                            bool crit = Random.Range(0f, 1f) <= critChance + effect.critModifier;
                            target.TakeDamage(effect.damageModifier * magic * (crit ? 2 : 1) * conditionalModdifier, DamageType.Magic, this, magicPierce + effect.pierce, out float damageDealt, out bool isKilled);
                            if (effect.lifeSteal > 0)
                                ReceiveHealing(damageDealt * effect.lifeSteal, out _);
                            for (int h = 0; h < effect.onHitEffects.Length; h++)
                            {
                                TriggerEffectOnTarget(target, effect.onHitEffects[h]);
                            }
                            if (crit)
                            {
                                for (int c = 0; c < effect.onCritEffects.Length; c++)
                                {
                                    TriggerEffectOnTarget(target, effect.onCritEffects[c]);
                                }
                            }
                            if (isKilled)
                            {
                                for (int c = 0; c < effect.onKillEffects.Length; c++)
                                {
                                    TriggerEffectOnTarget(target, effect.onKillEffects[c]);
                                }
                                targetParty.Remove(target);
                            }
                        }
                    }
                    break;
                case EffectTarget.RandomTarget:
                    int rTarget = Random.Range(0, targetParty.Count);
                    if ((effect.conditional.type == ConditionalType.TargetMissingHealth && effect.conditional.value > 1 - targetParty[rTarget].GetCurrentHealthPercentage) || (effect.conditional.type == ConditionalType.TargetCurrentHealth && effect.conditional.value > targetParty[rTarget].GetCurrentHealthPercentage) || (effect.conditional.type == ConditionalType.TargetIsStunned && targetParty[rTarget].IsStunned))
                        continue;
                    conditionalModdifier = 1;
                    if (effect.conditional.type == ConditionalType.TargetMissingHealthModifier)
                    {
                        conditionalModdifier += effect.conditional.value * (1 - targetParty[rTarget].GetCurrentHealthPercentage);
                    }
                    else if (effect.conditional.type == ConditionalType.TargetCurrentHealthModifier)
                    {
                        conditionalModdifier += effect.conditional.value * targetParty[rTarget].GetCurrentHealthPercentage;
                    }
                    if (effect.isHealing)
                    {
                        bool crit = Random.Range(0f, 1f) <= critChance + effect.critModifier;
                        targetParty[rTarget].ReceiveHealing(effect.damageModifier * magic * healingDoneMod * (crit ? 2 : 1) * conditionalModdifier, out float healingDone);
                        for (int h = 0; h < effect.onHitEffects.Length; h++)
                        {
                            TriggerEffectOnTarget(targetParty[rTarget], effect.onHitEffects[h], true);
                        }
                        if (crit)
                        {
                            for (int c = 0; c < effect.onCritEffects.Length; c++)
                            {
                                TriggerEffectOnTarget(targetParty[rTarget], effect.onCritEffects[c], true);
                            }
                        }
                    }
                    else
                    {
                        if (targetParty[rTarget].CheckToHit(accuracy + effect.accuracy))
                        {
                            bool crit = Random.Range(0f, 1f) <= critChance + effect.critModifier;
                            targetParty[rTarget].TakeDamage(effect.damageModifier * magic * (crit ? 2 : 1) * conditionalModdifier, DamageType.Magic, this, magicPierce + effect.pierce, out float damageDealt, out bool isKilled);
                            if (effect.lifeSteal > 0)
                                ReceiveHealing(damageDealt * effect.lifeSteal, out _);
                            for (int h = 0; h < effect.onHitEffects.Length; h++)
                            {
                                TriggerEffectOnTarget(targetParty[rTarget], effect.onHitEffects[h]);
                            }
                            if (crit)
                            {
                                for (int c = 0; c < effect.onCritEffects.Length; c++)
                                {
                                    TriggerEffectOnTarget(targetParty[rTarget], effect.onCritEffects[c]);
                                }
                            }
                            if (isKilled)
                            {
                                for (int c = 0; c < effect.onKillEffects.Length; c++)
                                {
                                    TriggerEffectOnTarget(targetParty[rTarget], effect.onKillEffects[c]);
                                }
                                targetParty.Remove(targetParty[rTarget]);
                            }
                        }
                    }
                    break;
                case EffectTarget.GroupTarget:

                    if (effect.isHealing)
                    {
                        for (int i = 0; i < targetParty.Count; i++)
                        {
                            if ((effect.conditional.type == ConditionalType.TargetMissingHealth && effect.conditional.value > 1 - targetParty[i].GetCurrentHealthPercentage) || (effect.conditional.type == ConditionalType.TargetCurrentHealth && effect.conditional.value > targetParty[i].GetCurrentHealthPercentage) || (effect.conditional.type == ConditionalType.TargetIsStunned && targetParty[i].IsStunned))
                                continue;
                            conditionalModdifier = 1;
                            if (effect.conditional.type == ConditionalType.TargetMissingHealthModifier)
                            {
                                conditionalModdifier += effect.conditional.value * (1 - targetParty[i].GetCurrentHealthPercentage);
                            }
                            else if (effect.conditional.type == ConditionalType.TargetCurrentHealthModifier)
                            {
                                conditionalModdifier += effect.conditional.value * targetParty[i].GetCurrentHealthPercentage;
                            }

                            bool crit = Random.Range(0f, 1f) <= critChance + effect.critModifier;
                            targetParty[i].ReceiveHealing(effect.damageModifier * magic * healingDoneMod * (crit ? 2 : 1) * conditionalModdifier, out float healingDone);
                            for (int h = 0; h < effect.onHitEffects.Length; h++)
                            {
                                TriggerEffectOnTarget(targetParty[i], effect.onHitEffects[h], true);
                            }
                            if (crit)
                            {
                                for (int c = 0; c < effect.onCritEffects.Length; c++)
                                {
                                    TriggerEffectOnTarget(targetParty[i], effect.onCritEffects[c], true);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < targetParty.Count; i++)
                        {
                            if ((effect.conditional.type == ConditionalType.TargetMissingHealth && effect.conditional.value > 1 - targetParty[i].GetCurrentHealthPercentage) || (effect.conditional.type == ConditionalType.TargetCurrentHealth && effect.conditional.value > targetParty[i].GetCurrentHealthPercentage) || (effect.conditional.type == ConditionalType.TargetIsStunned && targetParty[i].IsStunned))
                                continue;
                            conditionalModdifier = 1;
                            if (effect.conditional.type == ConditionalType.TargetMissingHealthModifier)
                            {
                                conditionalModdifier += effect.conditional.value * (1 - targetParty[i].GetCurrentHealthPercentage);
                            }
                            else if (effect.conditional.type == ConditionalType.TargetCurrentHealthModifier)
                            {
                                conditionalModdifier += effect.conditional.value * targetParty[i].GetCurrentHealthPercentage;
                            }

                            if (targetParty[i].CheckToHit(accuracy + effect.accuracy))
                            {
                                bool crit = Random.Range(0f, 1f) <= critChance + effect.critModifier;
                                targetParty[i].TakeDamage(effect.damageModifier * magic * (crit ? 2 : 1) * conditionalModdifier, DamageType.Magic, this, magicPierce + effect.pierce, out float damageDealt, out bool isKilled);
                                if (effect.lifeSteal > 0)
                                    ReceiveHealing(damageDealt * effect.lifeSteal, out _);
                                for (int h = 0; h < effect.onHitEffects.Length; h++)
                                {
                                    TriggerEffectOnTarget(targetParty[i], effect.onHitEffects[h]);
                                }
                                if (crit)
                                {
                                    for (int c = 0; c < effect.onCritEffects.Length; c++)
                                    {
                                        TriggerEffectOnTarget(targetParty[i], effect.onCritEffects[c]);
                                    }
                                }
                                if (isKilled)
                                {
                                    for (int c = 0; c < effect.onKillEffects.Length; c++)
                                    {
                                        TriggerEffectOnTarget(targetParty[i], effect.onKillEffects[c]);
                                    }
                                    targetParty.Remove(targetParty[i]);
                                }
                            }
                        }
                    }
                    break;
            }
        }
        yield return new WaitForSeconds(0.5f);
        EndTurn();
    }

    protected void TriggerEffectOnTarget(CharacterStats target, EffectTrigger effect, bool isFriendly = false)
    {
        float convertedEffectValue = effect.ammount;
        if (effect.statVar == StatVar.MagicHOT || effect.statVar == StatVar.MagicDOT || effect.statVar == StatVar.MagicHealing)
            convertedEffectValue *= magic;
        else if (effect.statVar == StatVar.AttackHOT || effect.statVar == StatVar.PhysicalDOT || effect.statVar == StatVar.AttackHealing)
            convertedEffectValue *= attack;

            if (effect.affectsSelf)
            ApplyBuff(effect.statVar, convertedEffectValue, effect.duration);
        else if (isFriendly)
            target.ApplyBuff(effect.statVar, convertedEffectValue, effect.duration);
        else
            target.AddDebuff(effect.statVar, convertedEffectValue, effect.duration);
    }
}
