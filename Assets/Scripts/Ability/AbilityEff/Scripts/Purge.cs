﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using OldBuff;

[System.Serializable]
[CreateAssetMenu(fileName="Purge", menuName = "HBCsystem/Purge")]
public class Purge : AbilityEff
{
  
    
    public override void startEffect(Actor _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null){
        // Debug.Log("Apply Buff");
        _target.buffHandler.RemoveRandomBuff(b => (b.BuffSO.isDebuff == false) && b.BuffSO.dispellable);
    }
    
    public Purge(){}
    
    public override AbilityEff clone()
    {
        Purge temp_ref = ScriptableObject.CreateInstance(typeof (Purge)) as Purge;
        copyBase(temp_ref);

        //temp_ref.eInstructs = new List<EffectInstruction>();
        
        return temp_ref;
    }
}
