using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New defense boost ability", menuName = "ScriptableObjects/Abilities/Support/Defense Boost")]

public class DefenseBoost : BaseAbility
{
    [Header("Ability Details")]
    [SerializeField] private float armorIncrease;
    [SerializeField] private float magicResistIncrease;
    [SerializeField] private int duration;

    [Header("Visualisation")]
    [SerializeField] private GameObject defenseParticle;
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
            DefenseBuff newbuff = CreateInstance<DefenseBuff>();

            newbuff.OnApplication(target.character, duration, armorIncrease, magicResistIncrease);
            if (defenseParticle != null)
                Instantiate(defenseParticle, target.standingPosition.position, target.standingPosition.rotation);
        }
        yield return base.TriggerAbilityEffects(caster, validTargets);
    }
}
