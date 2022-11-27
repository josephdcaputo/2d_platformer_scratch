using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName="ApplyBuff", menuName = "HBCsystem/ApplyBuff")]
public class ApplyBuff : DeliveryEff
{
    //Maybe this should just be an id instead of the whole buff but
    // for now this will work

    [SerializeField]protected Buff buffID; //Make this actually an id in the future
    //[SerializeField]protected int buffID;
    public override void startEffect(Actor _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null){
        Debug.Log("Apply Buff! To " + _target.getActorName() + "| " + _caster.getActorName());
        // Copy buff id __ and atatch it to target's buffs List<>
        /*

            Some way to search for a buff id here

            Then the output is put into this function below's Add()
        
        */
        Buff tempBuff_ref = buffID.clone();
        //Debug.Log("ApplyBuff caster: " + (_caster != null ? _caster.getActorName() : "caster is null"));
        tempBuff_ref.target = _secondaryTarget;
        tempBuff_ref.caster = _caster;
        //tempBuff_ref.eInstructs = eInstructs;
        _target.applyBuff(tempBuff_ref);
        //target.getBuffs().Add( _____.findBuff(buffID) );
    }
    public Buff getBuffID(){
        return buffID;
    }
    public void setBuffID(Buff _buff){
        buffID = buffID;
    }
    public ApplyBuff(){}
    public ApplyBuff(string _effectName, int _id, Buff _buffID){
        effectName = _effectName;
        id = _id;
        buffID = _buffID;
    }
    public override AbilityEff clone()
    {
        ApplyBuff temp_ref = ScriptableObject.CreateInstance(typeof (ApplyBuff)) as ApplyBuff;
        temp_ref.effectName = effectName;
        temp_ref.id = id;
        temp_ref.power = power;
        temp_ref.powerScale = powerScale;
        temp_ref.buffID = buffID;
        temp_ref.targetIsSecondary = targetIsSecondary;
        //temp_ref.eInstructs = new List<EffectInstruction>();
        foreach (EffectInstruction eI in eInstructs){
            temp_ref.eInstructs.Add(eI.clone());
        }
        return temp_ref;
    }
}