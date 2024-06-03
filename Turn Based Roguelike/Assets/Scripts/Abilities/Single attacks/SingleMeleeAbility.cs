using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Standard melee attack", menuName = "ScriptableObjects/Abilities/Single Attacks/Default Melee Attack")]
public class SingleMeleeAbility : BaseAbility
{
    [Header("AbilityDetails")]
    [SerializeField] private float damageScaling;
    [SerializeField] private float bonusCritRate;

    protected override IEnumerator TriggerAbilityEffects(CombatPositionData caster, CombatPositionData[] validTargets)
    {
        yield return new WaitForSeconds(delayToInitialEffect);

        float critroll = Random.Range(0f, 1f) + bonusCritRate + caster.character.CritRate;
        validTargets[0].character.TakeDamage(caster.character.Attack * damageScaling * (critroll >= 1 ? 2 : 1), DamageType.Physical,  out _);
        if (resourceCost == 0)
            caster.character.AddSkillPoints();
        if (critroll >= 1)
            caster.character.OnCrit();
        yield return base.TriggerAbilityEffects(caster, validTargets);
    }
}
