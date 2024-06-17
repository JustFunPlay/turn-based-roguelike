using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New standard buffing ability", menuName = "ScriptableObjects/Abilities/Support/Apply Buff")]
public class ApplyBuffTargets : BaseAbility
{
    [Header("Buff effects")]
    [SerializeField] private StatBuff[] buffs;
    protected override IEnumerator TriggerAbilityEffects(CombatPositionData caster, CombatPositionData[] validTargets)
    {
        CameraManager.Instance.SetTargetPosition(caster);
        yield return new WaitForSeconds(delayToInitialEffect);

        if (validTargets.Length > 1)
            CameraManager.Instance.SetTeamView(validTargets[0]);
        else
            CameraManager.Instance.SetTargetPosition(validTargets[0]);
        
        foreach (CombatPositionData target in validTargets)
        {
            for (int i = 0; i < buffs.Length; i++)
            {
                StatBuffEffect newBuff = CreateInstance<StatBuffEffect>();
                newBuff.OnApplication(target.character, buffs[i].duration + (target == caster ? 1 : 0), buffs[i].value, buffs[i].stat);
            }
        }
        yield return base.TriggerAbilityEffects(caster, validTargets);
    }
}