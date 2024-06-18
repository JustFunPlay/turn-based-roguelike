using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New random spell attacks", menuName = "ScriptableObjects/Abilities/Bounce Attacks/Random Spell Attacks")]
public class RandomMagicShots : BaseAbility
{
    [Header("Ability Effects")]
    [SerializeField] private int attacks;
    [SerializeField] private float damageScaling;
    [SerializeField] private float bonusCritRate;
    [SerializeField] private float delayBetweenAttacks;
    [SerializeField] private MagicShotProjectile projectile;
    private int receivedHits = 0;
 
    protected override IEnumerator TriggerAbilityEffects(CombatPositionData caster, CombatPositionData[] validTargets)
    {
        yield return new WaitForSeconds(delayToInitialEffect);
        receivedHits = 0;
        float damageToDo = caster.character.Magic * damageScaling;
        for (int i = 0; i < attacks; i++)
        {
            CombatPositionData target = CombatManager.instance.GetRandomTarget(caster, targeting);
            float critroll = Random.Range(0f, 1f) + bonusCritRate + caster.character.CritRate;
            MagicShotProjectile newProjectile = Instantiate(projectile, caster.character.abilityPoints[i%caster.character.abilityPoints.Length].position, Quaternion.identity);
            newProjectile.InitializeProjectile(caster, target, damageToDo, critroll, this);

            yield return new WaitForSeconds(delayBetweenAttacks);

        }
    }

    public void OnHit(CombatPositionData caster, bool isCrit)
    {
        receivedHits++;
        if (isCrit)
            caster.character.OnCrit();
        if (receivedHits == attacks)
        {
            GameManager.instance.StartCoroutine(EndAbility(caster));
        }
    }

    private IEnumerator EndAbility(CombatPositionData caster)
    {
        yield return new WaitForSeconds(delayToEnd);
        caster.character.EndTurn();
    }
}
