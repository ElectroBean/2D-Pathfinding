using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{

    CustomGrid grid;

    private void Awake()
    {
        grid = GetComponent<CustomGrid>();
    }

    // Use this for initialization
    void Start()
    { 

    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<Node> FindPath(Vector3 start, Vector3 end)
    {
        Node startNode = grid.NodeFromWorldPosition(start);
        Node targetNode = grid.NodeFromWorldPosition(end);

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if(openList[i].FCost < currentNode.FCost || openList[i].FCost == currentNode.FCost && openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if(currentNode == targetNode) //if current is target
            {
                return GetFinalPath(startNode, targetNode); //calc final path
            }

            List<Node> temp = grid.GetNeighbouringNodes(currentNode);

            foreach(Node neighbourNode in temp)
            {
                if(neighbourNode.isWall || closedList.Contains(neighbourNode) || !neighbourNode.isFloor) //if neighbour is a wall or has been checked
                {
                    continue; //skip
                }
                int MoveCost = currentNode.gCost + GetManhattenDistance(currentNode, neighbourNode); //get f cost of neighbour

                if(!openList.Contains(neighbourNode) || MoveCost < neighbourNode.FCost) //if openlist doesnt contain current neighbour OR the move cost is less than the neighbours FCost
                {
                    neighbourNode.gCost = MoveCost; //set gCost to fCost
                    neighbourNode.hCost = GetManhattenDistance(neighbourNode, targetNode); //set hCost
                    neighbourNode.parent = currentNode; //set parent of node for retracing

                    if(!openList.Contains(neighbourNode)) //if neighbour is not in openlist
                    {
                        openList.Add(neighbourNode); //add it to open list
                    }
                }
            }
        }
        return null;
    }

    List<Node> GetFinalPath(Node start, Node end)
    {
        List<Node> finalPath = new List<Node>(); //initialize list of nodes for final path
        Node currentNode = end; //set current node to end node (going backwards using parents)

        while(currentNode != start) //while current node isn't the start node
        {
            finalPath.Add(currentNode); //add current node to final path list
            currentNode = currentNode.parent; //set new current node to the parent of current node
        }

        finalPath.Reverse(); //reverse the list to get it in the correct order
        return finalPath; //return final path
    }

    public int GetManhattenDistance(Node currentNode, Node neighbourNode) //returns manhattan distance for pathfinding
    {
        int x = Mathf.Abs(currentNode.gridX - neighbourNode.gridX);
        int y = Mathf.Abs(currentNode.gridY - neighbourNode.gridY);

        return x + y;
    }
}
