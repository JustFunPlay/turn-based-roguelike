using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New orbital laserBeam", menuName = "ScriptableObjects/Abilities/AoE Attacks/Orbital Laser")]
public class BigAssLaser : BaseAbility
{
    [Header("AbilityDetails")]
    [SerializeField] private int numberOfHits = 5;
    [SerializeField] private float damageScalingPerHit = 0.5f;
    [SerializeField] private float delayBetweenHits = 0.25f;
    [SerializeField] private float bonusCritRate;

    [SerializeField] private GameObject laserBeam;

    [SerializeField] private int[] actionCamIndexes;

    protected override IEnumerator TriggerAbilityEffects(CombatPositionData caster, CombatPositionData[] validTargets)
    {
        CameraManager.Instance.SetActionCamPosition(actionCamIndexes[0]);
        yield return new WaitForSeconds(delayToInitialEffect);

        CameraManager.Instance.SetActionCamPosition(actionCamIndexes[1]);
        Vector3 laserPosition = new Vector3();
        foreach (CombatPositionData positionData in validTargets)
        {
            laserPosition += positionData.standingPosition.position;
        }
        laserPosition.x /= validTargets.Length;
        laserPosition.y = 0;
        laserPosition.z /= validTargets.Length;

        Instantiate(laserBeam, laserPosition, Quaternion.identity);

        for (int i = 0; i < numberOfHits; i++)
        {
            yield return new WaitForSeconds(delayBetweenHits);
            foreach (var target in validTargets)
            {
                float critroll = Random.Range(0f, 1f) + bonusCritRate + caster.character.CritRate;
                target.character.TakeDamage(caster.character.Magic * damageScalingPerHit * (critroll >= 1 ? 2 : 1), DamageType.Magic, out _);
                if (critroll >= 1)
                    caster.character.OnCrit();
            }
        }
        
        yield return base.TriggerAbilityEffects(caster, validTargets);
    }
}
