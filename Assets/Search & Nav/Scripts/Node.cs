using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool isTraversable;
    public Vector3 worldPos;
    public int gridX, gridY, gridZ;

    public int gCost, hCost;
    public Node parent;

    public Node(bool isTraversable, Vector3 worldPos, int gridX, int gridY, int gridZ)
    {
        this.isTraversable = isTraversable;
        this.worldPos = worldPos;
        this.gridX = gridX;
        this.gridY = gridY;
        this.gridZ = gridZ;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
