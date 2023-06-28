using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MobSpawner: MonoBehaviour
{
    // public GameObject mobPrefab;
    public List<GameObject> mobPrefabList;
    public bool spawnOnStart = true;
    void Start(){
        //StartCoroutine(tempSpawnTestMob(mobPrefab));
        if(NetworkServer.active && spawnOnStart){
            Debug.Log("spawning mobs on start");
            spawnMobs();

        }
    }
	IEnumerator tempSpawnTestMob(GameObject prefab){
        while(true){
            yield return new WaitForSeconds(15.0f);
            NetworkServer.Spawn(Instantiate(prefab, transform.position, Quaternion.identity));


        }
    }
    public void spawnMobs(){
            
        for (int i = 0; i < mobPrefabList.Count; i++)
        {
            GameObject goRef = Instantiate(mobPrefabList[i], transform.position, Quaternion.identity);
            goRef.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f,0.01f));
            goRef.AddComponent<GeneralMobPack>();
            NetworkServer.Spawn(goRef);
            //goRef.GetComponent<Controller>().moveOffOtherUnits();
        }
    }
}