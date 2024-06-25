using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicShotProjectile : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    [SerializeField] private float timeBeforeEnd;

    public void InitializeProjectile(CombatPositionData caster, CombatPositionData target, float damage, float critRoll, BaseAbility ability)
    {
        transform.LookAt(target.standingPosition.position + target.standingPosition.up * target.character.centreOfMassOffset, Vector3.up);
        StartCoroutine(FlyToTarget(caster, target, damage, critRoll, ability));
    }

    private IEnumerator FlyToTarget(CombatPositionData caster, CombatPositionData target, float damage, float critRoll, BaseAbility ability)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = target.standingPosition.position + target.standingPosition.up * target.character.centreOfMassOffset;
        for (float t = 0f; t <= lifeTime; t += Time.fixedDeltaTime)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, t / lifeTime);
            yield return new WaitForFixedUpdate();
        }
        target.character.TakeDamage(damage * (critRoll >= 1 ? 2 : 1), DamageType.Magic, out _);
        yield return new WaitForSeconds(timeBeforeEnd);
        ability.AltEndAbility(caster, critRoll >= 1);
        Destroy(gameObject);
    }
}
