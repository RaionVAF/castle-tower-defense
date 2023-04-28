using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class darkKnightController : MonoBehaviour
{
    // Public member for death particles
    public GameObject deathParticleEffects;

    // Zombie model rigidbody and joint references
    private Rigidbody darkKnightRB;
    private GameObject darkKnightModel, leftArmJoint, rightArmJoint, leftLegJoint, rightLegJoint,
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
        darkKnightRB = GetComponent<Rigidbody>();
        darkKnightModel = GameObject.Find("darkKnight");
        leftArmJoint = GameObject.Find("darkKnight/Left Arm Joint");
        rightArmJoint = GameObject.Find("darkKnight/Right Arm Joint");
        leftLegJoint = GameObject.Find("darkKnight/Left Leg Joint");
        rightLegJoint = GameObject.Find("darkKnight/Right Leg Joint");
    }

    // Update is called once per frame
    void Update()
    {
        // Make right arm move idly
        float leftArmRotation = Mathf.Sin(Time.time * 1f) * -10f;
        leftArmJoint.transform.localRotation = Quaternion.AngleAxis(leftArmRotation, Vector3.left);

        // Call attack animation
        attackingAnimation();
    }

    // Helper that animates knight attacking target
    private void attackingAnimation()
    {
        // Store values
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
