using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBuffEffect : StatusEffect
{
    float buffAmmount;
    StatVar buffEffect;

    public void OnApplication(CharacterVisual target, int duration, float Value, StatVar buffType)
    {
        buffAmmount = Value;
        buffEffect = buffType;
        target.IncreaseStat(buffAmmount, buffEffect);
        base.OnApplication(target, duration);
    }

    public override void OnRemove()
    {
        character.DecreaseStat(buffAmmount, buffEffect);
        base.OnRemove();
    }
}
