using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class Ability
{
    [SerializeField] private string abilityName;
    [SerializeField] private List<AbilityEffect> abilityEffects = new List<AbilityEffect>();

    [SerializeField] private float castTime;
    [SerializeField] private float cooldown;
    //[SerializeField] public bool needsTarget = true;
    [SerializeField] public int deliveryType;
    public int DeliveryType {get => deliveryType; set => deliveryType = value;}


    public string getName(){
        return abilityName;
    }
    public AbilityEffect getEffect(){
        return abilityEffects[0];
    }
    public List<AbilityEffect> getEffects(){
        return abilityEffects;
    }
    public float getCastTime(){
        return castTime;
    }

    public float getCooldown(){
        return cooldown;
    }

    public void setName(string _abilityName){
        abilityName = _abilityName;
    }
    public void setEffect(AbilityEffect _abilityEffect){
        List<AbilityEffect> temp = new List<AbilityEffect>();
        temp.Add(_abilityEffect);
        abilityEffects = temp;
    }
    public void setEffects(List<AbilityEffect> _abilityEffects){
        abilityEffects = _abilityEffects;
    }
    public void addEffect(AbilityEffect _abilityEffect){
        abilityEffects.Add(_abilityEffect);
    }
    public void setCastTime(float _castTime){
        castTime = _castTime;
    }

    public void setCooldown(float _cooldown){
        cooldown = _cooldown;
    }


    public Ability(string inName, AbilityEffect inAbilityEffect, float inCastTime){
        abilityName = inName;
        abilityEffects.Add(inAbilityEffect);
        castTime = inCastTime;
        cooldown = 0.0f;
    }
    public Ability(string inName, AbilityEffect inAbilityEffect, float inCastTime, float inCooldown){
        abilityName = inName;
        abilityEffects.Add(inAbilityEffect);
        castTime = inCastTime;
        cooldown = inCooldown;
    }
    public Ability(string inName, List<AbilityEffect> inAbilityEffects, float inCastTime, float inCooldown){
        abilityName = inName;
        abilityEffects = inAbilityEffects;
        castTime = inCastTime;
        cooldown = inCooldown;
    }
    /*public Ability(string inName, AbilityEffect inAbilityEffect, float inCastTime, float inCooldown, bool _needsTarget){
        abilityName = inName;
        abilityEffect = inAbilityEffect;
        castTime = inCastTime;
        cooldown = inCooldown;
        needsTarget = _needsTarget;
    }*/
    public Ability clone(){
        // Returns a Copy of the ability with a COPY of the the name, a REF to the effect, and copy of castTime since it is just a value type

        //return new Ability(String.Copy(abilityName), abilityEffect, castTime, cooldown, needsTarget);
        return new Ability(String.Copy(abilityName), abilityEffects, castTime, cooldown);
        
    }
    public bool NeedsTargetActor(){
        switch(DeliveryType){
            case 0:
                return true;
                break;
            case 1:
                return true;
                break;
            case 2:
                return false;
                break;
            default:
                Debug.Log("Unknown Delivery Type");
                return false;
                break;
        }   
    }
    public bool NeedsTargetWP(){
        switch(DeliveryType){
            case 0:
                return false;
                break;
            case 1:
                return false;
                break;
            case 2:
                return true;
                break;
            default:
                Debug.Log("Unknown Delivery Type");
                return false;
                break;
        }   
    }

}