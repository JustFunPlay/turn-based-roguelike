using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New hybrid multi-attack", menuName = "ScriptableObjects/Abilities/Single Attacks/Hybrid Multi Attack")]
public class MultiHitHybridAttack : BaseAbility
{
    [Header("AbilityDetails")]
    [SerializeField] private HybridAttackSegment[] attacks;

    [Header("Animation Support")]
    [SerializeField] private float moveDuration;
    [SerializeField] private float delayToHit;
    [SerializeField] private float distanceToTarget = 1f;


    protected override IEnumerator TriggerAbilityEffects(CombatPositionData caster, CombatPositionData[] validTargets)
    {
        //yield return new WaitForSeconds(delayBeforeMove);
        yield return new WaitForSeconds(delayToInitialEffect);
        for (float t = 0; t <= moveDuration; t += Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();
            caster.character.transform.position = Vector3.Lerp(caster.standingPosition.position, validTargets[0].standingPosition.position + validTargets[0].standingPosition.forward * distanceToTarget, t / moveDuration);
        }

        yield return new WaitForSeconds(delayToHit);
        //CameraManager.Instance.SetTargetPosition(validTargets[0]);
        for (int i = 0; i < attacks.Length; i++)
        {
            float critroll = Random.Range(0f, 1f) + attacks[i].bonusCritRate + caster.character.CritRate;
            validTargets[0].character.TakeDamage(caster.character.Attack * attacks[i].physicalDamageModifier * (critroll >= 1 ? 2 : 1), DamageType.Physical, out _);
            validTargets[0].character.TakeDamage(caster.character.Magic * attacks[i].magicDamageModifier * (critroll >= 1 ? 2 : 1), DamageType.Magic, out _);
            if (critroll >= 1)
                caster.character.OnCrit();

            yield return new WaitForSeconds(attacks[i].delayAfterHit);

        }

        for (float t = delayToEnd; t >= 0; t -= Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();
            caster.character.transform.position = Vector3.Lerp(caster.standingPosition.position, validTargets[0].standingPosition.position + validTargets[0].standingPosition.forward * distanceToTarget, t / delayToEnd);
        }
        caster.character.EndTurn();
        //yield return base.TriggerAbilityEffects(caster, validTargets);
    }
}


[System.Serializable]
public class HybridAttackSegment
{
    public float physicalDamageModifier;
    public float magicDamageModifier;
    public float bonusCritRate;
    public float delayAfterHit;
}
