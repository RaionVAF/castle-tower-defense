using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieController : MonoBehaviour
{
    // Public member for death particles
    public GameObject deathParticleEffects;

    // Zombie model rigidbody and joint references
    private Rigidbody zombieRB;
    private GameObject zombieModel, leftArmJoint, rightArmJoint, leftLegJoint, rightLegJoint;
    // Target placeholder (CHANGE)
    private GameObject target;

    // Speed constants
    float movementSpeed = 1f;
    float zombieRotationSpeed = 1.5f;
    float jointRotationSpeed = 5f;
    float legRotationAngle = 15f;
    float attackingRange = 2f;

    // Bool member to run moving animation script if true
    bool isMoving = false;
    bool finishedRotating = false;
    bool finishedMoving = false;
    bool armsAreRaised = false;
    bool appliedForce = false;

    // PLACEHOLDER BOOLEAN TO TEST DEATH ANIMATION
    bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize GameObjects that will be used for animating
        zombieRB = GetComponent<Rigidbody>();
        zombieModel = GameObject.Find("zombie");
        leftArmJoint = GameObject.Find("zombie/Left Arm Joint");
        rightArmJoint = GameObject.Find("zombie/Right Arm Joint");
        leftLegJoint = GameObject.Find("zombie/Left Leg Joint");
        rightLegJoint = GameObject.Find("zombie/Right Leg Joint");
        // Get target placeholder 
        target = GameObject.Find("blacksmith");
        StartCoroutine(animate());
    }

    // Update is called once per frame
    void Update()
    {
        // Keep rotating zombie towards target after spawning until the zombie is looking
        // at the target. If the rotation is finished, start moving zombie towards target
        if (!finishedRotating)
        {
            rotateZombieTowardsTarget(target.transform.localPosition);
        } 
        else if (finishedRotating && !finishedMoving)
        {
            moveZombieTowardsTarget(target.transform.localPosition);
        }

        // DELETE LATER (this section tests killing the zombie)
        if (isDead)
        {
            killZombie();
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

                // Call attacking animation
                attackingAnimation();

                // Wait and run again 
                yield return null;
            }
        }
    }

    // Helper that animates zombie dying and destroys object
    private void killZombie()
    {
        // Make zombie fall over then scale him down (negate RB constraint so it can fall)
        zombieRB.constraints &= ~RigidbodyConstraints.FreezePositionX;
        zombieRB.constraints &= ~RigidbodyConstraints.FreezePositionZ;
        Vector3 force = new Vector3(0f, 0f, -65f);
        if (!appliedForce)
        {
            zombieRB.AddForce(force * 2f);
            appliedForce = true;
        }
        zombieModel.transform.localScale = Vector3.Lerp(zombieModel.transform.localScale, Vector3.zero, Time.deltaTime * 0.5f);

        // Destroy zombie after dying animation finishes
        if (zombieModel.transform.localScale.x < 0.15f)
        {
            // Instantiate death particles
            Instantiate(deathParticleEffects, zombieModel.transform.localPosition, deathParticleEffects.transform.localRotation);
            Destroy(deathParticleEffects, 1);
            // (!) Could replace this line with resource object 
            Destroy(zombieModel);
        }
    }

    // Helper that moves zombie towards a fixed location
    private void moveZombieTowardsTarget(Vector3 location)
    {
        // Toggle isMoving
        isMoving = true;

        // Calculate direction of target (direction = provided position - zombie's position)
        Vector3 movement = (location - zombieModel.transform.localPosition).normalized * movementSpeed;
        // Move zombie towards the target by translating it using movement vector
        zombieModel.transform.localPosition += movement * Time.deltaTime;

        // Check if the zombie is within a fixed range (2f) of the target to signal zombie to stop moving
        if (Vector3.Distance(zombieModel.transform.position, target.transform.position) < attackingRange)
        {   
            finishedMoving = true;
            // Toggle isMoving off to stop movement animating coroutine
            isMoving = false;
        }
    }

    // Helper that rotates zombie towards a fixed location
    private void rotateZombieTowardsTarget(Vector3 location)
    {
        // Calculate direction of target (direction = provided position - zombie's position)
        Quaternion direction 
            = Quaternion.LookRotation(location - zombieModel.transform.localPosition);
        // Get the direction that the zombie should look at using RotateTowards
        zombieModel.transform.localRotation
            = Quaternion.Slerp(zombieModel.transform.localRotation, direction, Time.deltaTime * zombieRotationSpeed);

        // Check rotations to determine if zombie should stop rotating
        if (isFinishedRotating(zombieModel.transform.localRotation, direction, 0.001f))
        {   
            finishedRotating = true;
        }
    }

    // Helper that animates zombie moving towards target
    private void movingAnimation()
    {
        // Legs should rotate max 15 degrees to emulate a slow moving zombie
        float legRotation = Mathf.Sin(Time.time * jointRotationSpeed) * legRotationAngle;
        leftLegJoint.transform.localRotation = Quaternion.AngleAxis(-legRotation, Vector3.left);
        rightLegJoint.transform.localRotation = Quaternion.AngleAxis(legRotation, Vector3.left);
    }

    // Helper that animates zombie attacking target
    private void attackingAnimation()
    {
        // Store values
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
            }
        }
    }

    // Helper that determines if the zombie is finished rotation
    private static bool isFinishedRotating(Quaternion zombieDirection, Quaternion targetDirection, float precision)
    {
        // Quaternion.Dot method returns a value between 1 and -1.
        // 1 or -1 means that the 2 quaternions are "exact", 0 means that they are far from close
        return Mathf.Abs(Quaternion.Dot(zombieDirection, targetDirection)) >= 1 - precision;
    }

    // Helper that resets joint angles to Euler(0,0,0)
    private void resetMovementJoints()
    {
        leftLegJoint.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);
        rightLegJoint.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);
    }
}
