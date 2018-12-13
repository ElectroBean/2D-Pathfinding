using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    public float speed;
    private Rigidbody2D rb2D;
    bool hasSpawned = false;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>(); 
    }

    // Use this for initialization
    void Start ()
    {
        Vector2 mouse = Input.mousePosition;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
        Vector2 offset = mouse - screenPoint;
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>());
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void FixedUpdate()
    {
        if (!hasSpawned)
        {
            rb2D.AddForce(transform.right * speed * Time.deltaTime, ForceMode2D.Impulse);
            hasSpawned = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BaseEnemy temp = collision.gameObject.GetComponent<BaseEnemy>();
        if (temp)
        {
            temp.TakeDamage(100);
        }
        Destroy(gameObject);
    }
}
