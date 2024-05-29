using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAbility : ScriptableObject
{
    public string abilityName;
    public int resourceCost;
    [Multiline] public string description;
    [SerializeField] private float delayToEnd = 0.5f;
    [SerializeField] private Targeting targeting;
    [SerializeField] string animationName;

    public void GetTarget(CharacterVisual caster)
    {
        CombatManager.instance.PrepareTargetSelection(caster, targeting, ActivateAbility);
    }

    private void ActivateAbility(CombatPositionData caster, CombatPositionData[] validTargets)
    {
        GameManager.instance.StartCoroutine(TriggerAbilityEffects(caster, validTargets));
        caster.character.PlayAnimation(animationName);
    }

    protected virtual IEnumerator TriggerAbilityEffects(CombatPositionData caster, CombatPositionData[] validTargets)
    {
        yield return new WaitForSeconds(delayToEnd);
        CombatManager.instance.PerformEndOfActionChecks();
    }
}

public enum Targeting
{
    Self,
    Ally,
    AllyRandom,
    AllyTeam,
    Enemy,
    EnemyRandom,
    EnemyTeam
}