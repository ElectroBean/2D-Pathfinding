using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTargetBlock : MonoBehaviour {
    public LayerMask hitLayers;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetMouseButtonDown(0))
        {
            Vector3 mouse = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;
            if(Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers))
            {
                transform.position = hit.point;
            }
        }
	}
}
