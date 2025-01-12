using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public class AbilitySystemGeneratorWindow : EditorWindow
{
   private string abilityName = "";
   private string abilityEffectName = "";
   int effectSelection;
   string[] abilityEffectTypes = new string[]{nameof(SchoolDamage), nameof(Missile),
                                 nameof(Aoe), nameof(RingAoe), nameof(ApplyBuff), nameof(Dizzy),
                                 nameof(Heal), nameof(RestoreClassResource), nameof(Interrupt), nameof(Fear)};

   bool createWithAbility = false;
   bool addToDatabases = false;
    [MenuItem("Ability/Ability System Tools")]
    public static void ShowWindow()
    {
        GetWindow(typeof(AbilitySystemGeneratorWindow));
    }

    private void OnGUI()
    {
         GUILayout.Label("Ability System Tools", EditorStyles.boldLabel);
         abilityEffectName = EditorGUILayout.TextField("Effect Name", abilityEffectName);
         abilityName = EditorGUILayout.TextField("Ability Name", abilityName);
         
        
        effectSelection = EditorGUILayout.Popup(effectSelection, abilityEffectTypes);
        addToDatabases = EditorGUILayout.Toggle("Add To Databases", addToDatabases);
        if (GUILayout.Button("Build Effect")){
         // abilityEffectName = EditorGUILayout.TextField("Effect Name", abilityEffectName);
            GenerateSOs();
            
        }
        if (GUILayout.Button("Generate Ability With Effect")){
            createWithAbility = true;
            GenerateSOs();
        }
        
        EditorGUILayout.HelpBox(
         "\n> ~~~~~ ~~~~~WARNING!~~~~~ ~~~~~ <\n\n This does not yet add created Abilities or effects into AbilityDatabase or AbilityEffectDatabase!",
                      MessageType.Warning); 
    }

    private void GenerateSOs()
    {
        if (!Directory.Exists($"Assets/Scripts/Ability/AbilityEff/" + abilityEffectName + ".asset")){
            AbilityEff aeRef = null;
            //{"Magic Damage", "Missile", "AoE", "Ring Aoe", "Apply Buff", "Dizzy", "Heal"}
            switch(effectSelection){
               case(0):
                  aeRef = CreateInstance(typeof(SchoolDamage)) as AbilityEff;
                  
                  break;
               case(1):
                  aeRef = CreateInstance(typeof(Missile)) as AbilityEff;
                  
                  break;
               case(2):
                  aeRef = CreateInstance(typeof(Aoe)) as AbilityEff;
                  
                  break;
               case(3):
                  aeRef = CreateInstance(typeof(RingAoe)) as AbilityEff;
                  
                  break;
               case(4):
                  aeRef = CreateInstance(typeof(ApplyBuff)) as AbilityEff;
                  
                  break;
               case(5):
                  aeRef = CreateInstance(typeof(Dizzy)) as AbilityEff;
                  
                  break;
               case(6):
                  aeRef = CreateInstance(typeof(Heal)) as AbilityEff;
                  
                  break;
               case(7):
                  aeRef = CreateInstance(typeof(RestoreClassResource)) as AbilityEff;

                  break;
               case(8):
                  aeRef = CreateInstance(typeof(Interrupt)) as AbilityEff;

                  break;
               case(9):
                  aeRef = CreateInstance(typeof(Fear)) as AbilityEff;

                  break;   
               default:
                  Debug.LogError("Unknown selection");
                  break;
            }
            if((abilityEffectName == "" )||(abilityEffectName == null)){
               Debug.Log(abilityEffectName.ToString());
               abilityEffectName = aeRef.GetType().ToString() + "Effect";
            }
            aeRef.effectName = abilityEffectName;
            string aePath = "Assets/Scripts/Ability/AbilityEff/GeneratorOut/" + abilityEffectName + ".asset";
            AssetDatabase.CreateAsset(aeRef, aePath);
            
            Debug.Log(aeRef.GetType().ToString() + "| " + aePath);
            if(addToDatabases){
               addEffectToDatabase(aeRef);
            }
            // Directory.CreateDirectory($"Assets/{abilityName}");
            if(createWithAbility){
               Ability_V2 abilityRef;
               abilityRef = CreateInstance(typeof(Ability_V2)) as Ability_V2;
               abilityRef.setName(abilityName);
               List<EffectInstruction> temp = new List<EffectInstruction>();
               temp.Add(new EffectInstruction(aeRef, 0));
               abilityRef.setEffectInstructions(temp);
               
               string abilityPath =  "Assets/Scripts/Ability/GeneratorOut/" + abilityName + ".asset";
               AssetDatabase.CreateAsset(abilityRef, abilityPath);
               Debug.Log(abilityRef.GetType().ToString() + "| " + abilityPath);
               if(addToDatabases){
                  addAbilityToDatabase(abilityRef);
               }
               

               createWithAbility = false;
               temp = null;

            }
        }
        else{
            Debug.LogError("Ability already exists with that name");
        }
            
      	void addEffectToDatabase(AbilityEff _effect){
            AbilityEffectData.instance.effectsList.Add(_effect);
         }
         void addAbilityToDatabase(Ability_V2 _ability){
            AbilityData.instance.abilityList.Add(_ability);
         }
      //   foreach (GameObject go in Selection.gameObjects)
      //   {
      //       string localPath = AssetDatabase.GenerateUniqueAssetPath($"Assets/{pathName}{go.name}.prefab");
      //       PrefabUtility.SaveAsPrefabAssetAndConnect(go, localPath, InteractionMode.UserAction);
      //   }    
    }
}
