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
    private GameObject skeletonModel, leftArmJoint, rightArmJoint, leftLegJoint, rightLegJoint, 
                       leftElbowJoint, rightElbowJoint, source;
    // Target placeholder (CHANGE)
    boundary targetScript;
    private Transform target;
    private Vector3 targetVector;
    public GameObject Arrow;
    public UnityEngine.AI.NavMeshAgent skeleton;
    // Speed constants

    float jointRotationSpeed = 5f;
    float legRotationAngle = 15f;

    public float health = 50;
    public float damageOutput = 100;

    int attackInterval = 2;

    // Bool member to run moving animation script if true
    public bool armsAreRaised = false;

    // Audio
    public AudioSource audioSource, externalSource, deathSource;
    public AudioClip skeletonHit;
    public AudioClip skeletonDeath;
    public AudioClip shoot;

    // Resources
    public GameObject particles;
    private GameObject materialManager;
    private materialTracker materials;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize GameObjects that will be used for animating

        // Edit: access local gameobject instead of gameobject.find
        skeleton.stoppingDistance = 25f;
        skeleton.avoidancePriority = Random.Range(0,99);
        health *= spawning.bossStage;
        damageOutput *= spawning.bossStage;

        skeletonModel = transform.gameObject;
        leftArmJoint = transform.GetChild(4).gameObject;
        rightArmJoint = transform.GetChild(5).gameObject;
        leftLegJoint = transform.GetChild(6).gameObject;
        rightLegJoint = transform.GetChild(7).gameObject;

        source = GameObject.Find("Targets"); 
        target = closestTarget();
        targetVector = new Vector3(target.position.x, 0f, target.position.z);
        targetScript = target.GetComponent<boundary>();
        skeleton.destination = targetVector;

        audioSource = GetComponent<AudioSource>();
        externalSource = GameObject.Find("backgroundAudio").gameObject.GetComponent<AudioSource>();
        deathSource = GameObject.Find("mobAudio").gameObject.GetComponent<AudioSource>();

        materialManager = GameObject.Find("Material Manager"); 
        materials = materialManager.GetComponent<materialTracker>();

        StartCoroutine(animate());
        StartCoroutine(attack());
    }
    void Update()
    {
        if (!target.gameObject.activeInHierarchy){
            target = closestTarget();
            targetVector = new Vector3(target.position.x, 0f, target.position.z);
            skeleton.destination = targetVector;
            targetScript = target.GetComponent<boundary>();
            leftArmJoint.transform.localRotation = Quaternion.Euler(0, 0, 0);
            rightArmJoint.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        if (health <= 0){
            SpawnMaterial();
            externalSource.PlayOneShot(skeletonHit, 0.8f);
            externalSource.PlayOneShot(skeletonDeath, 0.8f);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
   void OnTriggerEnter(Collider other)
    {
        //when enemy enters the shooting radius, add this enemy to enemyList
        GameObject enemy = other.gameObject;
        if (enemy.tag == "towerWeapon")
        {
            GameObject particles = Instantiate(deathParticleEffects, skeletonModel.transform.localPosition, deathParticleEffects.transform.localRotation);
            Destroy(particles);
            health -= enemy.GetComponent<Projectile>().damageOutput;
            audioSource.PlayOneShot(skeletonHit, 0.8f);
        }
    }


    IEnumerator animate()
    {
        // Create a WaitUntil object that will wait until isMoving is true
        WaitUntil isMoving = new WaitUntil(() => Vector3.Distance(transform.position, targetVector) >= skeleton.stoppingDistance);

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
        WaitUntil inRange = new WaitUntil(() => Vector3.Distance(transform.position, targetVector) <= skeleton.stoppingDistance);

        while (true){
            yield return inRange;

            if (skeleton.isOnNavMesh && Vector3.Distance(transform.position, targetVector) <= skeleton.stoppingDistance){
                resetMovementJoints();
                skeleton.ResetPath();
                transform.LookAt(targetVector);
            }
            
            if (!armsAreRaised)
            {
                // Call animation to raise arms up
                leftArmJoint.transform.localRotation = Quaternion.Euler(-90f, 0, 0);
                rightArmJoint.transform.localRotation = Quaternion.Euler(-90f, 0, 45f);

                armsAreRaised = true;
            }          
            GameObject createdammo = Instantiate(Arrow, leftArmJoint.transform.position, leftArmJoint.transform.rotation);
            createdammo.GetComponent<Projectile>().settings("enemyWeapon", target.tag, damageOutput, 80f, 240f, target);
            createdammo.transform.localScale = new Vector3(.25f, .25f, .125f);
            audioSource.PlayOneShot(shoot, 1f);
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

    private void SpawnXP(){
        GameObject p = Instantiate(particles, skeletonModel.transform.localPosition, particles.transform.localRotation);

        materials.changeXP(10);
        
        Destroy(p, 1);
    } 

    private void SpawnMaterial(){
        int randomInt = Random.Range(1, 101);
        
        if (randomInt <= 40) Instantiate(wood, skeletonModel.transform.localPosition + new Vector3(0f, 3f, 0f), wood.transform.localRotation);
        
        if (randomInt >= 41 && randomInt <= 80) Instantiate(stone, skeletonModel.transform.localPosition + new Vector3(0f, 3f, 0f), stone.transform.localRotation);

        if (randomInt >= 81) Instantiate(iron, skeletonModel.transform.localPosition + new Vector3(0f, 3f, 0f), iron.transform.localRotation);
    }
}
