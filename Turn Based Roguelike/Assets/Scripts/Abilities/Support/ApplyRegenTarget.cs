using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New regen ability", menuName = "ScriptableObjects/Abilities/Support/Apply Regen")]
public class ApplyRegenTarget : BaseAbility
{
    [Header("Regen effects")]
    [SerializeField] private float regenModifier;
    [SerializeField] private int duration;
    protected override IEnumerator TriggerAbilityEffects(CombatPositionData caster, CombatPositionData[] validTargets)
    {
        CameraManager.Instance.SetTargetPosition(caster);
        yield return new WaitForSeconds(delayToInitialEffect);

        if (validTargets.Length > 1)
            CameraManager.Instance.SetTeamView(validTargets[0]);
        else
            CameraManager.Instance.SetTargetPosition(validTargets[0]);
        float regenValue = caster.character.Magic * regenModifier;
        foreach (CombatPositionData target in validTargets)
        {   
            HealOverTime newRegen = CreateInstance<HealOverTime>();
            newRegen.OnApplication(target.character, duration, regenValue);
        }
        yield return base.TriggerAbilityEffects(caster, validTargets);
    }
}
