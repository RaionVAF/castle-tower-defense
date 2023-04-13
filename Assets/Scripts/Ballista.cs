using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballista : MonoBehaviour
{

    float velocity = 10f;
    float radius = 60f;
    float shootingRate = 2.0f;
    float destroyRate = 6.5f;
    public GameObject arrow;
    public GameObject newArrow;
    Vector3 arrowPosition = new Vector3(0, 0, 0);
    Quaternion arrowRotation;

    /*
     * Start() uses the arrow already in the tower model. Shoot() is then called
     * to generate new instances of the arrow prefab.
     */
    void Start()
    {
        GameObject bow = transform.GetChild(0).gameObject;
        arrow = bow.transform.GetChild(0).gameObject;
        //add rigid body to arrow
        arrow.AddComponent<Rigidbody>();
        arrowPosition = arrowPosition + arrow.transform.position;
        arrowRotation = arrow.transform.rotation;

        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.useGravity = false;
        //arrow moves in local z axis!
        rb.velocity = transform.TransformDirection(new Vector3(0, 0, velocity));
        StartCoroutine(RemoveArrow(arrow));
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        /* 
         * coroutine will run indefinitely -- this will be changed to a while
           loop that automatically runs when it is NIGHT.
         */
        while (true)
        {
            GameObject arr2 = Instantiate(newArrow, arrowPosition, arrowRotation);
            arr2.AddComponent<Rigidbody>();
            Rigidbody rb2 = arr2.GetComponent<Rigidbody>();
            rb2.useGravity = false;
            rb2.velocity = transform.TransformDirection(new Vector3(0, 0, velocity));
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
