using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : ScriptableObject
{
    public virtual void OnApplication() { }
    public virtual void OnStartTurn() { }
    public virtual void OnEndTurn() { }

}
