using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCallAbility : MonoBehaviour
{
    public int abilityIndex;

    public void UseAbility()
    {
        CombatManager.instance.currentlyActingCharacter.UseAbility(abilityIndex);
    }
}
