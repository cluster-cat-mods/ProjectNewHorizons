using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEditor;


public class PathMap : MonoBehaviour
{
    [SerializeField] private bool drawDebug = false;
    [SerializeField, ShowIf("drawDebug")] private Color nodeColor = Color.cyan;
    [SerializeField, ShowIf("drawDebug")] private Color spawnNodeColor = Color.green;
    [SerializeField, ShowIf("drawDebug")] private Color endNodeColor = Color.red;
    [SerializeField, ShowIf("drawDebug")] private Color lineColor = Color.white;

    private bool _emptyGraph = true;

    public Graph<Transform> Graph { get; private set; }

    private void Start()
    {
        if (Graph == null) Graph = new();
    }

    private void OnDrawGizmos()
    {
        if (drawDebug && Graph != null)
        {
            foreach (var node in Graph.GetNodes())
                if (node != null)
                {
                    switch (node.tag)
                    {
                        case "Node":
                            Gizmos.color = nodeColor;
                            break;
                        case "SpawnNode":
                            Gizmos.color = spawnNodeColor;
                            break;
                        case "EndNode":
                            Gizmos.color = endNodeColor;
                            break;
                    }

                    Gizmos.DrawWireSphere(node.position, 0.1f);

                }

            foreach (var kvp in Graph.GetAdjacencyList())
                if (kvp.Key != null)
                {
                    foreach (var val in kvp.Value)
                        if (val != null)
                            Debug.DrawLine(kvp.Key.position, val.position, lineColor);

                }
        }
    }

    [Button, ShowIf("_emptyGraph")]
    public void AddNode()
    {
        if (Graph == null) Graph = new();
        GameObject node = new();
        node.name = "Node 0";
        node.transform.parent = transform;
        node.transform.position = transform.position;
        node.AddComponent<GraphNode>();
        node.GetComponent<GraphNode>().SetPathmap(this);
        Graph.AddNode(node.transform);
        _emptyGraph = false;

        #if UNITY_EDITOR
        Selection.SetActiveObjectWithContext(node, null);
        #endif
    }

    [Button]
    public void ClearGraph()
    {
        if (Graph == null) Graph = new();
        foreach (var node in Graph.GetNodes())
            if (node != null)
            {
                DestroyImmediate(node.gameObject);
            }

        Graph = new();
        _emptyGraph = true;
    }

    [Button]
    public void PrintGraph()
    {
        foreach (var key in Graph.GetAdjacencyList().Keys)
        {
            string vals = "";
            foreach (var val in Graph.GetAdjacencyList()[key])
            {
                vals += val.name + ", ";
            }

            Debug.Log(key.name + "->" + vals);
        }
    }
}

//Jaro Code
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
