﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/*
     Container many for any RPG related elements
*/

public class Actor : MonoBehaviour
{
    public bool showDebug = false;
    [SerializeField]protected string actorName;
    [SerializeField]protected int health; // 0
    [SerializeField]protected int maxHealth; // 1
    [SerializeField]protected int mana; // 2
    [SerializeField]protected int maxMana; // 3
    public Actor target;
    
    public Color unitColor;
    [SerializeField]protected List<AbilityEffect> abilityEffects;
    
    // When readyToFire is true queuedAbility will fire
    public bool readyToFire = false; // Will True by CastBar for abilities w/ casts. Will only be true for a freme
    public bool isCasting = false; // Will be set False by CastBar 
    [SerializeField]protected Ability queuedAbility; // Used when Ability has a cast time
    [SerializeField]protected Actor queuedTarget; // Used when Ability has a cast time
    [SerializeField]protected Vector3? queuedTargetWP;
    [SerializeField]protected List<AbilityCooldown> abilityCooldowns = new List<AbilityCooldown>();
    public UIManager uiManager;
    public GameObject abilityDeliveryPrefab;



    void Start(){
        abilityEffects = new List<AbilityEffect>();
    }
    void Update(){
        updateCooldowns();
        handleAbilityEffects();
        handleCastQueue();
    }
    //------------------------------------------------------------handling Active Ability Effects-------------------------------------------------------------------------
    
    void handleAbilityEffects(){

        if(abilityEffects.Count > 0){

            for(int i = 0; i < abilityEffects.Count; i++){
                abilityEffects[i].update();
                checkAbilityEffectToRemoveAtPos(abilityEffects[i], i);
            }
        //Debug.Log(actorName + " cleared all Ability effects!");
        }
    }
    public void damageValue(int amount, int valueType = 0){
        // Right now this only damages health, but, maybe in the future,
        // This could take an extra param to indicate a different value to "damage"
        // For ex. a Ability that reduces maxHealth or destroys mana

        //Debug.Log("damageValue: " + amount.ToString()+ " on " + actorName);
        if(amount >= 0){
            switch (valueType){
                case 0:
                    health -= amount;
                    if(health < 0){
                        health = 0;
                    }
                    break;
                case 1:
                    maxHealth -= amount;
                    if(maxHealth < 1){      // Making this 0 might cause a divide by 0 error. Check later
                        maxHealth = 1;
                    }
                    break;
                case 2:
                    mana -= amount;
                    if(mana < 0){
                        mana = 0;
                    }
                    break;
                case 3:
                    maxMana -= amount;
                    if(maxMana < 1){     // Making this 0 might cause a divide by 0 error. Check later
                        maxMana = 1;
                    }
                    break;
                default:
                    break;
            }
        }
        else{
            restoreValue(-1 * amount, valueType); //if negative call restore instead with amount's sign flipped
        }
    }

    public void restoreValue(int amount, int valueType = 0){
        // This would be the opposite of damageValue(). Look at that for more details
        //  Maybe in the future calcing healing may have diff formula to calcing damage taken
        
        //Debug.Log("restoreValue: " + amount.ToString()+ " on " + actorName);
        if(amount >= 0){
            switch (valueType){
                case 0:
                    health += amount;
                    if(health > maxHealth){
                        health = maxHealth;
                    }
                    break;
                case 1:
                    if(maxHealth + amount > maxHealth){ // if int did not wrap around max int
                        maxHealth += amount;    
                    }
                    break;
                case 2:
                    mana += amount;
                    if(mana > maxMana){
                        mana = maxMana;
                    }
                    break;
                case 3:
                    if(maxHealth + amount > maxHealth){ // if int did not wrap around max int
                        maxHealth += amount;    
                    }
                    break;
                default:
                    break;
            }
        }
        else{
            damageValue( -1 * amount, valueType); // if negative call damage instead with amount's sign flipped
        }
    }

