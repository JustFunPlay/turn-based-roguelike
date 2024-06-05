using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New standard healing ability", menuName = "ScriptableObjects/Abilities/Support/Standard Heal")]
public class HealTargets : BaseAbility
{
    [Header("Ability Details")]
    [SerializeField] private float physicalHealingScaling;
    [SerializeField] private float magicHealingScaling;
    [SerializeField] private float targetMaxHealthScaling;
    [SerializeField] private float targetMissingHealthScaling;
    [SerializeField] private float bonusCritRate;

    [Header("Visualisation")]
    [SerializeField] private GameObject healParticle;
    protected override IEnumerator TriggerAbilityEffects(CombatPositionData caster, CombatPositionData[] validTargets)
    {
        CameraManager.Instance.SetTargetPosition(caster);
        yield return new WaitForSeconds(delayToInitialEffect);

        if (validTargets.Length > 1)
            CameraManager.Instance.SetTeamView(validTargets[0]);
        else
            CameraManager.Instance.SetTargetPosition(validTargets[0]);
        float baseHealValue = magicHealingScaling * caster.character.Magic + physicalHealingScaling * caster.character.Attack;
        foreach (CombatPositionData target in validTargets)
        {
            float targetHpScaling = target.character.MaxHP * targetMaxHealthScaling + targetMissingHealthScaling * (target.character.MaxHP - target.character.CurrentHP);
            float critroll = Random.Range(0f, 1f) + bonusCritRate + caster.character.CritRate;
            target.character.RestoreHealth((baseHealValue + targetHpScaling) * (critroll >= 1 ? 2 : 1), out _);
            if (healParticle != null)
                Instantiate(healParticle, target.standingPosition.position, target.standingPosition.rotation);
        }
        yield return base.TriggerAbilityEffects(caster, validTargets);
    }
}
