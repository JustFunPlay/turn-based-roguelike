using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New backstab melee attack", menuName = "ScriptableObjects/Abilities/Single Attacks/Backstab Melee Attack")]
public class BackStabAttack : BaseAbility
{
    [Header("AbilityDetails")]
    [SerializeField] private float damageScaling;
    [SerializeField] private float bonusCritRate;

    [Header("Animation Support")]
    [SerializeField] private float distanceBehindTarget = 1f;
    [SerializeField] private GameObject teleportParticle;

    protected override IEnumerator TriggerAbilityEffects(CombatPositionData caster, CombatPositionData[] validTargets)
    {
        Instantiate(teleportParticle, caster.character.transform.position + caster.character.transform.up * caster.character.centreOfMassOffset, Quaternion.identity);
        caster.character.transform.position = validTargets[0].standingPosition.position - validTargets[0].standingPosition.forward * distanceBehindTarget;
        caster.character.transform.LookAt(validTargets[0].standingPosition, Vector3.up);
        yield return new WaitForSeconds(delayToInitialEffect);
        
        float critroll = Random.Range(0f, 1f) + bonusCritRate + caster.character.CritRate;
        validTargets[0].character.TakeDamage(caster.character.Attack * damageScaling * (critroll >= 1 ? 2 : 1), DamageType.Physical, out _);
        if (resourceCost == 0)
            caster.character.AddSkillPoints();
        if (critroll >= 1)
            caster.character.OnCrit();

        yield return new WaitForSeconds(delayToEnd);

        Instantiate(teleportParticle, caster.character.transform.position + caster.character.transform.up * caster.character.centreOfMassOffset, Quaternion.identity);
        caster.character.transform.position = caster.standingPosition.position;
        caster.character.transform.LookAt(caster.standingPosition.position + caster.standingPosition.forward, Vector3.up);

        caster.character.EndTurn();
        //yield return base.TriggerAbilityEffects(caster, validTargets);
    }
}
