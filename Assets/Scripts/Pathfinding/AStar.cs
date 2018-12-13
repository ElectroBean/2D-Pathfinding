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
        List<Node> openList = new List<Node>(); //initialize open list
        List<Node> closedList = new List<Node>(); //initialize closed list

        Node startNode = grid.NodeFromWorldPosition(start); //get nodes from input world positions
        Node endNode = grid.NodeFromWorldPosition(end); //get nodes from input world positions

        openList.Add(startNode); //push startNode to open list

        while (openList.Count > 0) //while open list isnt empty
        {
            Node currentNode = openList[0];

            openList.Sort((x, y) => x.fCost.CompareTo(y.fCost));
            openList.Reverse();
            currentNode = openList[0];
            openList.Remove(currentNode); //remove current node from open list
            closedList.Add(currentNode); //add current node to closed list

            if (currentNode == endNode) //if current node is the end node
            {
                return GetFinalPath(startNode, endNode); //return final path
            }

            List<Node> successors = grid.GetNeighbouringNodes(currentNode); //get a list of all current node's neighbours
            foreach (Node neighbours in successors) //for each of the neighbours
            {
                if (closedList.Contains(neighbours) || neighbours.isWall || !neighbours.isFloor) //if the current neighbour is a wall/floor or is in the closed list
                    continue; //skip current neighbour

                neighbours.gCost = currentNode.gCost + GetEuclideanDistance(neighbours.position, currentNode.position); //set neighbours g cost
                neighbours.hCost = GetEuclideanDistance(endNode.position, neighbours.position); //set neighbours h cost
                neighbours.fCost = neighbours.gCost + neighbours.hCost; // set neighbours f cost

                if (!openList.Contains(neighbours)) //if the open list doesnt contain current neighbour
                {
                    openList.Add(neighbours); //add neighbour to open list
                    neighbours.parent = currentNode; //set neighbours parent to current node
                }

            }

        }
        return null; //else return null
    }

    public List<Node> FindPath(Vector3 start, Vector3 target)
    {
        List<Node> openList = new List<Node>(); //initialize open list
        List<Node> closedList = new List<Node>(); //initialize closed list

        Node startNode = grid.NodeFromWorldPosition(start); //get nodes from input world positions
        Node endNode = grid.NodeFromWorldPosition(target); //get nodes from input world positions

        startNode.isStarOrEnd = true;
        endNode.isStarOrEnd = true;

        openList.Add(startNode); //push startNode to open list
        while (openList.Count > 0)
        {
            openList.Sort((x, y) => x.fCost.CompareTo(y.fCost));
            Node currentNode = openList[0];
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == endNode)
            {
                return GetFinalPath(startNode, endNode);
            }

            List<Node> successors = grid.GetNeighbouringNodes(currentNode);
            foreach (Node successor in successors)
            {
                if (closedList.Contains(successor) || successor.isWall || !successor.isFloor)
                {
                    continue;
                }

                successor.gCost = currentNode.gCost + Vector2.Distance(currentNode.position, successor.position);
                successor.hCost = Vector2.Distance(successor.position, endNode.position);
                successor.fCost = successor.gCost + successor.hCost;

                if (!openList.Contains(successor))
                {
                    openList.Add(successor);
                    successor.parent = currentNode;

                }
            }
        }
        return null;
    }

    float GetEuclideanDistance(Vector3 current, Vector3 target)
    {
        return Mathf.Sqrt((current.x - target.x) / 2 + (current.y - target.y) / 2);
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
