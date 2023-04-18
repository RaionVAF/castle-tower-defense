using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballista : MonoBehaviour
{

    float velocity = 15f;
    float radius = 60f;
    float shootingRate = 2.0f;
    float destroyRate = 6.5f;
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
        //Debug.Log("before remove arrow");
        //Debug.Log("after remove arrow");
        StartCoroutine(Shoot());
    }

    void OnCollisionEnter(Collision col)
    {
        //when enemy enters the shooting radius, add this enemy to enemyList
        GameObject enemy = col.gameObject;
        Debug.Log("on collision enter entered");
        if (enemy.name.Contains("Knight") && !enemyList.Contains(enemy))
        {
            enemyList.Add(enemy);
            Debug.Log("enemy added! " + enemyList.Count + " " + enemy.name);
        }
    }

    void OnCollisionExit(Collision col)
    {
        //when enemy leaves the shooting radius, remove this enemy to enemyList
        GameObject enemy = col.gameObject;
        Debug.Log("on collision exit entered: " + enemy.name);
        if (enemy.name.Contains("Knight") && enemyList.Contains(enemy))
        {
            enemyList.Remove(enemy);
            Debug.Log("enemy removed!");
        }
    }

    IEnumerator Shoot()
    {
        /* 
         * coroutine that waits indefinitely -- waits for enemy to enter radius.
         */

        //Debug.Log("waiting for enemy entrance");

        //Debug.Log(enemyList.Count);

        WaitUntil untilEnemyEnters = new WaitUntil(() => enemyList.Count > 0);

        while (true)
        {
            yield return untilEnemyEnters;

            //choose the closest enemy to shoot at

            Vector3 minEnemyPosition = enemyList[0].transform.position;
            Vector3 currEnemyPosition;
            Quaternion newRotation;
            float minDistance = float.PositiveInfinity;
            float enemyDistance;

            for (int e = 0; e < enemyList.Count; e++)
            {
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

            //PROBLEM: TILTS IN THE X AND Z DIRECTIONS

            newRotation = Quaternion.LookRotation(minEnemyPosition);
            rotatePoint.transform.LookAt(minEnemyPosition, transform.up);

            Vector3 angles = rotatePoint.transform.rotation.eulerAngles;

            transform.rotation = Quaternion.Euler(0, angles[1], 0);

            //bow rotation doesn't work with solely assigning rotatePoint.transform.rotation
            bow.transform.rotation = Quaternion.Euler(angles[0], angles[1], angles[2]);

            //bow.transform.rotation = rotatePoint.transform.rotation;

            //make new arrow

            GameObject arr2 = Instantiate(newArrow, arrow.transform.position, bow.transform.rotation);
            Rigidbody rb2 = arr2.GetComponent<Rigidbody>();
            rb2.useGravity = false;

            //change velocity of arrow

            Vector3 vel = minEnemyPosition - transform.position;
            Vector3 direction = vel.normalized;

            rb2.velocity = direction * velocity;

            StartCoroutine(RemoveArrow(arr2));

            yield return new WaitForSeconds(shootingRate);
        }
    }

    IEnumerator RemoveArrow(GameObject arr)
    {
        yield return new WaitForSeconds(destroyRate);
        Destroy(arr);
    }

}
