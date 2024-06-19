using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New armor-penetrating multi-attack", menuName = "ScriptableObjects/Abilities/Single Attacks/Penetrating Multi Attack")]
public class MultiHitPiercingAttack : BaseAbility
{
    [Header("AbilityDetails")]
    [SerializeField] private AttackSegment[] attacks;

    [Header("Animation Support")]
    [SerializeField] private float delayBeforeMove;
    [SerializeField] private float delayToHit;
    [SerializeField] private float overshootTime;
    [SerializeField] private float delayBeforeJumpBack;
    [SerializeField] private float distanceToTarget = 1f;


    protected override IEnumerator TriggerAbilityEffects(CombatPositionData caster, CombatPositionData[] validTargets)
    {
        
        yield return new WaitForSeconds(delayBeforeMove);
        for (float t = 0; t <= delayToInitialEffect; t += Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();
            caster.character.transform.position = Vector3.Lerp(caster.standingPosition.position, validTargets[0].standingPosition.position, t / delayToInitialEffect);
        }

        yield return new WaitForSeconds(delayToHit);
        //CameraManager.Instance.SetTargetPosition(validTargets[0]);
        for (int i = 0; i < attacks.Length; i++)
        {
            float critroll = Random.Range(0f, 1f) + attacks[i].bonusCritRate + caster.character.CritRate;
            validTargets[0].character.TakeDamage(caster.character.Attack * attacks[i].damageModifier * (critroll >= 1 ? 2 : 1), DamageType.Physical, out _, attacks[i].armorPenetration);
            if (critroll >= 1)
                caster.character.OnCrit();

            yield return new WaitForSeconds(attacks[i].delayAfterHit);

        }
        
        for (float t = 0; t <= overshootTime; t += Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();
            caster.character.transform.position = Vector3.Lerp(validTargets[0].standingPosition.position, validTargets[0].standingPosition.position + validTargets[0].standingPosition.forward * distanceToTarget, t / overshootTime);
        }

        yield return new WaitForSeconds(delayBeforeJumpBack);

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
public class AttackSegment
{
    public float damageModifier;
    public float bonusCritRate;
    public float armorPenetration;
    public float delayAfterHit;
}