    void checkAbilityEffectToRemoveAtPos(AbilityEffect _abilityEffect, int listPos){
        // Remove AbilityEffect is it's duration is <= 0.0f

        if(_abilityEffect.getRemainingTime() <= 0.0f){
            if(showDebug)
            Debug.Log(actorName + ": Removing.. "+ _abilityEffect.getEffectName());
            abilityEffects[listPos].OnEffectFinish(abilityEffects[listPos].getCaster(), this); // AE has a caster and target now so the args could be null?
            abilityEffects.RemoveAt(listPos);
        }
    }
    public void applyAbilityEffect(AbilityEffect _abilityEffect, Actor inCaster){

        //Adding AbilityEffect it to this actor's list<AbilityEffect>
        _abilityEffect.setCaster(inCaster);
        _abilityEffect.setTarget(this);
        _abilityEffect.setRemainingTime(_abilityEffect.getDuration());
        //Debug.Log(_abilityEffect.getRemainingTime().ToString() + " " + _abilityEffect.getDuration().ToString());
        _abilityEffect.setStart(true);
        abilityEffects.Add(_abilityEffect);

        _abilityEffect.OnEffectStart(inCaster, this);// AE has a caster and target now so the args could be null?
        //Debug.Log("Actor: Applying.." + _abilityEffect.getEffectName() + " to " + actorName);  

    }
    public void applyAbilityEffects(List<AbilityEffect> _abilityEffects, Actor inCaster){
        if(_abilityEffects.Count > 0){
            for(int i = 0; i < _abilityEffects.Count; i++ ){
                applyAbilityEffect(_abilityEffects[i], inCaster);
            }
        } 

    }

    //-------------------------------------------------------------------handling casts--------------------------------------------------------------------------

    public void castAbility(Ability _ability, Actor _target = null, Vector3? _targetWP = null){
        //Main way to make Acotr cast an ability

        if(checkOnCooldown(_ability) == false){
            if(!readyToFire){
                if(checkAbilityReqs(_ability, _target, _targetWP)){
                    if(_ability.getCastTime() > 0.0f){
                        if(!isCasting){
                            queueAbility(_ability, _target, _targetWP);
                            prepCast();
                        }
                    }
                    else{
                        fireCast(_ability, _target, _targetWP);
                    }
                }
                else{
                    if(showDebug)
                        Debug.Log("Ability doesn't have necessary requirments");
                    resetQueue();
                }                   
            }
            else{
                if(showDebug)
                Debug.Log("Something else is ready to fire and blocking this cast");
            }
        }
    }
    public void castAbility(Ability _ability, Vector3 _queuedTargetWP){
        //Main way to make Acotr cast an ability

        if(checkOnCooldown(_ability) == false){
            if(!readyToFire){
                Vector3? tempNullibleVect = _queuedTargetWP;
                if(checkAbilityReqs(_ability, null, tempNullibleVect)){
                    if(_ability.getCastTime() > 0.0f){
                        if(!isCasting){
                            queueAbility(_ability, null, tempNullibleVect);
                            prepCast();
                        }
                    }
                    else{
                        fireCast(_ability, null, tempNullibleVect);
                    }
                }
                else{
                    if(showDebug)
                        Debug.Log("Ability doesn't have necessary requirments: A, WP");
                    resetQueue();
                }                       
            }
            else{
                if(showDebug)
                Debug.Log("Something else is ready to fire and blocking this cast: A, WP");
            }
        }
    }
    public void freeCast(Ability _ability, Actor _target = null, Vector3? _targetWP = null){
        //  Make Acotor cast an ability without starting a cooldown or (in the future) cost resources
        // Maybe make this into an overload of castAbility later

        if(checkAbilityReqs(_ability, _target, _targetWP)){
            if(handleDelivery(_ability, _target)){
                // No cd gfenerated
                // Make not cost resources
                readyToFire = false;
            }
            
        }
        else{
            if(showDebug)
            Debug.Log(actorName + " Free cast failed reqs");
        }

    }
    public void freeCast(Ability _ability, Vector3 _targetWP){
        //  Make Acotor cast an ability without starting a cooldown or (in the future) cost resources
        // Maybe make this into an overload of castAbility later

        if(checkAbilityReqs(_ability, null, _targetWP)){
            if(handleDelivery(_ability, null, _targetWP)){
                // No cd gfenerated
                // Make not cost resources
                readyToFire = false;
            }
            
        }
        else{
            if(showDebug)
            Debug.Log(actorName + " Free cast failed reqs: A, WP");
        }

    }
    void forceCastAbility(Ability _ability, Actor _target){
        
        /*
                                                                  ***IGNORE*** Unused for now
        */
        
        if(_target != null){
            //Debug.Log("A: " + actorName + " casting " + _ability.getName() + " on " + target.actorName);
            _target.applyAbilityEffects(_ability.createEffects(), this);
            addToCooldowns(queuedAbility);
        }
        else{
            if(showDebug)
            Debug.Log("Actor: " + actorName + " has no target!");
        }

    }

