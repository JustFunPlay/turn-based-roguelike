using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New self-healing spell attack", menuName = "ScriptableObjects/Abilities/Single Attacks/Self Healing Spell Attack")]
public class HealSelfAttackSpell : BaseAbility
{
    [Header("AbilityDetails")]
    [SerializeField] private float damageScaling;
    [SerializeField] private float bonusCritRate;
    [SerializeField] private float selfHealingModifer;

    //[Header("Animation Support")]
    //[SerializeField] private float delayBeforeMove;
    //[SerializeField] private float delayToHit;
    //[SerializeField] private float delayAfterHit;
    //[SerializeField] private float distanceToTarget = 1f;


    protected override IEnumerator TriggerAbilityEffects(CombatPositionData caster, CombatPositionData[] validTargets)
    {
        yield return new WaitForSeconds(delayToInitialEffect);

        //yield return new WaitForSeconds(delayToHit);
        //CameraManager.Instance.SetTargetPosition(validTargets[0]);
        float critroll = Random.Range(0f, 1f) + bonusCritRate + caster.character.CritRate;
        validTargets[0].character.TakeDamage(caster.character.Attack * damageScaling * (critroll >= 1 ? 2 : 1), DamageType.Physical, out _);
        if (critroll >= 1)
            caster.character.OnCrit();
        caster.character.RestoreHealth(caster.character.Magic * selfHealingModifer, out _);
        //yield return new WaitForSeconds(delayAfterHit);

        yield return base.TriggerAbilityEffects(caster, validTargets);
    }
}
