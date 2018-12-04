using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{

    AStarPathfinding aStar;
    List<Node> path;
    [SerializeField]
    [Tooltip("LayerMask of Walkable areas")]
    private LayerMask hitLayers;
    [SerializeField]
    [Tooltip("What key you want to press to initiate pathfinding")]
    private KeyCode whichkey;
    [SerializeField]
    private float moveSpeed;

    private void Awake()
    {
        aStar = FindObjectOfType<AStarPathfinding>();
    }

    // Use this for initialization
    void Start()
    {
        path = new List<Node>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(whichkey))
        {
            Vector3 mouse = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers))
            {
                path = aStar.FindPath(transform.position, hit.point);
            }
        }

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
}
