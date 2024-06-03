using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hybrid melee attack", menuName = "ScriptableObjects/Abilities/Single Attacks/Hybrid Melee Attack")]
public class HybridMeleeAbility : BaseAbility
{
    [Header("AbilityDetails")]
    [SerializeField] private float physicalDamageScaling;
    [SerializeField] private float magicDamageScaling;
    [SerializeField] private float bonusCritRate;


    protected override IEnumerator TriggerAbilityEffects(CombatPositionData caster, CombatPositionData[] validTargets)
    {
        yield return new WaitForSeconds(delayToInitialEffect);

        float critroll = Random.Range(0f, 1f) + bonusCritRate + caster.character.CritRate;
        validTargets[0].character.TakeDamage(caster.character.Attack * physicalDamageScaling * (critroll >= 1 ? 2 : 1), DamageType.Physical, out _);
        validTargets[0].character.TakeDamage(caster.character.Magic * magicDamageScaling * (critroll >= 1 ? 2 : 1), DamageType.Magic, out _);
        if (critroll >= 1)
            caster.character.OnCrit();
        yield return base.TriggerAbilityEffects(caster, validTargets);
    }
}
