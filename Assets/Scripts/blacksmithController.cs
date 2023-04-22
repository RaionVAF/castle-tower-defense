using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blacksmithController : MonoBehaviour
{
    // Textures that emulate the king talking
    public Texture2D mainTexture, speakingTexture;
    // Renderer to change textures of material
    private Material m_Renderer;

    // Alchemist model joint references
    private GameObject leftArmJoint, rightArmJoint, headJoint, blacksmithHead;

    // Constants
    float noddingRotationSpeed = 2f;
    float jointRotationSpeed = 0.25f;
    float noddingRotationAngle = 2f;

    // Bools
    bool isSpeaking = true;
    bool armsAreRaised = false;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize GameObjects that will be used for animating
        headJoint = GameObject.Find("blacksmith/Head Joint");
        blacksmithHead = GameObject.Find("blacksmith/Head Joint/Head");
        leftArmJoint = GameObject.Find("blacksmith/Left Arm Joint");
        rightArmJoint = GameObject.Find("blacksmith/Right Arm Joint");

        // Fetch renderer from the GameObject
        m_Renderer = blacksmithHead.GetComponent<Renderer>().material;

        // Call animation coroutine
        StartCoroutine(animate());
    }

    // Update is called once per frame
    void Update()
    {
        // Call head idling animation
        headIdlingAnimation();
        // Call arm idling animation
        armIdlingAnimation();

        // If the blacksmith is not speaking, reset texture to its main texture
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

            // If the blacksmith is speaking, start talking coroutine
            StartCoroutine(animateSpeaking());

            // Coroutine that switches between multiple speaking textures
            IEnumerator animateSpeaking()
            {
                yield return new WaitForSeconds(2.0f);

                m_Renderer.mainTexture = speakingTexture;

                yield return new WaitForSeconds(2.0f);

                m_Renderer.mainTexture = mainTexture;
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

    // Helper that animates arms while the blacksmith is speaking
    private void armIdlingAnimation()
    {
        // Store values
        Quaternion raisingRotation = Quaternion.Euler(-45f, 0, 0);
        Quaternion swingingRotation = Quaternion.Euler(10f, 0, 0);

        if (!armsAreRaised)
        {
            leftArmJoint.transform.localRotation
                = Quaternion.Slerp(leftArmJoint.transform.localRotation, raisingRotation, Time.deltaTime * jointRotationSpeed);
            rightArmJoint.transform.localRotation
                = Quaternion.Slerp(rightArmJoint.transform.localRotation, raisingRotation, Time.deltaTime * jointRotationSpeed);

            if (isFinishedRotating(rightArmJoint.transform.localRotation, raisingRotation, 0.01f))
            {
                armsAreRaised = true;
            }
        }
        else
        {
            leftArmJoint.transform.localRotation
                = Quaternion.Slerp(leftArmJoint.transform.localRotation, swingingRotation, Time.deltaTime * jointRotationSpeed);
            rightArmJoint.transform.localRotation
                = Quaternion.Slerp(rightArmJoint.transform.localRotation, swingingRotation, Time.deltaTime * jointRotationSpeed);

            if (isFinishedRotating(rightArmJoint.transform.localRotation, swingingRotation, 0.01f))
            {
                armsAreRaised = false;
            }
        }
    }

    // Helper that determines if the blacksmith has finished rotation
    private static bool isFinishedRotating(Quaternion kingDirection, Quaternion targetDirection, float precision)
    {
        // Quaternion.Dot method returns a value between 1 and -1.
        // 1 or -1 means that the 2 quaternions are "exact", 0 means that they are far from close
        return Mathf.Abs(Quaternion.Dot(kingDirection, targetDirection)) >= 1 - precision;
    }
}
