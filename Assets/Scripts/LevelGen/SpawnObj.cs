using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObj : MonoBehaviour
{

    public GameObject[] tiles;
    public Transform myParent;
    public bool isOpening;

    // Use this for initialization
    void Start()
    {
        if (isOpening)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    RaycastHit2D thing = Physics2D.Raycast(transform.position, new Vector2(i, j), 0.5f);
                    if (thing)
                        if (thing.collider.gameObject.tag == "Border")
                        {
                            Instantiate(tiles[Random.Range(0, tiles.Length)], transform.position, Quaternion.identity, myParent);
                            break;
                        }
                }
            }
        }
        else
        {
            if (tiles.Length > 0)
            {
                Instantiate(tiles[Random.Range(0, tiles.Length)], transform.position, Quaternion.identity, myParent);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
