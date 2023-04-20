using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealth : MonoBehaviour
{
    public IntegerValue health;
    public int currHealth;

    void Start()
    {
        currHealth = health.InitValue;
        Debug.Log(currHealth);
    }

    //ONTRIGGERENTER IS ACTIVATED WHEN
    //  -both objects have colliders
    //  -one has collider.istrigger enabled and contains a rigidbody

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("weapon"))
        {
            currHealth -= other.gameObject.GetComponent<Projectile>().damageOutput;
            Debug.Log("player health: " + currHealth);
            if (currHealth <= 0)
            {
                /* 
                 * destroys current enemy when health reaches zero
                 */

                Debug.Log("current object died");
                Destroy(gameObject);
            }
        }
    }

}
