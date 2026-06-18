using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public List<Transform> Path = new();
    HashSet<Transform> Discovered = new();

    public List<Transform> CalculatePath(Graph<Transform> graph, Transform from, Transform to)
    {
        List<Transform> shortestPath = new();
        shortestPath = Dijkstra(graph, from, to);
        Path = shortestPath; //Used for drawing the Path

        return shortestPath;
    }

    public List<Transform> Dijkstra(Graph<Transform> graph, Transform start, Transform end)
    {
        //Use this "Discovered" list to see the nodes in the visual debugging used on OnDrawGizmos()
        Discovered.Clear();

        Dictionary<Transform, float> costMap = new();
        Dictionary<Transform, Transform> parentMap = new();
        List<(Transform node, float cost)> ToDo = new();

        costMap[start] = 0;
        ToDo.Add((start, 0));
        Discovered.Add(start);

        while (ToDo.Count > 0)
        {
            ToDo = ToDo.OrderByDescending(node => node.cost).ToList();

            var currentNode = ToDo[ToDo.Count - 1].node;
            ToDo.RemoveAt(ToDo.Count - 1);

            if (currentNode == end)
            {
                return ReconstructPath(parentMap, start, end);
            }

            var neighbors = graph.GetNeighbors(currentNode);

            foreach (Transform neighbor in neighbors)
            {
                var newCost = costMap[currentNode] + Cost(currentNode, neighbor);

                if (!costMap.ContainsKey(neighbor) || newCost < costMap[neighbor])
                {
                    Discovered.Add(neighbor);

                    costMap[neighbor] = newCost;
                    parentMap[neighbor] = currentNode;
                    ToDo.Add((neighbor, newCost));
                }
            }

        }

        /* */
        return new List<Transform>(); // No Path found
    }

    public float Cost(Transform from, Transform to)
    {
        return Vector3.Distance(from.position, to.position);
    }

    List<Transform> ReconstructPath(Dictionary<Transform, Transform> parentMap, Transform start, Transform end)
    {
        List<Transform> path = new();
        Transform currentNode = end;

        while (currentNode != start)
        {
            path.Add(currentNode);
            currentNode = parentMap[currentNode];
        }

        path.Add(start);
        path.Reverse();
        return path;
    }
}
