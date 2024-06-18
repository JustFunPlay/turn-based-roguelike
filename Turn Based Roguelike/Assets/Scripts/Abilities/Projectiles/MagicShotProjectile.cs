using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicShotProjectile : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    private float damageToDo;
    public void InitializeProjectile(CombatPositionData caster, CombatPositionData target, float damage, float critRoll, RandomMagicShots ability)
    {
        transform.LookAt(target.standingPosition.position + target.standingPosition.up * target.character.centreOfMassOffset, Vector3.up);
        StartCoroutine(FlyToTarget(caster, target, damage, critRoll, ability));
    }

    private IEnumerator FlyToTarget(CombatPositionData caster, CombatPositionData target, float damage, float critRoll, RandomMagicShots ability)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = target.standingPosition.position + target.standingPosition.up * target.character.centreOfMassOffset;
        for (float t = 0f; t <= lifeTime; t += Time.fixedDeltaTime)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, t / lifeTime);
            yield return new WaitForFixedUpdate();
        }
        target.character.TakeDamage(damageToDo * (critRoll >= 1 ? 2 : 1), DamageType.Magic, out _);
        ability.OnHit(caster, critRoll >= 1);
        Destroy(gameObject);
    }
}
