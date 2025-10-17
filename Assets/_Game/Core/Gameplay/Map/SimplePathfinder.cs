using UnityEngine;
using System.Collections.Generic;
// 3. Pathfinding đơn giản - chỉ A* cơ bản
public class SimplePathfinder
{
    private class Node
    {
        public Tile Tile;
        public Node Parent;
        public float GCost; // khoảng cách từ start
        public float HCost; // khoảng cách ước tính đến goal
        public float FCost => GCost + HCost;

        public Node(Tile tile)
        {
            this.Tile = tile;
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
            closedSet.Add(currentNode.Tile);

            // Tìm thấy đích
            if (currentNode.Tile == goal)
            {
                return BuildPath(currentNode);
            }

            // Kiểm tra neighbors
            foreach (Tile neighbor in map.GetNeighbors(currentNode.Tile))
            {
                if (!neighbor.IsWalkable || closedSet.Contains(neighbor))
                    continue;

                float newGCost = currentNode.GCost + 1; // mỗi bước = 1
                Node neighborNode = GetNodeFromOpenSet(openSet, neighbor);

                if (neighborNode == null)
                {
                    neighborNode = new Node(neighbor);
                    neighborNode.GCost = newGCost;
                    neighborNode.HCost = GetDistance(neighbor, goal);
                    neighborNode.Parent = currentNode;
                    openSet.Add(neighborNode);
                }
                else if (newGCost < neighborNode.GCost)
                {
                    neighborNode.GCost = newGCost;
                    neighborNode.Parent = currentNode;
                }
            }
        }

        return null; // Không tìm thấy đường
    }

    private Node GetNodeFromOpenSet(List<Node> openSet, Tile tile)
    {
        foreach (Node node in openSet)
        {
            if (node.Tile == tile)
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
            path.Add(currentNode.Tile);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }
}
