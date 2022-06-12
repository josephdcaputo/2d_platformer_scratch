using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class Ability
{
    [SerializeField] private string abilityName;
    [SerializeField] private AbilityEffect abilityEffect;

    [SerializeField] private float castTime;
    [SerializeField] private float cooldown;
    [SerializeField] private float cooldownRemaining = 0.0f;

    public string getName(){
        return abilityName;
    }
    public AbilityEffect getEffect(){
        return abilityEffect;
    }
    public float getCastTime(){
        return castTime;
    }

    public float getCooldown(){
        return cooldown;
    }
    public float getCooldownRemaining(){
        return cooldownRemaining;
    }
    public void setName(string _abilityName){
        abilityName = _abilityName;
    }
    public void setEffect(AbilityEffect _abilityEffect){
        abilityEffect = _abilityEffect;
    }
    public void setCastTime(float _castTime){
        castTime = _castTime;
    }

    public void setCooldown(float _cooldown){
        cooldown = _cooldown;
    }
    public void setCooldownRemaining(float _cooldownRemaining){
        cooldownRemaining = _cooldownRemaining;
    }
    public void updateCooldownRemaining(){
        cooldownRemaining -= Time.deltaTime;
    }

    public Ability(string inName, AbilityEffect inAbilityEffect, float inCastTime){
        abilityName = inName;
        abilityEffect = inAbilityEffect;
        castTime = inCastTime;
        cooldown = 0.0f;
    }
    public Ability(string inName, AbilityEffect inAbilityEffect, float inCastTime, float inCooldown){
        abilityName = inName;
        abilityEffect = inAbilityEffect;
        castTime = inCastTime;
        cooldown = inCooldown;
    }
    public Ability clone(){
        // Returns a Copy of the ability with a COPY of the the name, a REF to the effect, and copy of castTime since it is just a value type

        Ability abilityToReturn = new Ability(String.Copy(abilityName), abilityEffect, castTime, cooldown);
        return abilityToReturn;
    }

    public bool onCooldown(){
        if(cooldownRemaining > 0.0f){
        Debug.Log("Ability: " + abilityName + "is on cooldown!");
        return true;
        }
        else{
            return false;
        }
    }

    public void setRemaining(){
        cooldownRemaining = cooldown;
    }
}
