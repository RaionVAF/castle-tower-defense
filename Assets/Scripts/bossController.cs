using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossController : MonoBehaviour
{
    // Public member for death particles
    public GameObject deathParticleEffects;

    // Zombie model rigidbody and joint references
    private Rigidbody bossRB;
    private GameObject bossModel, leftArmJoint, rightArmJoint, leftLegJoint, rightLegJoint,
                       source;

    // Constants
    float movementSpeed = 1f;
    float bossRotationSpeed = 2f;
    float jointRotationSpeed = 5f;
    float legRotationAngle = 15f;
    float attackingRange = 10f;

    // Bool member to run moving animation script if true
    bool isMoving = false;
    bool finishedRotating = false;
    bool finishedMoving = false;
    bool appliedForce = false;
    public bool armsAreRaised = false;

    // Death boolean
    bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get joints
        bossRB = GetComponent<Rigidbody>();
        bossModel = GameObject.Find("boss");
        leftArmJoint = GameObject.Find("boss/Left Arm Joint");
        rightArmJoint = GameObject.Find("boss/Right Arm Joint");
        leftLegJoint = GameObject.Find("boss/Left Leg Joint");
        rightLegJoint = GameObject.Find("boss/Right Leg Joint");
    }

    // Update is called once per frame
    void Update()
    {
        // Make right arm move idly
        float rightArmRotation = Mathf.Sin(Time.time * 1f) * -10f;
        rightArmJoint.transform.localRotation = Quaternion.AngleAxis(rightArmRotation, Vector3.left);

        // Call attack animation
        attackingAnimation();
    }

    // Helper that animates boss attacking target
    private void attackingAnimation()
    {
        // Store values
        Quaternion leftArmJointStartRotation = leftArmJoint.transform.localRotation;
        Quaternion rightArmJointStartRotation = rightArmJoint.transform.localRotation;
        Quaternion raisingRotation = Quaternion.Euler(-110f, 0, 0);
        Quaternion swingingRotation = Quaternion.Euler(30f, 0, 0);

        if (!armsAreRaised)
        {
            // Call animation to raise arms up
            leftArmJoint.transform.localRotation
                = Quaternion.Slerp(leftArmJointStartRotation, raisingRotation, Time.deltaTime);

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

            if (isFinishedRotating(leftArmJoint.transform.localRotation, swingingRotation, 0.01f))
            {
                armsAreRaised = false;
            }
        }
    }

    // Helper that determines if the rotation is finished
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
}
