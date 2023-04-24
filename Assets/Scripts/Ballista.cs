using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//DEALS WITH THE PHYSICS AND FUNCTIONALITY OF THE TOWER
public class Ballista : MonoBehaviour
{

    float velocity = 15f;
    float destroyRate = 6.5f;
    public int damageOutput = 20;
    public float shootingRate = 2.0f;
    public GameObject arrow;
    public GameObject bow;
    public GameObject newArrow;
    public GameObject rotatePoint;
    public List<GameObject> enemyList = new List<GameObject>();

    /*
     * Start() first uses the arrow already in the tower model. Shoot() is then called
     * to generate new instances of the arrow prefab.
     */
    void Start()
    {
        bow = transform.GetChild(0).gameObject;
        arrow = bow.transform.GetChild(0).gameObject;
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

            //bow rotation doesn't work with solely assigning rotatePoint.transform.rotation
            bow.transform.rotation = Quaternion.Euler(angles[0], angles[1], angles[2]);

            //make new arrow
    

            GameObject arr2 = Instantiate(newArrow, arrow.transform.position, bow.transform.rotation);
            arr2.GetComponent<Projectile>().damageOutput = 50;
            arr2.GetComponent<Projectile>().target = enemyList[0].transform;
            arr2.GetComponent<Projectile>().settings("Enemy", "weapon");
            //Physics.IgnoreCollision(arr2.GetComponent<Collider>(), GetComponent<Collider>());

            //change velocity of arrow

            //Vector3 vel = minEnemyPosition - transform.position;
            //Vector3 direction = vel.normalized;
            //Rigidbody rbArrow = arr2.GetComponent<Rigidbody>();

            //rbArrow.velocity = direction * velocity;
           
            //StartCoroutine(RemoveArrow(arr2));

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
