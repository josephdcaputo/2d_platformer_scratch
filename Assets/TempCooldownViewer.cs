using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TempCooldownViewer : MonoBehaviour
{
    // Use Uimanager to reference the player
    // interate over player's cooldowns
    // display them in the textmeshpro.SetText
    public TextMeshProUGUI tmp;
    bool acFound = false;

    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        string toDisplay = "";
        
        if(acFound){
            
            foreach(AbilityCooldown ac in UIManager.playerActor.abilityCooldowns){
                //Debug.Log("Displaying cooldowns");
                toDisplay = toDisplay + ac.abilityName + "| " + ((int)ac.remainingTime + 1).ToString() + "\n";
                tmp.SetText(toDisplay);
            }
        }
        else{
            tmp.SetText(toDisplay);
        }
    }
    IEnumerator tryGetAbilityCooldowns(){ // To fix null ref exception 
        bool stay = true;
        while(stay){
            if(UIManager.playerActor.abilityCooldowns != null){
                stay = false;
                acFound = true;
                
            }
            yield return new WaitForSeconds(0.1f);
        }
        
    }
}
