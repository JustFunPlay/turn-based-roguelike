using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : ScriptableObject
{
    protected CharacterVisual character;
    protected int remainingDuration;
    protected void OnApplication(CharacterVisual target, int duration)
    {
        character = target;
        character.AddStatusEffect(this);
        remainingDuration = duration;

    }

    public virtual void OnStartTurn() { }
    public virtual void OnEndTurn() { ReduceDuration(); }

    protected void ReduceDuration()
    {
        remainingDuration--;
        if (remainingDuration <= 0 )
            OnRemove();
    }
    public virtual void OnRemove() { Destroy(this); }
}

public enum StatVar
{
    MaxHealth,
    Armor,
    MagicResist,
    Attack,
    Crit,
    Magic,
    ManaRegen,
    Speed,
    Stun
}