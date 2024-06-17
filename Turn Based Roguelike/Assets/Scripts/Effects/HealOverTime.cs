using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOverTime : StatusEffect
{
    float healingToDo;
    public void OnApplication(CharacterVisual target, int duration, float healingValue)
    {
        healingToDo = healingValue;
        OnApplication(target, duration);
    }
    public override void OnStartTurn()
    {
        character.RestoreHealth(healingToDo, out _);
        base.OnStartTurn();
        ReduceDuration();
    }

    public override void OnEndTurn() { }
}