    public void fireCast(Ability _ability, Actor _target = null, Vector3? _targetWP = null){
        // Main way for "Fireing" a cast by creating a delivery if needed then creating an AbilityCooldown
        if(handleDelivery(_ability, _target, _targetWP)){
            addToCooldowns(queuedAbility);
            readyToFire = false;
        }

    }  
    void queueAbility(Ability _ability, Actor _queuedTarget = null, Vector3? _queuedTargetWP = null){
        //Preparing variables for a cast
        queuedAbility = _ability;
        queuedTarget = _queuedTarget;
        queuedTargetWP = _queuedTargetWP;
    }
    void prepCast(){
        //Creates castbar for abilities with cast times

        //Debug.Log("Trying to create a castBar for " + _ability.getName())
            
        isCasting = true;   

        // Creating CastBar or CastBarNPC with apropriate variables   
        InitCastBarFromQueue();  
    }
    void InitCastBarFromQueue(){
        //Makes a castbar 

        if( (queuedAbility.NeedsTargetActor()) && (queuedAbility.NeedsTargetWP()) ){
            Debug.Log("Spell that needs an Actor and WP are not yet suported");
        }
        else if(queuedAbility.NeedsTargetActor()){
            initCastBarWithActor();
        }
        else if(queuedAbility.NeedsTargetWP()){
            initCastBarWithWP();
        }
        else{
            initCastBarWithActor();
        }
    }
    void initCastBarWithActor(){
        // Creates a CastBar with target being an Actor
        if(gameObject.tag == "Player"){ // For player
                //Creating cast bar and setting it's parent to canvas to display it properly

                GameObject newAbilityCast = Instantiate(uiManager.castBarPrefab, uiManager.canvas.transform);
                //                                   v (string cast_name, Actor from_caster, Actor to_target, float cast_time) v
                newAbilityCast.GetComponent<CastBar>().Init(queuedAbility.getName(), this, queuedTarget, queuedAbility.getCastTime());
        }
        else{// For NPCs
            if(showDebug)
            Debug.Log(actorName + " starting cast: " + queuedAbility.getName());
            gameObject.AddComponent<CastBarNPC>().Init(queuedAbility.getName(), this, queuedTarget, queuedAbility.getCastTime());
        }
    }
    void initCastBarWithWP(){
        //   Creates Castbar with target being a world point Vector3

        if(gameObject.tag == "Player"){ // For player
                //Creating cast bar and setting it's parent to canvas to display it properly

                GameObject newAbilityCast = Instantiate(uiManager.castBarPrefab, uiManager.canvas.transform);

                //                                   v (string cast_name, Actor from_caster, Actor to_target, float cast_time) v
                newAbilityCast.GetComponent<CastBar>().Init(queuedAbility.getName(), this, queuedTargetWP.Value, queuedAbility.getCastTime());
        }
        else{// For NPCs
            if(showDebug)
            Debug.Log(actorName + " starting cast: " + queuedAbility.getName());

            gameObject.AddComponent<CastBarNPC>().Init(queuedAbility.getName(), this, queuedTargetWP.Value, queuedAbility.getCastTime());
        }
    }
    bool handleDelivery(Ability _ability, Actor _target = null, Vector3? _targetWP = null){
        // Creates delivery if needed. Applies effects to target if not
        // ***** WILL RETURN FALSE if DeliveryType is -1 (auto apply to target) and there is no target *****


        //List<AbilityEffect> tempListAE_Ref = cloneListAE(_ability.getEffects());
        List<AbilityEffect> tempListAE_Ref = _ability.createEffects();

        if(_ability.getDeliveryType() == -1){
            if(_target != null){
                _target.applyAbilityEffects(tempListAE_Ref, this);
                return true;
            }
            else{
                Debug.Log(actorName + ": Direct Actor to Actor Delivery with no target");
                return false;
            }
        }
        else{
            GameObject delivery = CreateAndInitDelivery(tempListAE_Ref, _ability.getDeliveryType(), _target, _targetWP);
            return true;
        }
    }
    GameObject CreateAndInitDelivery(List<AbilityEffect> _abilityEffects, int _deliveryType, Actor _target = null, Vector3? _targetWP = null){
        // Creates and returns delivery

        GameObject delivery;

        if( (queuedAbility.NeedsTargetActor()) && (queuedAbility.NeedsTargetWP()) ){
            Debug.Log("Spell With Actor and WP reqs not yet suported");
            delivery = null;
        }
        else if(queuedAbility.NeedsTargetActor()){
            delivery = Instantiate(abilityDeliveryPrefab, gameObject.transform.position, gameObject.transform.rotation);
            delivery.GetComponent<AbilityDelivery>().init( _abilityEffects, _deliveryType, this, _target, 0.1f);
            return delivery;
        }
        else if(queuedAbility.NeedsTargetWP()){

            delivery = Instantiate(abilityDeliveryPrefab, gameObject.transform.position, gameObject.transform.rotation);
            delivery.GetComponent<AbilityDelivery>().init( _abilityEffects, _deliveryType, this, _targetWP.Value, 0.1f);
        }
        else{
            delivery = Instantiate(abilityDeliveryPrefab, gameObject.transform.position, gameObject.transform.rotation);
            delivery.GetComponent<AbilityDelivery>().init( _abilityEffects, _deliveryType, this, _target, 0.1f);
        }
        return delivery;
    }
    /*List<AbilityEffect> cloneListAE(List<AbilityEffect> _abilityEffects){
        List<AbilityEffect> tempListAE_Ref = new List<AbilityEffect>();
        if(_abilityEffects.Count > 0){
            for(int i = 0; i < _abilityEffects.Count; i++ ){
                AbilityEffect tempAE_Ref = _abilityEffects[i].clone();
                 //          
                 //    vV__Pretend below power is being modified by Actor's stats__Vv
                 //
                tempAE_Ref.setEffectName(tempAE_Ref.getEffectName() + " ("+ actorName + ")");
                tempListAE_Ref.Add(tempAE_Ref);
            }
        }
        return tempListAE_Ref;
    }*/
    void handleCastQueue(){
        // Called every Update() to see if queued spell is ready to fire

        if(readyToFire){
            //Debug.Log("castCompleted: " + queuedAbility.getName());
            if((queuedAbility.NeedsTargetActor()) && (queuedAbility.NeedsTargetWP())){
                Debug.Log("Cast that requires Actor and WP not yet supported. clearing queue.");
                resetQueue();
            }
            else if(queuedAbility.NeedsTargetWP()){
                fireCast(queuedAbility, null, queuedTargetWP);
            }
            else{
                fireCast(queuedAbility, queuedTarget);
            }
        }
    }
    bool checkAbilityReqs(Ability _ability, Actor _target = null, Vector3? _targetWP = null){
        // Checks if the requirments of _ability are satisfied

        //Debug.Log(_ability.NeedsTargetActor().ToString() + " " + _ability.NeedsTargetWP().ToString());
        if( (_ability.NeedsTargetActor()) && (_ability.NeedsTargetWP()) ){
            return ( (_target != null) && (_targetWP != null) );
        }
        else if(_ability.NeedsTargetActor()){
            return _target != null;
        }
        else if(_ability.NeedsTargetWP()){
            return _targetWP != null;
        }
        else{
            return true;
        }
    }
    void updateCooldowns(){
        if(abilityCooldowns.Count > 0){
            for(int i = 0; i < abilityCooldowns.Count; i++){
                if(abilityCooldowns[i].remainingTime > 0)
                    abilityCooldowns[i].remainingTime -= Time.deltaTime;
                else
                    abilityCooldowns.RemoveAt(i);
            }
        }
    }
    void addToCooldowns(Ability _ability){
        abilityCooldowns.Add(new AbilityCooldown(queuedAbility));
    }
    public bool checkOnCooldown(Ability _ability){
        if(abilityCooldowns.Count > 0){
            for(int i = 0; i < abilityCooldowns.Count; i++){
                if(abilityCooldowns[i].getName() == _ability.getName()){
                    if(showDebug)
                        Debug.Log(queuedAbility.getName() + " is on cooldown!");
                    return true;
                }
            }
            return false;
        }
        else{
            return false;
        }
    }
    void resetQueue(){
        queuedTarget = null;
        queuedTargetWP = null;
    }
    //------------------------------------------------------------------Setters/ getters---------------------------------------------------------------------------------
    public Ability getQueuedAbility(){
        return queuedAbility;
    }
    public int getHealth(){
        return health;
    }
    public void setHealth(int _health){
        health = _health;
    }
    public int getMaxHealth(){
        return maxHealth;
    }
    public void setMaxHealth(int _maxHealth){
        maxHealth = _maxHealth;
    }
    public int getMana(){
        return mana;
    }
    public void setMana(int _mana){
        mana = _mana;
    }
    public int getMaxMana(){
        return mana;
    }
    public void setMaxMana(int _mana){
        mana = _mana;
    }
    public string getActorName(){
        return actorName;
    }
    public void setActorName(string _actorName){
        actorName = _actorName;
    }





    //-------------------------------------------------------------------other---------------------------------------------------------------------------------------------------------
    
}

