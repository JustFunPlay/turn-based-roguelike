using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Standard ranged attack", menuName = "ScriptableObjects/Abilities/Single Attacks/Default Ranged Attack")]
public class StandardRangedAttack : BaseAbility
{
    [Header("AbilityDetails")]
    [SerializeField] private float damageScaling;
    [SerializeField] private float bonusCritRate;
    [SerializeField] private DamageType damageType;
    [SerializeField] private bool showTarget;
    [SerializeField] private MagicShotProjectile projectile;
    [SerializeField] private int abilityPointIndex;

    protected override IEnumerator TriggerAbilityEffects(CombatPositionData caster, CombatPositionData[] validTargets)
    {
        if (showTarget) CameraManager.Instance.SetFocusPosition(validTargets[0]);
        yield return new WaitForSeconds(delayToInitialEffect);

        float critroll = Random.Range(0f, 1f) + bonusCritRate + caster.character.CritRate;

        MagicShotProjectile newProjectile = Instantiate(projectile, caster.character.abilityPoints[abilityPointIndex].position, caster.character.abilityPoints[abilityPointIndex].rotation);
        newProjectile.InitializeProjectile(caster, validTargets[0], caster.character.Attack * damageScaling, critroll, damageType, this);
    }

    public override void AltEndAbility(CombatPositionData caster, bool isCrit)
    {
        if (isCrit)
            caster.character.OnCrit();
        if (resourceCost == 0)
            caster.character.AddSkillPoints();
        base.AltEndAbility(caster, isCrit);
    }
}
