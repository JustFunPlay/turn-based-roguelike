using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBuff : StatusEffect
{
    float armorBuff;
    float mrBuff;
    public void OnApplication(CharacterVisual target, int duration, float armorBuff, float mrBuff) 
    {
        this.armorBuff = armorBuff;
        this.mrBuff = mrBuff;
        target.UpdateStat(armorBuff, StatVar.Armor);
        target.UpdateStat(mrBuff, StatVar.MagicResist);
        base.OnApplication(target, duration);
    }

    public override void OnRemove()
    {
        character.UpdateStat(-armorBuff, StatVar.Armor);
        character.UpdateStat(-mrBuff, StatVar.MagicResist);
        base.OnRemove();
    }
}
