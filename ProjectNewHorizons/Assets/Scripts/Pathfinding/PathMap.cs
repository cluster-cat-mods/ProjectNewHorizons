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
    [SerializeField, ShowIf("drawDebug")] private float drawSize = .2f;

    private bool _emptyGraph = true;
    public List<Transform> SpawnNodes {get ; private set;} = new();
    public Transform EndNode { get; private set; }

    [field: SerializeField] public Graph<Transform> Graph { get; private set; } = new();

    private void OnAwake()
    {
        FindNodes();
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

                    Gizmos.DrawWireSphere(node.position, drawSize);

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
        node.tag = "Node";
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
    public void FindNodes()
    {
        SpawnNodes.Clear();
        Debug.Log($"graph = {Graph}");
        Debug.Log($"graph nodes = {Graph.GetNodes()}");
        Debug.Log($"graph nodes count = {Graph.GetNodes().Count}");
        foreach (var node in Graph.GetNodes())
        {
            Debug.Log(node.name);
            Debug.Log(node.tag);
            switch (node.tag)
            {
                case "SpawnNode":
                    SpawnNodes.Add(node.transform);
                    Debug.Log($"Spawn Node Added = {node.transform}");
                    break;
                case "EndNode":
                    EndNode = node.transform;
                    Debug.Log($"End Node Added = {node.transform}");
                    break;
            }
        }
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

