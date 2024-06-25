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

    [SerializeField] private MagicShotProjectile projectile;



    protected override IEnumerator TriggerAbilityEffects(CombatPositionData caster, CombatPositionData[] validTargets)
    {
        yield return new WaitForSeconds(delayToInitialEffect);

        float critroll = Random.Range(0f, 1f) + bonusCritRate + caster.character.CritRate;
        MagicShotProjectile newProjectile = Instantiate(projectile, caster.character.abilityPoints[0].position, caster.character.abilityPoints[0].rotation);
        yield return new WaitForSeconds(delayToEnd);
        newProjectile.InitializeProjectile(caster, validTargets[0], caster.character.Attack * damageScaling, critroll, this);
        //validTargets[0].character.TakeDamage(caster.character.Attack * damageScaling * (critroll >= 1 ? 2 : 1), DamageType.Physical, out _);
        //if (critroll >= 1)
        //    caster.character.OnCrit();
        //caster.character.RestoreHealth(caster.character.Magic * selfHealingModifer, out _);

        //yield return base.TriggerAbilityEffects(caster, validTargets);
    }

    public override void AltEndAbility(CombatPositionData caster, bool isCrit)
    {
        if (isCrit)
            caster.character.OnCrit();
        caster.character.RestoreHealth(caster.character.Magic * selfHealingModifer, out _);
        base.AltEndAbility(caster, isCrit);
    }
}
