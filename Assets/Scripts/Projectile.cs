using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damageOutput;
    public float velocity;
    public float rotationSpeed;
    public Transform target;

    Vector3 targetoffset;

    public string targetTag;

    bool activate = false;
    
    void Update(){
        if (target != null && activate){
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((target.position + targetoffset) - transform.position), rotationSpeed * Time.deltaTime);
            float amtToMove = velocity * Time.deltaTime;
            transform.Translate(Vector3.forward * amtToMove);
            if (!target.gameObject.activeSelf){
                Destroy(gameObject);
            }
        } else if (activate && target == null){
            Destroy(gameObject);
        }
    }
    public void settings(string inputThisTag, string inputTargetTag, float inputDamage, float inputVelocity, float inputRotation, Transform inputTarget){
        if (inputTargetTag == "Enemy"){
            targetoffset = new Vector3(0, 3, 0);
        } else {
            targetoffset = new Vector3(0, 0, 0);
        }
        gameObject.tag = inputThisTag;
        targetTag = inputTargetTag;
        damageOutput = inputDamage;
        velocity = inputVelocity;
        rotationSpeed = inputRotation;
        target = inputTarget;
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

    // 0.685 0.685 13.8

}
