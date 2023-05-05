using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alchemistController : MonoBehaviour
{
    // Textures that emulate the king talking
    public Texture2D mainTexture, speakingTexture;
    // Renderer to change textures of material
    private Material m_Renderer;

    // Cameras that will be enabled/disabled when player wants to interact with the alchemist
    public Camera mainCamera, alchemistCamera;
    // Also need a reference to the player to disable them when the menu is up
    public GameObject playerModel;

    // Alchemist model joint references
    private GameObject leftArmJoint, rightArmJoint, headJoint, alchemistHead;
    // Get interaction button reference
    public GameObject interactionPopup = null;

    public int hexNum;

    // Constants
    float noddingRotationSpeed = 2f;
    float jointRotationSpeed = 0.25f;
    float noddingRotationAngle = 2f;
    float floatingSpeed = 1f;

    // Bools
    bool isSpeaking = false;
    bool armsAreRaised = false;
    bool isPlayerDetected = false;
    bool isUpgradeMenuOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize GameObjects that will be used for animating
        headJoint = GameObject.Find("alchemist/Head Joint");
        alchemistHead = GameObject.Find("alchemist/Head Joint/Head");
        leftArmJoint = GameObject.Find("alchemist/Left Arm Joint");
        rightArmJoint = GameObject.Find("alchemist/Right Arm Joint");

        // Initialize button popup and set it inactive
        // interactionPopup = GameObject.Find("alchemist/Canvas Holder/Interaction Canvas/Interaction Popup");
        interactionPopup.SetActive(false);

        // Fetch renderer from the GameObject
        m_Renderer = alchemistHead.GetComponent<Renderer>().material;

        // Start animation coroutine
        StartCoroutine(animate());
    }

    // Update is called once per frame
    void Update()
    {
        // Run idle animations
        headIdlingAnimation();
        armIdlingAnimation();

        // If the player is detected set the popup and its floating animation to be active
        if (isPlayerDetected)
        {
            interactionPopup.SetActive(true);
            // interactionPopup.transform.rotation = Quaternion.LookRotation(interactionPopup.transform.position - mainCamera.transform.position);
            floatPopup();

            // Activate camera interactions (E key switches cameras b/w menu and world)
            cameraInteractions();
        }
        else
        {
            interactionPopup.SetActive(false);
        }

        // If the alchemist is not speaking, reset texture to its main texture
        if (!isSpeaking)
        {
            m_Renderer.mainTexture = mainTexture;
        }
    }

    // Detect when the NPC's collider touches the player and toggle's isPlayerDetected true
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isPlayerDetected = true;
        }
    }

    // Detect when the NPC's collider no longer touches the player and toggle's isPlayerDetected false
    void OnTriggerExit(Collider other)
    {
        isPlayerDetected = false;
    }

    // Coroutine that animates movement when isMoving is true
    IEnumerator animate()
    {
        // Create a WaitUntil object
        WaitUntil untilIsSpeaking = new WaitUntil(() => isSpeaking);

        while (true)
        {
            yield return untilIsSpeaking;

            // If the alchemist is speaking, start talking coroutine
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

    // Helper that animates arms while the alchemist is speaking
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

    // Helper that floats popup up and down (-1 to 1)
    private void floatPopup()
    {
        interactionPopup.transform.localPosition
            = new Vector3(0, Mathf.Sin(Time.time * floatingSpeed), 0);
    }

    // Helper that defines player logic for activating main and blacksmith cameras
    private void cameraInteractions()
    {
        // If the player presses E when the popup is active, switch to upgrade menu
        // If the menu is up and the player presses E again, leave upgrade menu
        if (Input.GetKeyDown(KeyCode.E) && !isUpgradeMenuOpen)
        {
            // Toggle cameras
            mainCamera.gameObject.SetActive(false);
            alchemistCamera.gameObject.SetActive(true);
            // Turn player model off so that it doesnt interfere with the menu camera
            playerModel.gameObject.SetActive(false);

            // Toggle bool
            isUpgradeMenuOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.E) && isUpgradeMenuOpen)
        {
            // Toggle cameras
            alchemistCamera.gameObject.SetActive(false);
            mainCamera.gameObject.SetActive(true);
            // Turn player model on so since the user left the menu
            playerModel.gameObject.SetActive(true);

            // Toggle bool
            isUpgradeMenuOpen = false;
        }
    }

    // Helper that determines if the alchemist has finished rotation
    private static bool isFinishedRotating(Quaternion kingDirection, Quaternion targetDirection, float precision)
    {
        // Quaternion.Dot method returns a value between 1 and -1.
        // 1 or -1 means that the 2 quaternions are "exact", 0 means that they are far from close
        return Mathf.Abs(Quaternion.Dot(kingDirection, targetDirection)) >= 1 - precision;
    }

    public void speaking(bool status){
        isSpeaking = status;
    }
}
