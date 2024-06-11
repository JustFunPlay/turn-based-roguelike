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
        target.UpdateStat(buffAmmount, buffEffect);
        base.OnApplication(target, duration);
    }

    public override void OnRemove()
    {
        character.UpdateStat(--buffAmmount, buffEffect);
        base.OnRemove();
    }
}
