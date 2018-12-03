using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrid : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Layer Mask of Walls/Non-Traverseable areas")]
    private LayerMask wallMask;
    [SerializeField]
    [Tooltip("Layer Mask of Walkable areas")]
    private LayerMask floorMask;
    [SerializeField]
    [Tooltip("World Size of Grid (Shown Using Gizmos)")]
    private Vector2 gridWorldSize;
    [SerializeField]
    [Tooltip("Radii of nodes")]
    private float nodeRadius;
    [SerializeField]
    [Tooltip("Distance between each node")]
    private float distance;
    Node[,] grid;

    private List<Node> FinalPath;

    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    private Vector3 bottomLeft;

    // Use this for initialization
    void Start()
    {
        nodeDiameter = nodeRadius * 2; //set node diameter to twice the radius
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter); //number of nodes on each axis
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter); //number of nodes on each axis
        CreateGrid(); //Create the grid
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY]; //initialize new grid
        bottomLeft = transform.position - Vector3.up * (gridWorldSize.y / 2) - Vector3.right * (gridWorldSize.x / 2); //set bottomleft vector
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius); //set world point of current node
                bool Wall = false; //initialize if node is a wall or not
                bool Floor = false; //initialize if node is a floor piece

                if (Physics.CheckSphere(worldPoint, nodeRadius, wallMask)) //sphere check to determine if wall
                {
                    Wall = true;
                }
                if (Physics.CheckSphere(worldPoint, nodeRadius, floorMask)) //sphere check to determine if floor 
                {
                    Floor = true;
                }
                
                grid[x, y] = new Node(Wall, Floor, worldPoint, x, y); //set grid piece at x and y to a new node with determined values and world positions
            }
        }
    }

    public Node NodeFromWorldPosition(Vector3 worldPos)
    {
        float xPoint = ((worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x); //get world point relative to grid size
        float yPoint = ((worldPos.y + gridWorldSize.y / 2) / gridWorldSize.y); //''

        xPoint = Mathf.Clamp01(xPoint); //clamp relative points between 0 and 1
        yPoint = Mathf.Clamp01(yPoint); //clamp relative points between 0 and 1

        int x = Mathf.RoundToInt((gridSizeX - 1) * xPoint); //get grid position 
        int y = Mathf.RoundToInt((gridSizeY - 1) * yPoint); //get grid position 

        return grid[x, y];
    }

    public List<Node> GetNeighbouringNodes(Node currentNode)
    {
        List<Node> neighbouringNodes = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                //if we are on the node that was passed in, skip this iteration.
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = currentNode.gridX + x;
                int checkY = currentNode.gridY + y;

                //Make sure the node is within the grid.
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbouringNodes.Add(grid[checkX, checkY]); //Adds to the neighbours list.
                }

            }
        }

        return neighbouringNodes;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

        if (grid != null) //make sure grid exists
        {
            foreach (Node node in grid) //for each node in the grid
            {
                if (node.isWall) //if it's a wall
                {
                    Gizmos.color = Color.yellow; 
                }
                else
                {
                    if (node.isFloor) //if it's a floor piece
                        Gizmos.color = Color.green;
                    else              //if there is nothing here
                        Gizmos.color = Color.clear;
                }

                if (FinalPath != null) //if a final path has been found
                {
                    if (FinalPath.Contains(node)) //if the final path contains the current node
                        Gizmos.color = Color.red;
                }

                Gizmos.DrawCube(node.position, Vector3.one * (nodeDiameter - distance)); //draw all nodes
            }
        }
    }
}
