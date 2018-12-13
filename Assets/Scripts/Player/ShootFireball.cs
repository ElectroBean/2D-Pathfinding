using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootFireball : MonoBehaviour {

    public GameObject fireballPrefab;
    public float shootDelay;
    private float shootCounter;

	// Use this for initialization
	void Start ()
    {
        shootCounter = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        shootCounter -= Time.deltaTime;
		if(Input.GetMouseButton(0))
        {
            if(shootCounter <= 0)
            {
                //shoot
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = 0;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);
                Vector2 direction = mousePos - transform.position;
                GameObject thing = Instantiate(fireballPrefab, (Vector2)transform.position, Quaternion.identity);
                shootCounter = shootDelay;
            }
        }
	}
}
