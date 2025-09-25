using UnityEngine;
using System.Collections.Generic;
// 3. Pathfinding đơn giản - chỉ A* cơ bản
public class SimplePathfinder
{
    private class Node
    {
        public Tile tile;
        public Node parent;
        public float gCost; // khoảng cách từ start
        public float hCost; // khoảng cách ước tính đến goal
        public float FCost => gCost + hCost;

        public Node(Tile tile)
        {
            this.tile = tile;
        }
    }

    public List<Tile> FindPath(GridMap map, Tile start, Tile goal)
    {
        List<Node> openSet = new List<Node>();
        HashSet<Tile> closedSet = new HashSet<Tile>();

        Node startNode = new Node(start);
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            // Tìm node có F cost thấp nhất
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost)
                    currentNode = openSet[i];
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode.tile);

            // Tìm thấy đích
            if (currentNode.tile == goal)
            {
                return BuildPath(currentNode);
            }

            // Kiểm tra neighbors
            foreach (Tile neighbor in map.GetNeighbors(currentNode.tile))
            {
                if (!neighbor.IsWalkable || closedSet.Contains(neighbor))
                    continue;

                float newGCost = currentNode.gCost + 1; // mỗi bước = 1
                Node neighborNode = GetNodeFromOpenSet(openSet, neighbor);

                if (neighborNode == null)
                {
                    neighborNode = new Node(neighbor);
                    neighborNode.gCost = newGCost;
                    neighborNode.hCost = GetDistance(neighbor, goal);
                    neighborNode.parent = currentNode;
                    openSet.Add(neighborNode);
                }
                else if (newGCost < neighborNode.gCost)
                {
                    neighborNode.gCost = newGCost;
                    neighborNode.parent = currentNode;
                }
            }
        }

        return null; // Không tìm thấy đường
    }

    private Node GetNodeFromOpenSet(List<Node> openSet, Tile tile)
    {
        foreach (Node node in openSet)
        {
            if (node.tile == tile)
                return node;
        }
        return null;
    }

    private float GetDistance(Tile a, Tile b)
    {
        Vector2Int posA = a.Position;
        Vector2Int posB = b.Position;
        return Mathf.Abs(posA.x - posB.x) + Mathf.Abs(posA.y - posB.y); // Manhattan distance
    }

    private List<Tile> BuildPath(Node endNode)
    {
        List<Tile> path = new List<Tile>();
        Node currentNode = endNode;

        while (currentNode != null)
        {
            path.Add(currentNode.tile);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }
}
