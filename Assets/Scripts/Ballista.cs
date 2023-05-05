using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//DEALS WITH THE PHYSICS AND FUNCTIONALITY OF THE TOWER
public class Ballista : MonoBehaviour
{
    public float damageOutput;
    public float velocity;
    public float rotationSpeed;
    public float shootingRate = 1.0f;
    public string towerType;
    public GameObject body;
    public GameObject ammostart;
    public GameObject ammo;
    public GameObject rotatePoint;
    public List<GameObject> enemyList = new List<GameObject>();
    public AudioSource ballistaAudio;
    public AudioClip hitSound;
    public Ballista previousTower;

    /*
     * Start() first uses the arrow already in the tower model. Shoot() is then called
     * to generate new instances of the arrow prefab.
     */
    void Start()
    {   
        if (towerType == "Cannon")
        {
            damageOutput = previousTower.damageOutput + 500;
            shootingRate = previousTower.shootingRate - 0.25f;
        }
        body = transform.GetChild(0).gameObject;
        ammostart = body.transform.GetChild(0).gameObject;
        rotatePoint = transform.Find("rotatePoint").gameObject;

        //START COROUTINE TO SHOOT ARROWS
        StartCoroutine(Shoot());
    }

    //THE ENEMY IS THE TRIGGER
    void OnTriggerEnter(Collider other)
    {
        //when enemy enters the shooting radius, add this enemy to enemyList
        GameObject enemy = other.gameObject;
        if (enemy.CompareTag("Enemy") && !enemyList.Contains(enemy))
        {
            enemyList.Add(enemy);
        }
    }

    void OnTriggerExit(Collider other)
    {
        //when enemy leaves the shooting radius, remove this enemy to enemyList
        GameObject enemy = other.gameObject;
        if (enemy.CompareTag("Enemy") && enemyList.Contains(enemy))
        {
            enemyList.Remove(enemy);
        }
    }

    IEnumerator Shoot()
    {
        /* 
         * coroutine that waits indefinitely -- waits for enemy to enter radius.
         */

        WaitUntil untilEnemyEnters = new WaitUntil(() => enemyList.Count > 0);

        while (true)
        {
            //clean out any enemies that have been killed
            enemyList.RemoveAll(e => e == null);

            yield return untilEnemyEnters;

            //choose the closest enemy to shoot at

            Vector3 minEnemyPosition = enemyList[0].transform.position;
            Vector3 currEnemyPosition;
            Quaternion newRotation;
            float minDistance = float.PositiveInfinity;
            float enemyDistance;

            for (int e = 0; e < enemyList.Count; e++)
            {
                //check if any enemies are dead (marked as non-active)
                GameObject currEnemy = enemyList[e];

                currEnemyPosition = enemyList[e].transform.position;
                enemyDistance = Vector3.Distance(transform.position, currEnemyPosition);
                if (enemyDistance < minDistance)
                {
                    minDistance = enemyDistance;
                    minEnemyPosition = currEnemyPosition;
                }
            }

            //aim arrow at the middle of the enemy

            minEnemyPosition = new Vector3(minEnemyPosition[0], minEnemyPosition[1] + 3, minEnemyPosition[2]);

            /* 
             * rotate TOWER in direction of the closest enemy!
             * ! first rotate "rotatePoint" which will have all the euler angles !
             * 
             * -  tower rotates in solely y-direction
             * -  bow and arrow rotate in all directions
             * 
             * */

            newRotation = Quaternion.LookRotation(minEnemyPosition);
            rotatePoint.transform.LookAt(minEnemyPosition, transform.up);
            Vector3 angles = rotatePoint.transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0, angles[1], 0);

            if (towerType != "Catapult"){
                if (towerType == "Ballista"){
                    body.transform.rotation = Quaternion.Euler(angles[0], angles[1], angles[2]);
                    //ballistaAudio.PlayOneShot(hitSound, 0.6f);
                } else {
                    body.transform.rotation = Quaternion.Euler(angles[0] + 90f, angles[1], angles[2]);
                }
            } else {
                body.transform.rotation = Quaternion.Slerp(body.transform.rotation, Quaternion.Euler(60f, 0 ,0), Time.deltaTime);
            }

            //make new arrow

            GameObject createdammo = Instantiate(ammo, ammostart.transform.position, body.transform.rotation);
            createdammo.GetComponent<Projectile>().settings("towerWeapon", enemyList[0].transform.gameObject.tag, damageOutput, velocity, rotationSpeed, enemyList[0].transform);
            if (towerType == "Catapult"){
                body.transform.rotation = Quaternion.Slerp(body.transform.rotation, Quaternion.Euler(-60f, 0 ,0), Time.deltaTime);
            }  

            yield return new WaitForSeconds(shootingRate);
        }
    }

    IEnumerator RemoveArrow(GameObject arr)
    {
        //wait until the arrow has hit an enemy (and is destroyed) OR the arrow has left the radius of the tower
        WaitUntil arrowDestroy = new WaitUntil(() => arr == null ||
            arr.transform.position[1] < 5);

        yield return arrowDestroy;

        //if the arrow hasn't been destroyed already, destroy it :)
        if(arr != null)
        { 
            Destroy(arr);
        }
        
    }
}
