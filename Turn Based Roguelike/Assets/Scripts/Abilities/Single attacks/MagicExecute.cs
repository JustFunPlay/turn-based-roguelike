using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New magic execute", menuName = "ScriptableObjects/Abilities/Single Attacks/Magic Execute Attack")]
public class MagicExecute : BaseAbility
{
    [Header("Ability effects")]
    [SerializeField] private float baseScaling;
    [SerializeField] private float maxBonusScaling;
    [SerializeField, Range(0, 1)] private float missingHealthCap;
    [SerializeField] private float bonusCritRate;
    [SerializeField] private int resourceRefund;
    [SerializeField] private StatBuff[] buffs;
    [SerializeField] private MagicExecuteProjectile projectile;


    protected override IEnumerator TriggerAbilityEffects(CombatPositionData caster, CombatPositionData[] validTargets)
    {
        yield return new WaitForSeconds(delayToInitialEffect);
        
        //CameraManager.Instance.SetTargetPosition(validTargets[0]);
        float critroll = Random.Range(0f, 1f) + bonusCritRate + caster.character.CritRate;

        MagicExecuteProjectile newOrb = Instantiate(projectile, validTargets[0].standingPosition.position + validTargets[0].standingPosition.up * validTargets[0].character.centreOfMassOffset, new Quaternion(1, 1, -1, 0));
        float baseDamage = caster.character.Magic * baseScaling;
        float bonusDamage = caster.character.Magic * maxBonusScaling;
        Debug.Log($"precalculated oblivon orb damage is {baseDamage} base damage and {bonusDamage} bonus damage, based on {caster.character.Magic} ability power");
        newOrb.InitializeProjectile(caster, validTargets[0], baseDamage, bonusDamage, missingHealthCap, critroll, this);



        //yield return base.TriggerAbilityEffects(caster, validTargets);
    }

    public IEnumerator EndAbility(CombatPositionData caster, bool isCrit, bool killingBlow)
    {
        if (isCrit)
            caster.character.OnCrit();
        if (killingBlow)
        {
            caster.character.AddResource(resourceRefund);
            for (int i = 0; i < buffs.Length; i++)
            {
                StatBuffEffect newBuff = CreateInstance<StatBuffEffect>();
                newBuff.OnApplication(caster.character, buffs[i].duration + 1, buffs[i].value, buffs[i].stat);
            }
        }
        yield return new WaitForSeconds(delayToEnd);
        caster.character.EndTurn();
    }
}
