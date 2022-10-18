using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EffectInstruction
{
    [SerializeField]public AbilityEff effect;
    [SerializeField]public int targetArg = 0;
    public void startEffect(Actor inTarget = null, NullibleVector3 inTargetWP = null, Actor inCaster = null, Actor inSecondaryTarget = null){
        //Debug.Log(inTargetWP == null ? "eInstruct: No targetWP" : ("eInstruct: wp = " + inTargetWP.Value.ToString()));
        effect.startEffect(inTarget, inTargetWP, inCaster, inSecondaryTarget);
    }
    Actor getTarget(Actor _target = null, NullibleVector3 _targetWP = null, Actor _caster = null){
        switch(targetArg){
            case(0):
                return _target;
                break;
            case(1):
                return _caster;
                break;
            default:
                return null;
                break;
        }
    }
    public void startApply(Actor inTarget = null, NullibleVector3 inTargetWP = null, Actor inCaster = null, Actor inSecondaryTarget = null){
        //Debug.Log(inTargetWP == null ? "eInstruct: No targetWP" : ("eInstruct: wp = " + inTargetWP.Value.ToString()));
        
        switch(targetArg){
            case(0):
                break;
            case(1):
                inTarget = inCaster;
                break;
            default:
                Debug.Log("EI: Unknown targetArg " + effect.effectName);
                break;
        }
        if(effect.targetIsSecondary){
            //Debug.Log("Target is 2ndary!");
            inSecondaryTarget = inTarget;
            inTarget = inCaster;
        }
        // switch(targetArg){
        //     case(0):
        //         inTarget.recieveEffect(this, inTargetWP, inCaster, inSecondaryTarget);
        //         break;
        //     case(1):
        //         inCaster.recieveEffect(this, inTargetWP, inCaster, inSecondaryTarget);
        //         break;
        //     default:
        //         Debug.Log("EI: Could not start effect: " + effect.effectName);
        //         break;
        // }
        //Debug.Log("startApply caster" + (inCaster != null ? inCaster.getActorName() : "caster is null"));
        inTarget.recieveEffect(this, inTargetWP, inCaster, inSecondaryTarget);
    }
    public EffectInstruction(){

    }
    public EffectInstruction(AbilityEff _effect, int _targetArg){
        effect = _effect;
        targetArg = _targetArg;
    }
    public EffectInstruction clone(){
        EffectInstruction toReturn = new EffectInstruction();
        if(effect != null){
            toReturn.effect = effect.clone();
        }
        toReturn.targetArg = targetArg;
        return toReturn;
    }
}