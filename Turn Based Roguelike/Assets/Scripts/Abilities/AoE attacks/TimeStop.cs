using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New time stop", menuName = "ScriptableObjects/Abilities/AoE Attacks/Time Stop")]
public class TimeStop : BaseAbility
{
    [SerializeField] private float timeStopDuration;
    [SerializeField] private float damageScaling;
    [SerializeField] private float bonusCritRate;

    protected override IEnumerator TriggerAbilityEffects(CombatPositionData caster, CombatPositionData[] validTargets)
    {
        yield return new WaitForSeconds(delayToInitialEffect);

        CameraManager.Instance.ToggleInverseFilter(true);
        Time.timeScale = 0.000000001f;
        yield return new WaitForSecondsRealtime(timeStopDuration);
        Time.timeScale = 1;
        CameraManager.Instance.ToggleInverseFilter(false);

        float damageToDo = caster.character.Magic * damageScaling;
        foreach (CombatPositionData target in validTargets)
        {
            float critroll = Random.Range(0f, 1f) + bonusCritRate + caster.character.CritRate;
            target.character.TakeDamage(damageToDo * (critroll >= 1 ? 2 : 1), DamageType.Magic, out _);
            if (critroll >= 1)
            {
                caster.character.OnCrit();
            }
        }

        yield return base.TriggerAbilityEffects(caster, validTargets);
    }
}
