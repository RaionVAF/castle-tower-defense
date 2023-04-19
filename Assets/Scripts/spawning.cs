using System.Collections;
using System.Collections.Generic;
using System;
using Random=UnityEngine.Random;
using UnityEngine;

public class spawning : MonoBehaviour
{
    private int mobcap, plat;
    private float timer;

    // time between increase of mobs
    static float leveltimer = 15f;
    private GameObject[] spawnplats;
    public List<GameObject> enemyList = new List<GameObject>();
    
    // needs prefabs 
    public GameObject Zombie;

    // needs prefabs 
    public GameObject Skeleton;

    private GameObject newmob;

    // Start is called before the first frame update
    void Start()
    {
        spawnplats = GameObject.FindGameObjectsWithTag("spawnplat");

        mobcap = 1;

        newmob = Instantiate(Zombie, spawnplats[0].transform.position, spawnplats[0].transform.rotation);
        enemyList.Add(newmob);
        
        StartCoroutine(spawn());
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > leveltimer){
            mobcap +=1;
            timer = 0;
        }
        
        // temp hard cap on mobs
        mobcap = Math.Clamp(mobcap, 1, 5);
    }

    IEnumerator spawn(){

    WaitUntil lessthancap = new WaitUntil(() => enemyList.Count < mobcap);

    while (true){

        yield return lessthancap;

        plat = UnityEngine.Random.Range(0,6);
        if (mobcap < 5){
            // early game always spawns zombies
            newmob = Instantiate(Zombie, spawnplats[plat].transform.position, spawnplats[plat].transform.rotation);
        } else {
            //probability for mobs to be spawned
            int i = Random.Range(0,100);
            if (i < 80){
                newmob = Instantiate(Zombie, spawnplats[plat].transform.position, spawnplats[plat].transform.rotation);
            } else if (i > 80){
                newmob = Instantiate(Skeleton, spawnplats[plat].transform.position, spawnplats[plat].transform.rotation);
            }
        }
        enemyList.Add(newmob);
        Debug.Log("enemy spawned");
    }
    }
   
}
