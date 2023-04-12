using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterController : MonoBehaviour
{
    // Player model rigidbody and joint references
    private Rigidbody playerRB;
    private GameObject playerModel, leftArmJoint, rightArmJoint, leftLegJoint, rightLegJoint;

    // Speed constants
    float movementSpeed = 75f;
    float playerRotationSpeed = 10f;
    float jointRotationSpeed = 8f;
    float armRotationAngle = 45f;
    float legRotationAngle = 30f;

    // Public member to run moving animation script if true
    bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize GameObjects that will be used for animating
        playerRB = GetComponent<Rigidbody>();
        playerModel = GameObject.Find("playableKnight");
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
        // Check user input on character to determine how character moves
        // Possible input (W, A, S, D, W+A, W+D, S+D, S+A)
        // Null input - No movement (W+S, A+D)
        if (!isNullMovement())
        {
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            {
                // Move character forward-left
                float x = calculateCoord(playerModel.transform.localPosition.x, true);
                float z = calculateCoord(playerModel.transform.localPosition.z, false);
                moveCharacter(new Vector3(x, playerModel.transform.position.y, z));

                // Rotate character model to look forward-left
                rotateCharacter(Quaternion.Euler(0, 135, 0));
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
            {
                // Move character backward-left
                float x = calculateCoord(playerModel.transform.localPosition.x, true);
                float z = calculateCoord(playerModel.transform.localPosition.z, true);
                moveCharacter(new Vector3(x, playerModel.transform.position.y, z));

                // Rotate character model to look backward-left
                rotateCharacter(Quaternion.Euler(0, 45, 0));
            }
            else if (Input.GetKey(KeyCode.A))
            {
                // Move character left
                float x = calculateCoord(playerModel.transform.localPosition.x, true);
                moveCharacter(new Vector3(x, playerModel.transform.position.y, playerModel.transform.position.z));

                // Rotate character to look left
                rotateCharacter(Quaternion.Euler(0, 90, 0));
            }
            else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            {
                // Move character forward-right
                float x = calculateCoord(playerModel.transform.localPosition.x, false);
                float z = calculateCoord(playerModel.transform.localPosition.z, false);
                moveCharacter(new Vector3(x, playerModel.transform.position.y, z));

                // Rotate character model to look forward-right
                rotateCharacter(Quaternion.Euler(0, -135, 0));
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            {
                // Move character backward-right
                float x = calculateCoord(playerModel.transform.localPosition.x, false);
                float z = calculateCoord(playerModel.transform.localPosition.z, true);
                moveCharacter(new Vector3(x, playerModel.transform.position.y, z));

                // Rotate character to look backward-right
                rotateCharacter(Quaternion.Euler(0, 315, 0));
            }
            else if (Input.GetKey(KeyCode.D))
            {
                // Move character right
                float x = calculateCoord(playerModel.transform.localPosition.x, false);
                moveCharacter(new Vector3(x, playerModel.transform.position.y, playerModel.transform.position.z));

                // Rotate character to look right
                rotateCharacter(Quaternion.Euler(0, -90, 0));
            }
            else if (Input.GetKey(KeyCode.W))
            {
                // Move character forward
                float z = calculateCoord(playerModel.transform.localPosition.z, false);
                moveCharacter(new Vector3(playerModel.transform.position.x, playerModel.transform.position.y, z));
            
                // Rotate character to look forward
                rotateCharacter(Quaternion.Euler(0, 180, 0));
            }
            else if (Input.GetKey(KeyCode.S))
            {
                // Move character backward
                float z = calculateCoord(playerModel.transform.localPosition.z, true);
                moveCharacter(new Vector3(playerModel.transform.position.x, playerModel.transform.position.y, z));

                // Rotate character to look backward
                rotateCharacter(Quaternion.Euler(0, 0, 0));
            }
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

    // Helper to check null movement
    private bool isNullMovement()
    {
        if ((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)))
        {   
            return true;
        }
        return false;
    }

    // Helper to calculate new coord position based on whether the movement goes on the pos/neg axis
    private float calculateCoord(float pos, bool positiveMovement)
    {
        if (positiveMovement)
        {
            return pos += Time.deltaTime * movementSpeed;
        }
        else
        {
            return pos -= Time.deltaTime * movementSpeed;
        }
    }

    // Helper to move character based on user movement
    private void moveCharacter(Vector3 newPos)
    {
        playerModel.transform.localPosition = newPos;
        // Toggle isMoving
        isMoving = true;
    }

    // Helper to rotate character towards user movement
    private void rotateCharacter(Quaternion rotation)
    {
        playerModel.transform.localRotation
                = Quaternion.Slerp(playerModel.transform.localRotation, rotation, Time.deltaTime * playerRotationSpeed);
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
