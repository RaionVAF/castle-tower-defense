using System.Collections;
using System.Collections.Generic;
using Random=UnityEngine.Random;
using UnityEngine;

public class spawning : MonoBehaviour
{
    public int mobcap, plat;
    private float timer;
    private int bossStage;

    public int mobcapLimit = 5;

    // time between increase of mobs
    public float leveltimer = 3f;
    private GameObject[] spawnplats;
    public List<GameObject> enemyList = new List<GameObject>();
    
    public GameObject Zombie;

    public GameObject Skeleton;

    public GameObject darkKnight;
    public GameObject bossKnight;

    public bool bossPhase = false;

    private GameObject newmob, boss;
    void Start()
    {
        Debug.Log("active");

        spawnplats = GameObject.FindGameObjectsWithTag("spawnplat");

        mobcap = 0;
        
        StartCoroutine(spawn());
    }

    // Update is called once per frame
    void Update()
    {
        // mobcap inreases with time, basic difficulty scaler\
        enemyList.RemoveAll(e => e == null);
        timer += Time.deltaTime;
        if (!bossPhase && timer > leveltimer){
            mobcap +=1;
            timer = 0;
        }

        if (mobcap >= mobcapLimit){
            mobcap = 0;
            timer = 0;
            bossSpawn();
        }

        if (bossPhase && boss == null){
            if (bossStage == 3){
                //end game
            }
            mobcapLimit += 25;
            bossStage += 1;
            bossPhase = false;
        }
        
        
    }

    IEnumerator spawn(){

        WaitUntil lessthancap = new WaitUntil(() => enemyList.Count < mobcap);

        while (true){

            yield return lessthancap;
            int i = Random.Range(0,101);
            plat = UnityEngine.Random.Range(0,spawnplats.Length);
            if (mobcap < 20){
                // early game always spawns zombies
                newmob = Instantiate(Zombie, spawnplats[plat].transform.position, spawnplats[plat].transform.rotation);
            } else {         
                if (i < 70){
                    newmob = Instantiate(Zombie, spawnplats[plat].transform.position, spawnplats[plat].transform.rotation);
                } else if (i > 70 && i < 90){
                    newmob = Instantiate(Skeleton, spawnplats[plat].transform.position, spawnplats[plat].transform.rotation);
                } else {
                    newmob = Instantiate(darkKnight, spawnplats[plat].transform.position, spawnplats[plat].transform.rotation);
                }
            }
            enemyList.Add(newmob);
        }
    }

    private void bossSpawn(){   
        Debug.Log("big boi"); 
        boss = Instantiate(bossKnight, spawnplats[0].transform.position, spawnplats[0].transform.rotation);  
        boss.transform.localScale = new Vector3(3f,3f,3f);
        bossPhase = true;
    }
   
}
