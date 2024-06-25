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

    [Header("Animation Support")]
    [SerializeField] private float delayToHit;
    [SerializeField] private float delayAfterHit;
    [SerializeField] private float distanceToTarget = 1f;

    protected override IEnumerator TriggerAbilityEffects(CombatPositionData caster, CombatPositionData[] validTargets)
    {
        for (float t = 0; t <= delayToInitialEffect; t += Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();
            caster.character.transform.position = Vector3.Lerp(caster.standingPosition.position, validTargets[0].standingPosition.position + validTargets[0].standingPosition.forward * distanceToTarget, t / delayToInitialEffect);
        }

        yield return new WaitForSeconds(delayToHit);

        float critroll = Random.Range(0f, 1f) + bonusCritRate + caster.character.CritRate;
        validTargets[0].character.TakeDamage(caster.character.Attack * physicalDamageScaling * (critroll >= 1 ? 2 : 1), DamageType.Physical, out _);
        validTargets[0].character.TakeDamage(caster.character.Magic * magicDamageScaling * (critroll >= 1 ? 2 : 1), DamageType.Magic, out _);
        if (critroll >= 1)
            caster.character.OnCrit();
        yield return new WaitForSeconds(delayAfterHit);

        for (float t = delayToEnd; t >= 0; t -= Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();
            caster.character.transform.position = Vector3.Lerp(caster.standingPosition.position, validTargets[0].standingPosition.position + validTargets[0].standingPosition.forward * distanceToTarget, t / delayToEnd);
        }
        caster.character.EndTurn();
        //yield return base.TriggerAbilityEffects(caster, validTargets);
    }
}
