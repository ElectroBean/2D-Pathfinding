using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    //
    public int gridX;
    //
    public int gridY;

    public bool isWall;

    public bool isFloor;

    public Vector3 position;

    public Node parent; //parent for path finding

    public int gCost, hCost; //G Cost and H Cost

    public int FCost { get { return gCost + hCost; } } //getter and setter for FCost

    public Node(bool isWall, bool isFloor, Vector3 aPos, int aGridX, int aGridY) //constructor 
    {
        this.isWall = isWall;
        this.isFloor = isFloor;
        position = aPos;
        gridX = aGridX;
        gridY = aGridY;
    }

}
