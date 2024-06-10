using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAbility : ScriptableObject
{
    public string abilityName;
    public int resourceCost;
    [Multiline] public string description;
    [SerializeField] protected float delayToInitialEffect = 0.5f;
    [SerializeField] protected float delayToEnd = 0.5f;
    [SerializeField] private Targeting targeting;
    [SerializeField] string animationName;

    public void GetTarget(CharacterVisual caster)
    {
        CombatManager.instance.PrepareTargetSelection(caster, targeting, ActivateAbility);
        Debug.Log($"{caster.gameObject.name} is getting target");
    }

    private void ActivateAbility(CombatPositionData caster, CombatPositionData[] validTargets)
    {
        GameManager.instance.StartCoroutine(TriggerAbilityEffects(caster, validTargets));
        caster.character.PlayAnimation(animationName);
        Debug.Log($"{caster.character.gameObject.name} is using {abilityName}");
    }

    protected virtual IEnumerator TriggerAbilityEffects(CombatPositionData caster, CombatPositionData[] validTargets)
    {
        yield return new WaitForSeconds(delayToEnd);
        Debug.Log($"{caster.character.gameObject.name} has finished using {abilityName}");
        caster.character.EndTurn();
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