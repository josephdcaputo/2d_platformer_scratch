using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AbilityEffectPreset //rename to presets
{
    protected string effectName;
    protected int effectType; // 0=damage, 1=heal, 2=DoT, 3=Hot, 4= something else... tbd
    protected float power;
    protected float duration;
    protected float tickRate; // for now will be rounded
    protected GameObject particles;
    protected Action<Actor, Actor> startAction;
    protected Action<Actor, Actor> hitAction;
    protected Action<Actor, Actor> finishAction;
    protected int id; // Should be a positive unique identifer
    protected bool stackable;
    protected bool refreshable;
    public AbilityEffectPreset(string _effectName, int _effectType, float _power, float _duration = 0.0f,
                                float _tickRate = 0.0f, GameObject _particles = null, Action<Actor, Actor> _startAction = null,
                                Action<Actor, Actor> _hitAction = null, Action<Actor, Actor> _finishAction = null, int _id = -1,
                                bool _stackable = false, bool _refreshable = true){
        effectName = _effectName;
        effectType = _effectType;
        power = _power;
        duration = _duration;
        tickRate = _tickRate;
        particles = _particles;
        startAction = _startAction;
        hitAction = _hitAction;
        finishAction = _finishAction;
        id = _id;
        stackable = _stackable;
        refreshable = _refreshable;

    }
    public string getEffectName(){
        return effectName;
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
    public GameObject getParticles(){
        return particles;
    }
    public Action<Actor, Actor> getStartAction(){
        return startAction;
    }
    public Action<Actor, Actor> getHitAction(){
        return hitAction;
    }
    public Action<Actor, Actor> getFinishAction(){
        return finishAction;
    }
    public int getID(){
        return id;
    }
    public bool isStackable(){
        return stackable;
    }
    public bool isRefreshable(){
        return refreshable;
    }
    
}
