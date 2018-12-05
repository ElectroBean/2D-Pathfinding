using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
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

    public List<Node> FindPathAStar(Vector3 start, Vector3 end)
    {
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        Node startNode = grid.NodeFromWorldPosition(start);
        Node endNode = grid.NodeFromWorldPosition(end);

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node q = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < q.fCost || openList[i].fCost == q.fCost && openList[i].hCost < q.hCost)
                {
                    q = openList[i];
                    q.wasTested = true;
                }
            }

            openList.Remove(q);
            closedList.Add(q);

            List<Node> successors = grid.GetNeighbouringNodes(q);

            foreach (Node successor in successors)
            {
                if (successor == endNode)
                {
                    return GetFinalPath(startNode, endNode);
                }

                successor.gCost = q.gCost + Vector3.Distance(successor.position, q.position);
                successor.hCost = Vector3.Distance(endNode.position, successor.position);
                successor.fCost = successor.gCost + successor.hCost;
                successor.parent = q;

                if (closedList.Contains(successor) || successor.isWall || !successor.isFloor)
                {
                    continue;
                }

                if (!openList.Contains(successor))
                {
                    openList.Add(successor);
                }
            }
            
        }
        return null;
    }

    List<Node> GetFinalPath(Node start, Node end)
    {
        List<Node> finalPath = new List<Node>(); //initialize list of nodes for final path
        Node currentNode = end; //set current node to end node (going backwards using parents)

        while (currentNode != start) //while current node isn't the start node
        {
            currentNode.isPath = true;
            currentNode.wasTested = false;
            finalPath.Add(currentNode); //add current node to final path list
            currentNode = currentNode.parent; //set new current node to the parent of current node
        }

        finalPath.Reverse(); //reverse the list to get it in the correct order
        return finalPath; //return final path
    }
}
