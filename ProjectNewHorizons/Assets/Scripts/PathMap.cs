using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using NaughtyAttributes;
using Newtonsoft.Json;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;


public class PathMap : MonoBehaviour
{
    [SerializeField] private bool drawDebug = false;
    [SerializeField, ShowIf("drawDebug")] private Color nodeColor = Color.cyan;
    [SerializeField, ShowIf("drawDebug")] private Color lineColor = Color.white;
    
    private bool emptyGraph = true;

    public Graph<Transform> Graph { get; private set; }

    private void Start()
    {
        if (Graph == null)  Graph = new();
    }

    private void OnDrawGizmos()
    {
        if (drawDebug && Graph != null)
        {
            foreach (var node in Graph.GetNodes()) if (node != null)
            {
                Gizmos.color = nodeColor;
                Gizmos.DrawWireSphere(node.position, 0.1f);
                
            }
            foreach (var kvp in Graph.GetAdjacencyList()) if (kvp.Key != null)
            {
                foreach (var val in kvp.Value) if (val != null) Debug.DrawLine(kvp.Key.position, val.position, lineColor);
                
            }
        }
    }

    [Button, ShowIf("emptyGraph")]
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
        emptyGraph = false;
        
        #if UNITY_EDITOR
        Selection.SetActiveObjectWithContext(node, null);
        #endif
    }
    
    [Button]
    public void ClearGraph()
    {
        if (Graph == null) Graph = new();
        foreach (var node in Graph.GetNodes()) if (node != null)
        {
            DestroyImmediate(node.gameObject);
        }
        Graph = new();
        emptyGraph = true;
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
