using System.Collections;
using System.Collections.Generic;
using Random=UnityEngine.Random;
using UnityEngine;

public class spawning : MonoBehaviour
{
    private int mobcap, plat;
    private float timer;

    // time between increase of mobs
    static float leveltimer = 2f;
    private GameObject[] spawnplats;
    public List<GameObject> enemyList = new List<GameObject>();
    
    public GameObject Zombie;

    public GameObject Skeleton;

    private GameObject newmob;

    // Start is called before the first frame update
    void Start()
    {
        spawnplats = GameObject.FindGameObjectsWithTag("spawnplat");

        mobcap = 0;
        
        StartCoroutine(spawn());
    }

    // Update is called once per frame
    void Update()
    {
        // mobcap inreases with time, basic difficulty scaler
        timer += Time.deltaTime;
        if (timer > leveltimer){
            mobcap +=1;
            timer = 0;
        }
        
        // temp hard cap on mobs
        mobcap = Mathf.Clamp(mobcap, 1, 25);
        enemyList.RemoveAll(e => e == null);
    }

    IEnumerator spawn(){

        WaitUntil lessthancap = new WaitUntil(() => enemyList.Count < mobcap);

        while (true){

            yield return lessthancap;

            plat = UnityEngine.Random.Range(0,spawnplats.Length);
            if (mobcap < 5){
                // early game always spawns zombies
                newmob = Instantiate(Zombie, spawnplats[plat].transform.position, spawnplats[plat].transform.rotation);
            } else {
                //probability for mobs to be spawned
                int i = Random.Range(0,101);
                if (i < 80){
                    newmob = Instantiate(Zombie, spawnplats[plat].transform.position, spawnplats[plat].transform.rotation);
                } else if (i > 80){
                    newmob = Instantiate(Skeleton, spawnplats[plat].transform.position, spawnplats[plat].transform.rotation);
                }
            }
            enemyList.Add(newmob);
        }
    }
   
}
