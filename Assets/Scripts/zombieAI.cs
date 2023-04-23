using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class zombieAI : MonoBehaviour
{
    private Rigidbody zombieRB;
    private GameObject zombieModel, leftArmJoint, rightArmJoint, leftLegJoint, rightLegJoint, target;
    public NavMeshAgent zombie;
    // Start is called before the first frame update
    void Start()
    {
        zombieRB = GetComponent<Rigidbody>();
        zombieModel = transform.gameObject;
        leftArmJoint = transform.GetChild(2).gameObject;
        rightArmJoint = transform.GetChild(3).gameObject;
        leftLegJoint = transform.GetChild(4).gameObject;
        rightLegJoint = transform.GetChild(5).gameObject;
        // Get target placeholder 
        target = GameObject.Find("playableKnight");
    }

    // Update is called once per frame
    void Update()
    {
        zombie.SetDestination(target.transform.position);
    }
}
