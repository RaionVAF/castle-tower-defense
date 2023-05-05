using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class materialController : MonoBehaviour
{
    private float t = 0;
    private bool flip = true;
    private Vector3 start;
    private Vector3 end;
    private GameObject materialManager;
    private materialTracker materials;
    [SerializeField] private LayerMask inputLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        materialManager = GameObject.Find("Material Manager"); 
        materials = materialManager.GetComponent<materialTracker>();

        start = transform.position;
        end = transform.position + new Vector3(0f, 1f, 0f);

        Camera.main.eventMask = inputLayerMask;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += new Vector3(0f, 2f, 0f);
        
        transform.position = Vector3.Lerp(start, end, t);
        
        if(flip){
            t += .025f;

            if(t >= 1){
                flip = false;
            }
        } else {
            t -= .025f;

            if(t <= 0){
                flip = true;
            }
        }
    }

    void OnMouseDown(){
        if(transform.name == "stone(Clone)"){
            materials.changeStone(10);
        } else if(transform.name == "stick(Clone)"){
            materials.changeWood(10);
        } else {
            materials.changeIron(10);
        }

        Destroy(gameObject);
    }
}
