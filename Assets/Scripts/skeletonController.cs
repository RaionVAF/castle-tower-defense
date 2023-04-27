using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeletonController : MonoBehaviour
{
    // Public member for death particles
    public GameObject deathParticleEffects;
    public GameObject wood;
    public GameObject stone;
    public GameObject iron;

    // Zombie model rigidbody and joint references
    private Rigidbody  skeletonRB;
    private GameObject skeletonModel, leftArmJoint, rightArmJoint, leftLegJoint, rightLegJoint, 
                       leftElbowJoint, rightElbowJoint, source;
    // Target placeholder (CHANGE)
    targets targetScript;
    private Transform target;
    public GameObject newArrow;
    public UnityEngine.AI.NavMeshAgent skeleton;
    // Speed constants
    float movementSpeed = 3f;
    float skeletonRotationSpeed = 2f;
    float jointRotationSpeed = 5f;
    float legRotationAngle = 15f;
    float attackingRange = 10f;

    public float health = 50;

    int attackInterval = 2;

    // Bool member to run moving animation script if true
    bool isMoving = false;
    bool finishedRotating = false;
    bool finishedMoving = false;
    bool appliedForce = false;
    public bool armsAreRaised = false;

 

    // PLACEHOLDER BOOLEAN TO TEST DEATH ANIMATION
    bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize GameObjects that will be used for animating

        // Edit: access local gameobject instead of gameobject.find
        skeleton.stoppingDistance = 20f;

        skeletonRB = GetComponent<Rigidbody>();
        skeletonModel = transform.gameObject;
        leftArmJoint = transform.GetChild(4).gameObject;
        rightArmJoint = transform.GetChild(5).gameObject;
        leftLegJoint = transform.GetChild(6).gameObject;
        rightLegJoint = transform.GetChild(7).gameObject;

     

        source = GameObject.Find("Targets"); 
        target = closestTarget();
        targetScript = target.GetComponent<targets>();
        skeleton.destination = target.position;

        StartCoroutine(animate());
        StartCoroutine(attack());
    }
    void Update()
    {
        if (!target.gameObject.active){
            target = closestTarget();
            skeleton.destination = target.position;
            targetScript = target.GetComponent<targets>();
            leftArmJoint.transform.localRotation = Quaternion.Euler(0, 0, 0);
            rightArmJoint.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        if (health <= 0){
            SpawnMaterial();
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
   void OnTriggerEnter(Collider other)
    {
        //when enemy enters the shooting radius, add this enemy to enemyList
        GameObject enemy = other.gameObject;
        if (enemy.tag == "weapon")
        {
            GameObject particles = Instantiate(deathParticleEffects, skeletonModel.transform.localPosition, deathParticleEffects.transform.localRotation);
            Destroy(particles);
            health -= enemy.GetComponent<Projectile>().damageOutput;
        }
    }


    IEnumerator animate()
    {
        // Create a WaitUntil object that will wait until isMoving is true
        WaitUntil isMoving = new WaitUntil(() => Vector3.Distance(transform.position, target.position) >= skeleton.stoppingDistance);

        while (true)
        {
            yield return isMoving; 
                Quaternion lowerRotation = Quaternion.Euler(0, 0, 0);
            if (armsAreRaised) {
                leftArmJoint.transform.localRotation = Quaternion.Euler(0, 0, 0);
                rightArmJoint.transform.localRotation = Quaternion.Euler(0, 0, 0);

                armsAreRaised = false;

            }
            float legRotation = Mathf.Sin(Time.time * jointRotationSpeed) * legRotationAngle;
            leftLegJoint.transform.localRotation = Quaternion.AngleAxis(-legRotation, Vector3.left);
            rightLegJoint.transform.localRotation = Quaternion.AngleAxis(legRotation, Vector3.left);
        }
    }

    IEnumerator attack()
    {
        WaitUntil inRange = new WaitUntil(() => Vector3.Distance(transform.position, target.position) <= skeleton.stoppingDistance);

        while (true){
            yield return inRange;

            if (Vector3.Distance(transform.position, target.position) <= skeleton.stoppingDistance){
                resetMovementJoints();
                if (skeleton.isOnNavMesh){
                    skeleton.ResetPath();
                }
                transform.LookAt(target);  
            }
            
            if (!armsAreRaised)
            {
                // Call animation to raise arms up
                leftArmJoint.transform.localRotation = Quaternion.Euler(-90f, 0, 0);
                rightArmJoint.transform.localRotation = Quaternion.Euler(-90f, 0, 45f);

                armsAreRaised = true;
            }          
            GameObject arr = Instantiate(newArrow, leftArmJoint.transform.position, leftArmJoint.transform.localRotation);
            arr.GetComponent<Projectile>().damageOutput = 50;
            arr.GetComponent<Projectile>().target = target;
            arr.GetComponent<Projectile>().settings("Tower", "skeletonweapon");
            arr.transform.localScale = new Vector3(.25f, .25f, .125f);
            yield return new WaitForSeconds(attackInterval);
        }
            
    }

    // Helper that determines if the skeleton is finished rotation
    private static bool isFinishedRotating(Quaternion skeletonDirection, Quaternion targetDirection, float precision)
    {
        // Quaternion.Dot method returns a value between 1 and -1.
        // 1 or -1 means that the 2 quaternions are "exact", 0 means that they are far from close
        return Mathf.Abs(Quaternion.Dot(skeletonDirection, targetDirection)) >= 1 - precision;
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
        
        if (randomInt <= 40) Instantiate(wood, skeletonModel.transform.localPosition + new Vector3(0f, 3f, 0f), wood.transform.localRotation);
        
        if (randomInt >= 41 && randomInt <= 80) Instantiate(stone, skeletonModel.transform.localPosition + new Vector3(0f, 3f, 0f), stone.transform.localRotation);

        if (randomInt >= 81) Instantiate(iron, skeletonModel.transform.localPosition + new Vector3(0f, 3f, 0f), iron.transform.localRotation);
    }
}
