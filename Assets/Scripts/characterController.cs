using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterController : MonoBehaviour
{
    // Player model rigidbody and joint references
    private Rigidbody playerRB;
    private GameObject leftArmJoint, rightArmJoint, leftLegJoint, rightLegJoint;

    // Speed constants
    public float movementSpeed = 25f;
    private float jointRotationSpeed = 8f;
    private float armRotationAngle = 45f;
    private float legRotationAngle = 30f;

    // Public member to run moving animation script if true
    private bool isMoving = false;

    public Transform pivot;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize GameObjects that will be used for animating
        playerRB = GetComponent<Rigidbody>();
        leftArmJoint = GameObject.Find("Left Arm Joint");
        rightArmJoint = GameObject.Find("Right Arm Joint");
        leftLegJoint = GameObject.Find("Left Leg Joint");
        rightLegJoint = GameObject.Find("Right Leg Joint");
        // Start animation coroutine
        StartCoroutine(AnimateMovement());
    }

    // Update is called once per frame
    void Update()
    {
        // Takes WASD input and applys it as:
        //      - W up a region
        //      - A towards castle
        //      - S down a region
        //      - D away from the castle
        Vector3 input = new Vector3(-1f * Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
        input = Quaternion.Euler(0, pivot.eulerAngles.y, 0) * input;
        playerRB.MovePosition(playerRB.position + input * Time.deltaTime * movementSpeed);
        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) isMoving = true;
        
        // Rotates character in direction of movement
        if(input != Vector3.zero){
            Quaternion rotation = Quaternion.LookRotation(input, Vector3.up);
            playerRB.rotation = Quaternion.Slerp(playerRB.rotation, Quaternion.LookRotation(input, Vector3.up), 0.5F);
        }
    }

    // Coroutine that animates movement when isMoving is true
    IEnumerator AnimateMovement()
    {
        // Create a WaitUntil object that will wait until isMoving is true
        WaitUntil untilIsMoving = new WaitUntil(() => isMoving);

        while (true)
        {   
            // If isMoving is not true, reset joints to angle 0
            if (!isMoving)
            {
                // Reset joint rotations
                resetJointAngles();

                // Wait for reset animation to end
                yield return null;
            }

            // Only proceed to animation when isMoving is true
            yield return untilIsMoving;

            // Animate joint rotations 
            animateJointRotations();
            // Toggle isMoving
            isMoving = false;

            // Break loop and run again when isMoving is true
            yield return null;
        }
    }

    // Helper that rotates joint angles based on whether it is an arm or leg joint
    // Movements should be inverse of each other (i.e. left arm +45 deg, right arm -45 deg)
    private void animateJointRotations()
    {
        // Arms can rotate max 45 degrees, legs can rotate max 30 degress (based on model)
        float armRotation = Mathf.Sin(Time.time * jointRotationSpeed) * armRotationAngle;
        leftArmJoint.transform.localRotation = Quaternion.AngleAxis(armRotation, Vector3.left);
        rightArmJoint.transform.localRotation = Quaternion.AngleAxis(-armRotation, Vector3.left);
        float legRotation = Mathf.Sin(Time.time * jointRotationSpeed) * legRotationAngle;
        leftLegJoint.transform.localRotation = Quaternion.AngleAxis(-legRotation, Vector3.left);
        rightLegJoint.transform.localRotation = Quaternion.AngleAxis(legRotation, Vector3.left);
    }

    // Helper that resets joint angles to Euler(0,0,0)
    private void resetJointAngles()
    {
        leftArmJoint.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);
        rightArmJoint.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);
        leftLegJoint.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);
        rightLegJoint.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);
    }
}
