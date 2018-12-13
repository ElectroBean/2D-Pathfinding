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
    public Node[,] grid;

    private List<Node> FinalPath;

    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    private Vector3 bottomLeft;

    private enum Dimensions { THREE, TWO };

    [SerializeField]
    private Dimensions dimension;

    // Use this for initialization
    void Start()
    {
        nodeDiameter = nodeRadius * 2; //set node diameter to twice the radius
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter); //number of nodes on each axis
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter); //number of nodes on each axis
        CreateGrid(); //Create the grid
    }

    public void CreateGrid()
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


                switch (dimension) // change grid creation depending on number of dimentions wanted
                {
                    case Dimensions.THREE:
                        if (Physics.CheckBox(worldPoint, new Vector3(float.Epsilon, float.Epsilon, 1), Quaternion.identity, floorMask))
                        {
                            Floor = true;
                        }
                        if (Physics.CheckBox(worldPoint, new Vector3(float.Epsilon, float.Epsilon, 1), Quaternion.identity, wallMask))
                        {
                            Wall = true;
                        }

                        break;
                    case Dimensions.TWO:
                        if (Physics2D.OverlapCircle(worldPoint, 0.1f, wallMask)) //check for 2d colliders and set bools if found
                        {
                            Wall = true;
                        }
                        if (Physics2D.OverlapCircle(worldPoint, 0.1f, floorMask)) //check for 2d colliders and set bools if found
                        {
                            Floor = true;
                        }

                        break;
                }

                grid[x, y] = new Node(Wall, Floor, worldPoint, x, y); //set grid piece at x and y to a new node with determined values and world positions
            }
        }
    }

    //old way of getting closest node doesnt work properly
    public Node NodeFromWorldPosition(Vector3 worldPos)
    {
        //float xPoint = ((worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x); //get world point relative to grid size
        //float yPoint = ((worldPos.y + gridWorldSize.y / 2) / gridWorldSize.y); //''
        //
        //xPoint = Mathf.Clamp01(xPoint); //clamp relative points between 0 and 1
        //yPoint = Mathf.Clamp01(yPoint); //clamp relative points between 0 and 1
        //
        //int x = Mathf.RoundToInt((gridSizeX - 1) * xPoint); //get grid position 
        //int y = Mathf.RoundToInt((gridSizeY - 1) * yPoint); //get grid position 
        //
        //return grid[x, y];
        Node closestNode = grid[0, 0];
        foreach(Node ca in grid)
        {
            if(Vector2.Distance(worldPos, ca.position) < Vector2.Distance(worldPos, closestNode.position))
            {
                closestNode = ca;
            }
        }
        return closestNode;
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
                    //grid[checkX, checkY].parent = currentNode;
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
                if (node.isStarOrEnd)
                {
                    Gizmos.color = Color.black;
                }
                else
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

                }


                //if(node.isPath)
                //{
                //    Gizmos.color = Color.red;
                //}
                //if(node.wasTested)
                //{
                //    Gizmos.color = Color.blue;
                //}

                Gizmos.DrawWireCube(node.position, Vector3.one * (nodeDiameter - distance)); //draw all nodes
            }
        }
    }
}
