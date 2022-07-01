using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
[System.Serializable]
public class AbilityEffect
{
    /*
        These are private bc they shoud be thought about as "starting points".

        If you want an effect but, for ex., want it to last a little longer 

        you call an Actor's applyAbilityEffect() then manipulate the ActiveAbilityEffect
        that it generates 
        
    */
    // Creates hiden backing field
    [SerializeField] protected string effectName;
    protected int effectType; // 0=damage, 1=heal, 2=DoT, 3=Hot, 4= something else... tbd
    public float power;
    public float duration;
    public float tickRate; // for now will be rounded
    public GameObject particles;
    public Action<Actor, Actor> startAction;
    public Action<Actor, Actor> hitAction;
    public Action<Actor, Actor> finishAction;

    public float lastTick = 0.0f; // time since last tick
    public float remainingTime = 0.0f;
    public bool start = false;
    public Actor caster;
    public Vector3 dashTarget;

    
public AbilityEffect(){
}
public AbilityEffect(string _effectName, int _effectType, float _power, float _duration, float _tickRate){
    baseInit(_effectName, _effectType, _power, _duration,_tickRate);
}
public AbilityEffect(string _effectName, int _effectType, float _power, float _duration, float _tickRate, GameObject _particles){
    baseInit(_effectName, _effectType, _power, _duration,_tickRate);
    particles = _particles;

}
public AbilityEffect(string _effectName, int _effectType, float _power, float _duration, float _tickRate, GameObject _particles,
                     Action<Actor, Actor> _startAction, Action<Actor , Actor> _hitAction, Action<Actor, Actor> _finishAction){
    baseInit(_effectName, _effectType, _power, _duration,_tickRate);
    particles = _particles;
    startAction = _startAction;
    hitAction = _hitAction;
    finishAction = _finishAction;
}

    public string getEffectName(){
        return effectName;
    }
    public void setEffectName(string _effectName){
        effectName = _effectName;
    }
    public int getEffectType(){
        return effectType;
    }
    public float getPower(){
        return power;
    }
    public float getDuration(){
        return duration;
    }
    public float getTickRate(){
        return tickRate;
    }
    public virtual void OnEffectFinish(Actor _caster, Actor _target){
        if(finishAction != null){
            finishAction(_caster, _target);
        }
        //Debug.Log("AE: deafult finish");
    }
    public virtual void OnEffectHit(Actor _caster, Actor _target){          //  Make this work in actor later
        if(hitAction != null){
            hitAction(_caster, _target);
        }
    }
    public virtual void OnEffectStart(Actor _caster, Actor _target){
        if(startAction != null){
            startAction(_caster, _target);
        }
    }
    
    public AbilityEffect clone(){
        //Debug.Log("vvvvvvv      Cloning.. " + effectName + "      vvvvvvv");
        return new AbilityEffect(String.Copy(effectName), effectType, power, duration, tickRate, particles, startAction, hitAction, finishAction);
    }
    

// ------------------------------------------Start/ hit/ finish effects-------------------------------------------------------
public void baseInit(string _effectName, int _effectType, float _power, float _duration, float _tickRate){
    //Debug.Log("Creating AbilityEffect: " + _effectName);
    effectName = _effectName;
    effectType = _effectType;
    power = _power;
    if((_effectType != 0) && (_effectType != 1))
        duration = _duration;
    else{
        if(_duration > 0.0f){
            duration = 0.0f;
            Debug.Log("Tried to make a damage or heal type with a durtion that isn't 0.0f. Setting duration to 0.0f");
        }
    }
    tickRate = MathF.Round(_tickRate);
}
public void delegateboltFinish(AbilityEffect _abilityEffect){
    Debug.Log("Delegatebolt finish!");
}
public void secondaryTestbolt(Actor caster, Actor target){
    caster.freeCast(PlayerAbilityData._castedDamage, target);
}
public void secondaryDoT(Actor caster, Actor target){
    caster.freeCast(PlayerAbilityData._instantAbility, target);
}


}