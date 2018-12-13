using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float movementSpeed;
    private Vector2 movementDir;
    private Rigidbody2D rb2D;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start ()
    {
        rb2D.interpolation = RigidbodyInterpolation2D.Interpolate;
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        movementDir.Set(horizontal, vertical);
        rb2D.MovePosition(rb2D.position + movementDir * movementSpeed * Time.deltaTime);
    }
}
