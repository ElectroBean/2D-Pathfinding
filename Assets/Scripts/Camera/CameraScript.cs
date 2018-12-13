using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    private GameObject player;
    public float lerpSpeed;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
	}

    private void LateUpdate()
    {
        if (!player)
            return;
        transform.position = Vector2.Lerp((Vector2)transform.position, (Vector2)player.transform.position, lerpSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
    }
}
