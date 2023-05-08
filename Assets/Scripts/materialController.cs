using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class materialController : MonoBehaviour
{
    public float dist = 1f;
    public float diff = .025f;
    public float deg = 2f;

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
        end = transform.position + new Vector3(0f, dist, 0f);

        GameObject cam = GameObject.FindWithTag("MainCamera");
        
        if(cam != null) cam.GetComponent<Camera>().eventMask = inputLayerMask;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += new Vector3(0f, deg, 0f);
        
        transform.position = Vector3.Lerp(start, end, t);
        
        if(flip){
            t += diff;

            if(t >= 1){
                flip = false;
            }
        } else {
            t -= diff;

            if(t <= 0){
                flip = true;
            }
        }
    }

    void OnMouseDown(){
        if(transform.name == "stone(Clone)"){
            materials.changeStone(2);
        } else if(transform.name == "stick(Clone)"){
            materials.changeWood(2);
        } else {
            materials.changeIron(1);
        }

        Destroy(gameObject);
    }
}
