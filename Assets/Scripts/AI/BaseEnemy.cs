using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour{
    
    public int health;
    public int damage;

    public AStar aStar;
    private List<Node> path;
    public float moveSpeed;

    private GameObject player;

    private void Awake()
    {
        aStar = FindObjectOfType<AStar>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        path = aStar.FindPath((Vector2)transform.position, (Vector2)player.transform.position);
    }

    public void Update()
    {
        path = aStar.FindPath((Vector2)transform.position, (Vector2)player.transform.position);

        if (path != null)
            if (path.Count > 0)
            {
                transform.position = Vector3.Lerp(transform.position, path[0].position, moveSpeed);
                if (Vector3.Distance(transform.position, path[0].position) < 1.0f)
                {
                    path.RemoveAt(0);
                }
            }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if(health <= 0)
        {
            //die
            this.Die();
        }
    }
	
    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
