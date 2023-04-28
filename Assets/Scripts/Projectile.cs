using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damageOutput;

    public float rotationSpeed;

    public float velocity;

    public Transform target;
 
    Vector3 targetoffset;

    public string targetTag;

    bool activate = false;
    
    void Update(){
        if (activate && target != null){
            if (!target.gameObject.activeSelf){
                Destroy(gameObject);
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((target.position + targetoffset) - transform.position), rotationSpeed * Time.deltaTime);
            float amtToMove = velocity * Time.deltaTime;
            transform.Translate(Vector3.forward * amtToMove);
        } else if (activate && target == null){
            Destroy(gameObject);
        }
    }

    public void settings(string inputtype, string inputTargetTag, float inputdamageOutput, float inputvelocity, float inputrotationSpeed, Transform inputtarget){
        gameObject.tag = inputtype;
        if (inputTargetTag == "Enemy"){
            targetoffset = new Vector3(0, 3, 0);
        } else {
            targetoffset = new Vector3(0, 0, 0);
        }
        targetTag = inputTargetTag;
        damageOutput = inputdamageOutput;
        velocity = inputvelocity;
        rotationSpeed = inputrotationSpeed;
        target = inputtarget;
        activate = true;
    }
    void OnTriggerEnter(Collider other)
    {
        GameObject enemy = other.gameObject;
        if (enemy.tag == targetTag)
        {
            Destroy(gameObject);
        }
    }
}
