using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathFinding : MonoBehaviour
{
    Grid grid;
    PathManager pathManager;

    void Awake()
    {
        grid = GetComponent<Grid>();
        pathManager = GetComponent<PathManager>();
    }

    public void StartFindPath(Vector3 start, Vector3 finish)
    {
        StartCoroutine(FindPath(start, finish));
    }

    IEnumerator FindPath(Vector3 start, Vector3 finish)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathFound = false;

        Node startNode = grid.NodePosition(start);
        Node finishNode = grid.NodePosition(finish);

        // List could be a heap
        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];

            // This could be avoided with a heap sort
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost <= currentNode.fCost)
                {
                    if (openList[i].hCost < currentNode.hCost)
                    {
                        currentNode = openList[i];
                    }
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == finishNode)
            {
                pathFound = true;
                break;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.isTraversable || closedList.Contains(neighbour))
                {
                    continue;
                }

                int moveCost = currentNode.gCost + GetDistance(currentNode, neighbour);

                if (moveCost < neighbour.gCost || !openList.Contains(neighbour))
                {
                    neighbour.gCost = moveCost;
                    neighbour.hCost = GetDistance(neighbour, finishNode);
                    neighbour.parent = currentNode;

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }

        yield return null;

        if (pathFound)
        {
            waypoints = RetracePath(startNode, finishNode);
        }

        pathManager.FinishedProcessingPath(waypoints, pathFound);
    }

    Vector3[] RetracePath(Node start, Node finish)
    {
        List<Node> path = new List<Node>();
        Node currentNode = finish;

        while (currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        List<Vector3> waypointList = new List<Vector3>();
        Vector3 oldDir = Vector3.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector3 newDir = new Vector3(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY, path[i - 1].gridZ - path[i].gridZ);
            if (newDir != oldDir)
            {
                waypointList.Add(path[i].worldPos);
            }
            oldDir = newDir;
        }

        Vector3[] waypoints = waypointList.ToArray();
        Array.Reverse(waypoints);
        return waypoints;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        List<int> dstList = new List<int>();

        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        int dstZ = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);

        dstList.Add(dstX);
        dstList.Add(dstY);
        dstList.Add(dstZ);

        dstList.Sort();

        int dstAmount = 0;

        // 17: movement cost in a diagonal direction in all 3 dimentions (square root of 3 * 10),
        // 14 repressents a movement in a diagonal direction 2 dimenstions (square root of 2 * 10),
        // and 10 is a straight line in one dimension.
        if (dstList[2] > dstList[1] && dstList[1] > dstList[0])
        {
            dstAmount = 17 * dstList[0] + 14 * (dstList[1] - dstList[0]) + 10 * (dstList[2] - (dstList[1] + dstList[0]));
        }
        else if (dstList[2] > dstList[1] && dstList[1] == dstList[0])
        {
            dstAmount = 17 * dstList[0] + 10 * (dstList[2] - dstList[0]);
        }
        else if (dstList[2] == dstList[1] && dstList[1] > dstList[0])
        {
            dstAmount = 17 * dstList[0] + 14 * (dstList[2] - dstList[0]);
        }
        else if (dstList[2] == dstList[1] && dstList[1] == dstList[0])
        {
            dstAmount = 17 * dstList[0];
        }

        return dstAmount;
    }
}
