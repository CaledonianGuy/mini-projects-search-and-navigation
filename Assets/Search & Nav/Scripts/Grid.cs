using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector3 gridSize;
    public float nodeRadius;
    Node[,,] grid;
    public bool displayGizmos;

    float nodeDiameter;
    int gridSizeX, gridSizeY, gridSizeZ;

    public List<Node> path;

    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridSize.y / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(gridSize.z / nodeDiameter);

        CreateGrid();
    }

    public void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY, gridSizeZ];
        Vector3 gridBottomLeft = transform.position + Vector3.left * gridSize.x / 2 + Vector3.down * gridSize.y / 2 + Vector3.back * gridSize.z / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    // Acts as an index
                    Vector3 PointInWorld = gridBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius) + Vector3.forward * (z * nodeDiameter + nodeRadius);

                    // Checks if a node is walkable
                    bool isTraversable = !(Physics.CheckSphere(PointInWorld, nodeRadius, unwalkableMask));

                    // Adds node to grid
                    grid[x, y, z] = new Node(isTraversable, PointInWorld, x, y, z);
                }
            }
        }
    }

    // Gets node position in grid
    public Node NodePosition(Vector3 worldPos)
    {
        float percentX = (worldPos.x + gridSize.x / 2) / gridSize.x;
        float percentY = (worldPos.y + gridSize.y / 2) / gridSize.y;
        float percentZ = (worldPos.z + gridSize.z / 2) / gridSize.z;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        percentZ = Mathf.Clamp01(percentZ);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        int z = Mathf.RoundToInt((gridSizeZ - 1) * percentZ);

        return grid[x, y, z];
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (x == 0 && y == 0 && z == 0)
                    {
                        continue;
                    }

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;
                    int checkZ = node.gridZ + z;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY && checkZ >= 0 && checkZ < gridSizeZ)
                    {
                        neighbours.Add(grid[checkX, checkY, checkZ]);
                    }
                }
            }
        }

        return neighbours;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, gridSize.y, gridSize.z));
        if (grid != null && displayGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.isTraversable) ? Color.white : Color.red;
                //if (path != null)
                //{
                //    if (path.Contains(n))
                //    {
                //        Gizmos.color = Color.black;
                //        Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - 0.1f));
                //    }
                //}
                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
}
