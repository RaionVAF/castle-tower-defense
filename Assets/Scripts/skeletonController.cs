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
                       leftElbowJoint, rightElbowJoint;
    // Target placeholder (CHANGE)
    public GameObject target;

    // Speed constants
    float movementSpeed = 3f;
    float skeletonRotationSpeed = 2f;
    float jointRotationSpeed = 5f;
    float legRotationAngle = 15f;
    float attackingRange = 10f;

    // Bool member to run moving animation script if true
    bool isMoving = false;
    bool finishedRotating = false;
    bool finishedMoving = false;
    bool appliedForce = false;

    // PLACEHOLDER BOOLEAN TO TEST DEATH ANIMATION
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize GameObjects that will be used for animating
        skeletonRB = GetComponent<Rigidbody>();
        skeletonModel = GameObject.Find("skeleton");
        leftArmJoint = GameObject.Find("skeleton/Left Arm Joint");
        rightArmJoint = GameObject.Find("skeleton/Right Arm Joint");
        leftLegJoint = GameObject.Find("skeleton/Left Leg Joint");
        rightLegJoint = GameObject.Find("skeleton/Right Leg Joint");
        leftElbowJoint = GameObject.Find("skeleton/Left Arm Joint/Left Elbow Joint");
        rightElbowJoint = GameObject.Find("skeleton/Right Arm Joint/Right Elbow Joint");
        // Get target placeholder 
        // target = GameObject.Find("alchemist");
        StartCoroutine(animate());
    }

    // Update is called once per frame
    void Update()
    {
        // Keep rotating skeleton towards target after spawning until the skeleton is looking
        // at the target. If the rotation is finished, start moving skeleton towards target
        if (!finishedRotating)
        {
            rotateSkeletonTowardsTarget(target.transform.localPosition);
        }
        else if (finishedRotating && !finishedMoving)
        {
            Debug.Log("finished rotating");
            moveSkeletonTowardsTarget(target.transform.localPosition);
        }

        // DELETE LATER (this section tests killing the zombie)
        if (isDead)
        {
            killSkeleton();
        }
    }

    // Coroutine that animates movement when isMoving is true
    IEnumerator animate()
    {
        // Create a WaitUntil object that will wait until isMoving is true
        WaitUntil untilIsMoving = new WaitUntil(() => isMoving);

        while (true)
        {
            if (!finishedMoving)
            {
                // Only proceed to animation when isMoving is true
                yield return untilIsMoving;

                // Animate joint rotations for movement
                movingAnimation();

                // Wait and run again when isMoving is true
                yield return null;
            }
            else
            {
                // Reset joint rotations that were being used for movement
                resetMovementJoints();

                // Run animation that prepares skeleton to attack
                preparingAnimation();
                
                // Run attacking method/coroutine/etc

                // Wait and run again 
                yield return null;
            }
        }
    }

    // Helper that animates skeleton dying and destroys object
    private void killSkeleton()
    {
        // Make skeleton fall over then scale him down (negate RB constraint so it can fall)
        skeletonRB.constraints &= ~RigidbodyConstraints.FreezePositionX;
        skeletonRB.constraints &= ~RigidbodyConstraints.FreezePositionZ;
        Vector3 force = new Vector3(0f, 0f, -25f);
        if (!appliedForce)
        {
            skeletonRB.AddForce(force * 2f);
            appliedForce = true;
        }
        skeletonModel.transform.localScale = Vector3.Lerp(skeletonModel.transform.localScale, Vector3.zero, Time.deltaTime * 0.5f);

        // Destroy skeleton after dying animation finishes
        if (skeletonModel.transform.localScale.x < 0.15f)
        {
            // Instantiate death particles
            Instantiate(deathParticleEffects, skeletonModel.transform.localPosition, deathParticleEffects.transform.localRotation);
            SpawnMaterial();
            Destroy(deathParticleEffects, 1);
            // (!) Could replace this line with resource object 
            Destroy(skeletonModel);
        }
    }

    // Helper that moves skeleton towards a fixed location
    private void moveSkeletonTowardsTarget(Vector3 location)
    {
        // Toggle isMoving
        isMoving = true;

        // Calculate direction of target (direction = provided position - skeleton's position)
        Vector3 movement = (location - skeletonModel.transform.localPosition).normalized * movementSpeed;
        // Move skeleton towards the target by translating it using movement vector
        skeletonModel.transform.localPosition += movement * Time.deltaTime;

        // Check if the skeleton is within a fixed range (10f) of the target to signal skeleton to stop moving
        if (Vector3.Distance(skeletonModel.transform.position, target.transform.position) < attackingRange)
        {
            Debug.Log("finished moving");
            finishedMoving = true;
            // Toggle isMoving off to stop movement animating coroutine
            isMoving = false;
        }
    }

    // Helper that rotates skeleton towards a fixed location
    private void rotateSkeletonTowardsTarget(Vector3 location)
    {
        // Calculate direction of target (direction = provided position - skeleton's position)
        Quaternion direction
            = Quaternion.LookRotation(location - skeletonModel.transform.localPosition);
        // Get the direction that the skeleton should look at using RotateTowards
        skeletonModel.transform.localRotation
            = Quaternion.Slerp(skeletonModel.transform.localRotation, direction, Time.deltaTime * skeletonRotationSpeed);

        // Check rotations to determine if skeleton should stop rotating
        if (isFinishedRotating(skeletonModel.transform.localRotation, direction, 0.0001f))
        {
            finishedRotating = true;
        }
    }

    // Helper that animates skeleton moving towards target
    private void movingAnimation()
    {
        // Legs can rotate max 20 degress (based on model)
        float legRotation = Mathf.Sin(Time.time * jointRotationSpeed) * legRotationAngle;
        leftLegJoint.transform.localRotation = Quaternion.AngleAxis(-legRotation, Vector3.left);
        rightLegJoint.transform.localRotation = Quaternion.AngleAxis(legRotation, Vector3.left);
    }

    // Helper that animates skeleton's arms to prepare them to attack
    private void preparingAnimation()
    {
        // Store values
        Quaternion leftArmJointStartRotation = leftArmJoint.transform.localRotation;
        Quaternion rightArmJointStartRotation = rightArmJoint.transform.localRotation;
        Quaternion leftElbowJointStartRotation = leftElbowJoint.transform.localRotation;
        Quaternion rightElbowJointStartRotation = rightElbowJoint.transform.localRotation;

        // Call animation to raise arms up
        leftArmJoint.transform.localRotation
            = Quaternion.RotateTowards(leftArmJointStartRotation, Quaternion.Euler(-90, 0, -3), Time.deltaTime * 60f);
        rightArmJoint.transform.localRotation
            = Quaternion.RotateTowards(rightArmJointStartRotation, Quaternion.Euler(-80, 0, 3), Time.deltaTime * 60f);
        leftElbowJoint.transform.localRotation
            = Quaternion.RotateTowards(leftElbowJointStartRotation, Quaternion.Euler(0, 0, -10), Time.deltaTime * 60f);
        rightElbowJoint.transform.localRotation
            = Quaternion.RotateTowards(rightElbowJointStartRotation, Quaternion.Euler(0, 0, 50), Time.deltaTime * 60f);
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

    private void SpawnMaterial(){
        int randomInt = Random.Range(1, 101);
        
        if (randomInt <= 20) Instantiate(wood, skeletonModel.transform.localPosition + new Vector3(0f, 3f, 0f), wood.transform.localRotation);
        
        if (randomInt >= 21 && randomInt <= 25) Instantiate(stone, skeletonModel.transform.localPosition + new Vector3(0f, 3f, 0f), stone.transform.localRotation);

        if (randomInt == 100) Instantiate(iron, skeletonModel.transform.localPosition + new Vector3(0f, 3f, 0f), iron.transform.localRotation);
    }
}
