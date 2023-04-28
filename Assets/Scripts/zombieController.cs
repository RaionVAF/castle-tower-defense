using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;
using UnityEngine.AI;

using Random = UnityEngine.Random;

public class zombieController : MonoBehaviour
{
    private Rigidbody zombieRB;
    private GameObject zombieModel, leftArmJoint, rightArmJoint, leftLegJoint, rightLegJoint, source;
    public GameObject deathParticleEffects;
    public GameObject wood;
    public GameObject stone;
    public GameObject iron;
    public NavMeshAgent zombie;

    targets targetScript;

    float movementSpeed = 1f;
    float zombieRotationSpeed = 1.5f;
    float jointRotationSpeed = 5f;
    float legRotationAngle = 15f;
    float attackingRange = 2f;

    private Transform target;

    float attackdamage = 20;

    public float health = 100;

    // Bool member to run moving animation script if true
    bool isMoving = false;
    bool finishedRotating = false;
    bool finishedMoving = false;
    bool armsAreRaised = false;
    bool appliedForce = false;

    // Start is called before the first frame update
    void Start()
    {
        zombie.stoppingDistance = 10f;

        zombieRB = GetComponent<Rigidbody>();
        zombieModel = transform.gameObject;
        leftArmJoint = transform.GetChild(2).gameObject;
        rightArmJoint = transform.GetChild(3).gameObject;
        leftLegJoint = transform.GetChild(4).gameObject;
        rightLegJoint = transform.GetChild(5).gameObject;

        source = GameObject.Find("Targets"); 
        target = closestTarget();
        targetScript = target.GetComponent<targets>();
        zombie.destination = target.position;

        StartCoroutine(animate());
        StartCoroutine(attack());
    }

    // Update is called once per frame
    void Update()
    {
        if (!target.gameObject.active){
            target = closestTarget();
            zombie.destination = target.position;
            targetScript = target.GetComponent<targets>();
            leftArmJoint.transform.localRotation = Quaternion.Euler(-90f, 0, 0);
            rightArmJoint.transform.localRotation = Quaternion.Euler(-90f, 0, 0);
        }

        if (health <= 0){
            SpawnMaterial();
            Destroy(gameObject);
        }


    }

    void OnTriggerEnter(Collider other)
    {
        //when enemy enters the shooting radius, add this enemy to enemyList
        GameObject enemy = other.gameObject;
        if (enemy.tag == "weapon")
        {
            GameObject particles = Instantiate(deathParticleEffects, zombieModel.transform.localPosition, deathParticleEffects.transform.localRotation);
            Destroy(particles);
            health -= enemy.GetComponent<Projectile>().damageOutput;
        }
    }


    IEnumerator animate()
    {
        // Create a WaitUntil object that will wait until isMoving is true
        WaitUntil isMoving = new WaitUntil(() => Vector3.Distance(transform.position, target.position) >= zombie.stoppingDistance);

        while (true)
        {
            yield return isMoving; 

            //stand.enabled = false;
        float legRotation = Mathf.Sin(Time.time * jointRotationSpeed) * legRotationAngle;
        leftLegJoint.transform.localRotation = Quaternion.AngleAxis(-legRotation, Vector3.left);
        rightLegJoint.transform.localRotation = Quaternion.AngleAxis(legRotation, Vector3.left);
        }
    }

    IEnumerator attack()
    {
        WaitUntil inRange = new WaitUntil(() => Vector3.Distance(transform.position, target.position) <= zombie.stoppingDistance);

        while (true){
            yield return inRange;

            if (Vector3.Distance(transform.position, target.position) <= zombie.stoppingDistance){
                resetMovementJoints();
                if (zombie.isOnNavMesh){
                    zombie.ResetPath();
                }
                transform.LookAt(target);
            }
           

            Quaternion leftArmJointStartRotation = leftArmJoint.transform.localRotation;
            Quaternion rightArmJointStartRotation = rightArmJoint.transform.localRotation;
            Quaternion raisingRotation = Quaternion.Euler(-160f, 0, 0);
            Quaternion swingingRotation = Quaternion.Euler(-60f, 0, 0);

            if (!armsAreRaised)
            {
                // Call animation to raise arms up
                leftArmJoint.transform.localRotation
                    = Quaternion.Slerp(leftArmJointStartRotation, raisingRotation, Time.deltaTime);
                rightArmJoint.transform.localRotation
                    = Quaternion.Slerp(rightArmJointStartRotation, raisingRotation, Time.deltaTime);

                if (isFinishedRotating(leftArmJoint.transform.localRotation, raisingRotation, 0.01f))
                {
                    armsAreRaised = true;
                }
            }
            else
            {
                // After previous animation ends, call animation to swing arms down
                leftArmJoint.transform.localRotation
                    = Quaternion.Slerp(leftArmJointStartRotation, swingingRotation, Time.deltaTime * 4f);
                rightArmJoint.transform.localRotation
                    = Quaternion.Slerp(rightArmJointStartRotation, swingingRotation, Time.deltaTime * 4f);

                if (isFinishedRotating(leftArmJoint.transform.localRotation, swingingRotation, 0.01f))
                {
                    armsAreRaised = false;
                    targetScript.hit(attackdamage);
                }
            }
        }
        
    }

    private static bool isFinishedRotating(Quaternion zombieDirection, Quaternion targetDirection, float precision)
    {
        // Quaternion.Dot method returns a value between 1 and -1.
        // 1 or -1 means that the 2 quaternions are "exact", 0 means that they are far from close
        return Mathf.Abs(Quaternion.Dot(zombieDirection, targetDirection)) >= 1 - precision;
    }

    private void resetMovementJoints()
    {
        leftLegJoint.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);
        rightLegJoint.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);
    }

    private Transform closestTarget(){
        Transform closest = null;
        float minDist = Mathf.Infinity;
        foreach (Transform t in source.transform)
        {
            if (!t.gameObject.active){
                continue;
            }
            float dist = Vector3.Distance(t.position, transform.position);
            if (dist < minDist)
            {
                closest= t;
                minDist = dist;
            }
        }
        return closest;
    }   

    private void SpawnMaterial(){
        int randomInt = Random.Range(1, 101);
        
        if (randomInt <= 20) Instantiate(wood, zombieModel.transform.localPosition + new Vector3(0f, 3f, 0f), wood.transform.localRotation);
        
        if (randomInt >= 21 && randomInt <= 25) Instantiate(stone, zombieModel.transform.localPosition + new Vector3(0f, 3f, 0f), stone.transform.localRotation);

        if (randomInt == 100) Instantiate(iron, zombieModel.transform.localPosition + new Vector3(0f, 3f, 0f), iron.transform.localRotation);
    }
}

