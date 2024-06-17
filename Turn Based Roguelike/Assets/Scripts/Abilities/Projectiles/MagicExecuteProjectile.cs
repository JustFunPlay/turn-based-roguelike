using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MagicExecuteProjectile : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    private float damageToDo;
    public void InitializeProjectile(CombatPositionData caster, CombatPositionData target, float baseDamage, float maxBonusDamage, float missingHealthCap, float critRoll, MagicExecute ability)
    {
        float targetMissingHpPercentage = 1 - (target.character.CurrentHP / target.character.MaxHP);
        damageToDo = baseDamage + (maxBonusDamage * (Mathf.Clamp(targetMissingHpPercentage, 0, missingHealthCap) / missingHealthCap));
        Debug.Log($"Black hole will do {damageToDo} damage");
        StartCoroutine(FlyToTarget(caster, target, 1f + (targetMissingHpPercentage * 4) / missingHealthCap, critRoll, ability));
    }

    private IEnumerator FlyToTarget(CombatPositionData caster, CombatPositionData target, float maxSize, float critRoll, MagicExecute ability)
    {
        for (float t = 0f; t <= 0.4f; t += Time.fixedDeltaTime)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * maxSize, t/0.4f);
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(lifeTime);
        bool killingBlow = target.character.TakeDamage(damageToDo * (critRoll >= 1 ? 2 : 1), DamageType.Magic, out _);
        GameManager.instance.StartCoroutine(ability.EndAbility(caster, critRoll >= 1, killingBlow));
        for (float t = 0f; t <= 0.4f; t += Time.fixedDeltaTime)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * maxSize, 1 - t / 0.4f);
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }
}
