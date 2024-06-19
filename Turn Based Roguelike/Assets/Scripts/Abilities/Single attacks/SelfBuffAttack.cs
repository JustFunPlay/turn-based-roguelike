using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buffing Attack", menuName = "ScriptableObjects/Abilities/Single Attacks/Self Buffing Attack")]
public class SelfBuffAttack : BaseAbility
{
    [Header("AbilityDetails")]
    [SerializeField] private float damageScaling;
    [SerializeField] private float bonusCritRate;
    [SerializeField] private StatBuff[] buffs;

    [Header("Animation Support")]
    [SerializeField] private float delayToHit;
    [SerializeField] private float delayAfterHit;
    [SerializeField] private float distanceToTarget = 1f;


    protected override IEnumerator TriggerAbilityEffects(CombatPositionData caster, CombatPositionData[] validTargets)
    {
        for (float t = 0; t <= delayToInitialEffect; t += Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();
            caster.character.transform.position = Vector3.Lerp(caster.standingPosition.position, validTargets[0].standingPosition.position + validTargets[0].standingPosition.forward * distanceToTarget, t / delayToInitialEffect);
        }

        yield return new WaitForSeconds(delayToHit);
        //CameraManager.Instance.SetTargetPosition(validTargets[0]);
        float critroll = Random.Range(0f, 1f) + bonusCritRate + caster.character.CritRate;
        validTargets[0].character.TakeDamage(caster.character.Attack * damageScaling * (critroll >= 1 ? 2 : 1), DamageType.Physical, out _);
        if (critroll >= 1)
            caster.character.OnCrit();
        yield return new WaitForSeconds(delayAfterHit);

        for (int i = 0; i < buffs.Length; i++)
        {
            StatBuffEffect newBuff = CreateInstance<StatBuffEffect>();
            newBuff.OnApplication(caster.character, buffs[i].duration + 1, buffs[i].value, buffs[i].stat);
        }

        for (float t = delayToEnd; t >= 0; t -= Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();
            caster.character.transform.position = Vector3.Lerp(caster.standingPosition.position, validTargets[0].standingPosition.position + validTargets[0].standingPosition.forward * distanceToTarget, t / delayToEnd);
        }
        caster.character.EndTurn();
        //yield return base.TriggerAbilityEffects(caster, validTargets);
    }
}

[System.Serializable]
public class StatBuff
{
    public StatVar stat;
    public float value;
    public int duration;
}