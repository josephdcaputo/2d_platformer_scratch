using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDelivery : MonoBehaviour
{
    /*
        0 = Regular tracking delivery
        1 = Skill shot
        2 = AoE w/ time duration WP
        3 = AoE w/ duration target


    */
    public Actor caster;
    public Actor target;
    public Vector3 worldPointTarget;
    public List<TargetCooldown> aoeActorIgnore;

    [SerializeField]public List<AbilityEffect> abilityEffects;
    
    public int type; // 0 detroys when reaches target, 1 = skill shot
    public float speed;
    public float duration;
    public float tickRate = 1.5f; // an AoE type will hit you every tickRate secs
    public int aoeCap;
    public bool ignoreDuration;
    
    void Start()
    {   
        
        if(type == 2){ // aoe no target
            gameObject.transform.position = worldPointTarget;
        }
        if(type == 3){ // aoe no target
            gameObject.transform.position = target.transform.position;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {   
        if(type == 0){   
            if (other.gameObject.GetComponent<Actor>() == target){
                other.gameObject.GetComponent<Actor>().applyAbilityEffects(abilityEffects, caster);
                Destroy(gameObject);
            }
        }
        if(type == 1){
            if (other.gameObject.GetComponent<Actor>() != caster){
                if (other.gameObject.GetComponent<Actor>() != null){
                    other.gameObject.GetComponent<Actor>().applyAbilityEffects(abilityEffects, caster);
                }
                Destroy(gameObject);
            }
        }
        
    }
    private void OnTriggerStay2D(Collider2D other){
        if(type == 2){
            if (other.gameObject.GetComponent<Actor>() != caster){
                if (other.gameObject.GetComponent<Actor>() != null){
                    if(checkIgnoreTarget(other.gameObject.GetComponent<Actor>()) == false){
                        other.gameObject.GetComponent<Actor>().applyAbilityEffects(abilityEffects, caster);
                        addToAoeIgnore(other.gameObject.GetComponent<Actor>(), tickRate);

                    }
                    
                }
                // make version that has a set number for ticks?
            }
        }
    }
    void FixedUpdate()
    {
        if(type == 0){
            transform.position = Vector2.MoveTowards(transform.position, target.gameObject.transform.position, speed);
        }
        else if(type == 1){
            transform.position = Vector2.MoveTowards(transform.position, worldPointTarget, speed);
            if(transform.position == worldPointTarget){
                Destroy(gameObject);
            }
        }
    }
    void Update()
    {
        if(type == 2){
            updateTargetCooldowns();
            if(!ignoreDuration){
                duration -= Time.deltaTime;
            }
            if(duration <= 0){
                Debug.Log("Destroying AoE");        
                Destroy(gameObject);
            }
        }
    }

    /*public AbilityDelivery(ActiveAbilityEffect _abilityEffect, int _type, Actor _caster){
        abilityEffect = _abilityEffect;
        type = _type;
        caster = _caster;
    }*/
    public void init(List<AbilityEffect> _abilityEffects, Actor _target, int _type, Actor _caster = null, float _speed = 0.1f,
                        float _duration = 8.0f, float _tickRate = 1.5f, int _aoeCap = -1, bool _ignoreDuration = false){
        
        abilityEffects = _abilityEffects;
        type = _type;
        caster = _caster;
        target = _target;
        speed = _speed;
        duration = _duration;
        aoeCap = _aoeCap;
        ignoreDuration = _ignoreDuration;
        
    }
    public void init(List<AbilityEffect> _abilityEffects, Vector3 _worldPointTarget, int _type,  Actor _caster = null, float _speed = 0.1f,
                     float _duration = 8.0f, float _tickRate = 1.5f, int _aoeCap = -1, bool _ignoreDuration = false){
        
        abilityEffects = _abilityEffects;
        type = _type;
        caster = _caster;
        worldPointTarget = _worldPointTarget;
        speed = _speed;
        duration = _duration;
        aoeCap = _aoeCap;
        ignoreDuration = _ignoreDuration;
    }
    
    
    Vector3 getWorldPointTarget(){
        Vector3 scrnPos = Input.mousePosition;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(scrnPos);
        worldPoint.z = 0.0f;
        return worldPoint;

    }
    public bool checkIgnoreTarget(Actor _target){
        if(aoeActorIgnore.Count > 0){
            for(int i = 0; i < aoeActorIgnore.Count; i++){
                if(aoeActorIgnore[i].actor == _target){
                    //Debug.Log(aoeActorIgnore[i].actor.actorName +"At [" + i.ToString() + "] is on cooldown!");
                    return true;
                }
            }
            return false;
        }
        else{
            return false;
        }
    }
    void addToAoeIgnore(Actor _target, float _remainingtime){
        aoeActorIgnore.Add(new TargetCooldown(_target, _remainingtime));
    }
    void updateTargetCooldowns(){
        if(aoeActorIgnore.Count > 0){
            for(int i = 0; i < aoeActorIgnore.Count; i++){
                if(aoeActorIgnore[i].remainingTime > 0)
                    aoeActorIgnore[i].remainingTime -= Time.deltaTime;
                else
                    aoeActorIgnore.RemoveAt(i);
            }
        }
    }
}
