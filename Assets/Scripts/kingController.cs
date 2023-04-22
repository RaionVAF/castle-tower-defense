using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kingController : MonoBehaviour
{
    // Textures that emulate the king talking
    public Texture2D mainTexture, midSpeakingTexture, speakingTexture;
    // Renderer to change textures of material
    private Material m_Renderer;

    // King model joint references
    private GameObject leftArmJoint, rightArmJoint, leftElbowJoint, rightElbowJoint, 
                       headJoint, kingHead;
    // Starting quaternions
    private Quaternion startingLeftElbowRotation, startingRightElbowRotation;

    // Constants
    float noddingRotationSpeed = 2f;
    float elbowRotationSpeed = 0.25f;
    float noddingRotationAngle = 2f;

    // Bools
    bool isSpeaking = false;
    bool elbowsAreRaised = false;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize GameObjects that will be used for animating
        headJoint = GameObject.Find("king/Head Joint");
        kingHead = GameObject.Find("king/Head Joint/Head");
        leftArmJoint = GameObject.Find("king/Left Arm Joint");
        rightArmJoint = GameObject.Find("king/Right Arm Joint");
        leftElbowJoint = GameObject.Find("king/Left Arm Joint/Left Elbow Joint");
        rightElbowJoint = GameObject.Find("king/Right Arm Joint/Right Elbow Joint");

        // Store starting quaternions
        startingLeftElbowRotation = leftElbowJoint.transform.localRotation;
        startingRightElbowRotation = rightElbowJoint.transform.localRotation;

        // Fetch renderer from the GameObject
        m_Renderer = kingHead.GetComponent<Renderer>().material;

        // Call animation coroutine
        StartCoroutine(animate());
    }

    // Update is called once per frame
    void Update()
    {   
        // Call head idling animation
        headIdlingAnimation();

        // If the king is not speaking, reset texture to its main texture
        if (!isSpeaking)
        {
            m_Renderer.mainTexture = mainTexture;
        }
    }

    // Coroutine that animates movement when isMoving is true
    IEnumerator animate()
    {
        // Create a WaitUntil object
        WaitUntil untilIsSpeaking = new WaitUntil(() => isSpeaking);

        while (true)
        {   
            yield return untilIsSpeaking;

            // If the king is speaking, start talking coroutine
            StartCoroutine(animateSpeaking());

            // Also animate elbows to make the dialogue more lively
            elbowIdlingAnimation();

            // Coroutine that switches between multiple speaking textures
            IEnumerator animateSpeaking()
            {
                yield return new WaitForSeconds(1.0f);

                m_Renderer.mainTexture = midSpeakingTexture;

                yield return new WaitForSeconds(1.0f);

                m_Renderer.mainTexture = speakingTexture;
            }
            // Wait and then run again
            yield return null;
        }
    }

    // Helper that emulates head movement
    private void headIdlingAnimation()
    {
        float noddingRotation = Mathf.Sin(Time.time * noddingRotationSpeed) * noddingRotationAngle;
        headJoint.transform.localRotation = Quaternion.AngleAxis(noddingRotation, Vector3.left);
    }

    // Helper that animates elbows while the king is speaking
    private void elbowIdlingAnimation()
    {
        // Store values
        Quaternion raisingRotation = Quaternion.Euler(-45f, 0, 0);
        Quaternion swingingRotation = Quaternion.Euler(10f, 0, 0);

        if (!elbowsAreRaised)
        {
            leftElbowJoint.transform.localRotation
                = Quaternion.Slerp(leftElbowJoint.transform.localRotation, raisingRotation, Time.deltaTime * elbowRotationSpeed);
            rightElbowJoint.transform.localRotation
                = Quaternion.Slerp(rightElbowJoint.transform.localRotation, raisingRotation, Time.deltaTime * elbowRotationSpeed);

            if (isFinishedRotating(rightElbowJoint.transform.localRotation, raisingRotation, 0.01f))
            {
                elbowsAreRaised = true;
            }
        }
        else
        {
            leftElbowJoint.transform.localRotation
                = Quaternion.Slerp(leftElbowJoint.transform.localRotation, swingingRotation, Time.deltaTime * elbowRotationSpeed);
            rightElbowJoint.transform.localRotation
                = Quaternion.Slerp(rightElbowJoint.transform.localRotation, swingingRotation, Time.deltaTime * elbowRotationSpeed);

            if (isFinishedRotating(rightElbowJoint.transform.localRotation, swingingRotation, 0.01f))
            {
                elbowsAreRaised = false;
            }
        }
    }

    // Helper that resets elbow joint angles
    private void resetElbowJoints()
    {
        leftElbowJoint.transform.localRotation
            = Quaternion.Slerp(leftElbowJoint.transform.localRotation, startingLeftElbowRotation, Time.deltaTime * elbowRotationSpeed);
        rightElbowJoint.transform.localRotation 
            = Quaternion.Slerp(rightElbowJoint.transform.localRotation, startingRightElbowRotation, Time.deltaTime * elbowRotationSpeed);
    }

    // Helper that determines if the king has finished rotation
    private static bool isFinishedRotating(Quaternion kingDirection, Quaternion targetDirection, float precision)
    {
        // Quaternion.Dot method returns a value between 1 and -1.
        // 1 or -1 means that the 2 quaternions are "exact", 0 means that they are far from close
        return Mathf.Abs(Quaternion.Dot(kingDirection, targetDirection)) >= 1 - precision;
    }
}
