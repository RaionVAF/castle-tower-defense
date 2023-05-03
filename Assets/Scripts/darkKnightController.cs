using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class darkKnightController : MonoBehaviour
{
    // Public member for death particles
    public GameObject deathParticleEffects;

    // Zombie model rigidbody and joint references
    private GameObject darkKnightModel, leftArmJoint, rightArmJoint, leftLegJoint, rightLegJoint,
                       source;
    public GameObject wood;
    public GameObject stone;
    public GameObject iron;
    public UnityEngine.AI.NavMeshAgent darkKnight;

    boundary targetScript;

    // Constants
    float jointRotationSpeed = 5f;
    float legRotationAngle = 15f;


    // Bool member to run moving animation script if true
    private Transform target;
    private Vector3 targetVector;

    float attackdamage = 50;

    public float health = 300;

    // Bool member to run moving animation script if true
    bool armsAreRaised = false;

    // Start is called before the first frame update
    void Start()
    {
        darkKnight.stoppingDistance = 16f;
        darkKnight.avoidancePriority = Random.Range(0,99);
        // Get joints

        darkKnightModel = transform.gameObject;
        leftArmJoint = transform.GetChild(2).gameObject;
        rightArmJoint = transform.GetChild(3).gameObject;
        leftLegJoint = transform.GetChild(4).gameObject;
        rightLegJoint = transform.GetChild(5).gameObject;

        source = GameObject.Find("Targets"); 
        target = closestTarget();
        targetVector = new Vector3(target.position.x, 0, target.position.z) ;
        targetScript = target.GetComponent<boundary>();
        darkKnight.destination = targetVector;

        StartCoroutine(animate());
        StartCoroutine(attack());
    }

    // Update is called once per frame
    void Update()
    {
        
         if (!target.gameObject.activeInHierarchy){
            target = closestTarget();
            targetVector = new Vector3(target.position.x, 0, target.position.z) ;
            darkKnight.destination = targetVector;
            targetScript = target.GetComponent<boundary>();
            leftArmJoint.transform.localRotation = Quaternion.Euler(0, 0, 0);
            rightArmJoint.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        if (health <= 0){
            //SpawnMaterial();
            Destroy(gameObject);
        }      
        
        float leftArmRotation = Mathf.Sin(Time.time * 1f) * -10f;
        leftArmJoint.transform.localRotation = Quaternion.AngleAxis(leftArmRotation, Vector3.left);
    }

    // Helper that animates knight attacking target
    void OnTriggerEnter(Collider other)
    {
        //when enemy enters the shooting radius, add this enemy to enemyList
        GameObject enemy = other.gameObject;
        if (enemy.tag == "towerWeapon")
        {
            //GameObject particles = Instantiate(deathParticleEffects, zombieModel.transform.localPosition, deathParticleEffects.transform.localRotation);
            //Destroy(particles);
            health -= enemy.GetComponent<Projectile>().damageOutput;
        }
    }
    
    IEnumerator animate()
    {
        // Create a WaitUntil object that will wait until isMoving is true
        WaitUntil isMoving = new WaitUntil(() => Vector3.Distance(transform.position, targetVector) >= darkKnight.stoppingDistance);

        while (true)
        {
            yield return isMoving; 

            float legRotation = Mathf.Sin(Time.time * jointRotationSpeed) * legRotationAngle;
            leftLegJoint.transform.localRotation = Quaternion.AngleAxis(-legRotation, Vector3.left);
            rightLegJoint.transform.localRotation = Quaternion.AngleAxis(legRotation, Vector3.left);
        }
    }

    IEnumerator attack()
    {
        WaitUntil inRange = new WaitUntil(() => Vector3.Distance(transform.position, targetVector) <= darkKnight.stoppingDistance);

        while (true){
            yield return inRange;
            
            if (darkKnight.isOnNavMesh && Vector3.Distance(transform.position, targetVector) <= darkKnight.stoppingDistance){
                resetMovementJoints();
                darkKnight.ResetPath();
                // Negates y location so model does not tilt when looking at target
                transform.LookAt(targetVector);
            }
           

            Quaternion leftArmJointStartRotation = leftArmJoint.transform.localRotation;
            Quaternion rightArmJointStartRotation = rightArmJoint.transform.localRotation;
            Quaternion raisingRotation = Quaternion.Euler(-150f, 0, 0);
            Quaternion swingingRotation = Quaternion.Euler(10f, 0, 0);

            if (!armsAreRaised)
            {
                // Call animation to raise arms up
                rightArmJoint.transform.localRotation
                    = Quaternion.Slerp(rightArmJointStartRotation, raisingRotation, Time.deltaTime);

                if (isFinishedRotating(rightArmJoint.transform.localRotation, raisingRotation, 0.01f))
                {
                    armsAreRaised = true;
                }
            }
            else
            {
                // After previous animation ends, call animation to swing arms down
                rightArmJoint.transform.localRotation
                    = Quaternion.Slerp(rightArmJointStartRotation, swingingRotation, Time.deltaTime * 6f);

                if (isFinishedRotating(rightArmJoint.transform.localRotation, swingingRotation, 0.01f))
                {
                    armsAreRaised = false;
                    targetScript.hit(attackdamage);
                }
            }
        }
        
    }
    // Helper that determines if the rotation is finished
    private static bool isFinishedRotating(Quaternion darkKnightDirection, Quaternion targetDirection, float precision)
    {
        // Quaternion.Dot method returns a value between 1 and -1.
        // 1 or -1 means that the 2 quaternions are "exact", 0 means that they are far from close
        return Mathf.Abs(Quaternion.Dot(darkKnightDirection, targetDirection)) >= 1 - precision;
    }

    // Helper that resets joint angles to Euler(0,0,0)
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
            if (!t.gameObject.activeInHierarchy){
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
        
        if (randomInt <= 20) Instantiate(wood, darkKnightModel.transform.localPosition + new Vector3(0f, 3f, 0f), wood.transform.localRotation);
        
        if (randomInt >= 21 && randomInt <= 25) Instantiate(stone, darkKnightModel.transform.localPosition + new Vector3(0f, 3f, 0f), stone.transform.localRotation);

        if (randomInt == 100) Instantiate(iron, darkKnightModel.transform.localPosition + new Vector3(0f, 3f, 0f), iron.transform.localRotation);
    }
}
