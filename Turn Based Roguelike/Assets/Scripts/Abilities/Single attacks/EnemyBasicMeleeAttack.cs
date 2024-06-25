using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New enemy melee basic attack", menuName = "ScriptableObjects/Abilities/Single Attacks/Enemy Melee Basic Attack")]
public class EnemyBasicMeleeAttack : BaseAbility
{
    [Header("AbilityDetails")]
    [SerializeField] private float damageScaling;
    [SerializeField] private float bonusCritRate;

    [Header("Animation Support")]
    [SerializeField] private float delayToHit;
    [SerializeField] private float delayAfterHit;
    [SerializeField] private float distanceToTarget = 1f;

    protected override IEnumerator TriggerAbilityEffects(CombatPositionData caster, CombatPositionData[] validTargets)
    {
        //yield return new WaitForSeconds(delayToInitialEffect);
        CameraManager.Instance.SetFocusPosition(validTargets[0]);
        for (float t = 0; t <= delayToInitialEffect; t += Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();
            caster.character.transform.position = Vector3.Lerp(caster.standingPosition.position, validTargets[0].standingPosition.position + validTargets[0].standingPosition.forward * distanceToTarget, t / delayToInitialEffect);
        }

        yield return new WaitForSeconds(delayToHit);
        float critroll = Random.Range(0f, 1f) + bonusCritRate + caster.character.CritRate;
        validTargets[0].character.TakeDamage(caster.character.Attack * damageScaling * (critroll >= 1 ? 2 : 1), DamageType.Physical, out _);
        if (resourceCost == 0)
            caster.character.AddSkillPoints();
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
