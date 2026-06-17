using System;
using System.Collections;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GraphNode : MonoBehaviour
{
    public PathMap pathMap { get; private set; }
    private Graph<Transform> _graph;

    [Button]
    public void AddConnection()
    {
        GameObject node = new();
        node.name = "Node " + _graph.GetNodeCount();
        node.tag = "Node";
        node.transform.parent = transform.parent;
        node.transform.position = transform.position;
        node.AddComponent<GraphNode>();
        node.GetComponent<GraphNode>().SetPathmap(pathMap);
        
        _graph.AddEdge(transform, node.transform);
        
        #if UNITY_EDITOR
        Selection.SetActiveObjectWithContext(node, null);
        #endif
    }

    [Button]
    public void RemoveNode()
    {
        StartCoroutine(RemoveNodeMethod());
    }

    private IEnumerator RemoveNodeMethod()
    {
        yield return new WaitForSeconds(0.001f);
        Undo.DestroyObjectImmediate(gameObject);
    }
    
    public void SetPathmap(PathMap pathMapP)
    {
        pathMap = pathMapP;
        _graph = pathMap.Graph;
    }
    
    
    private void OnDestroy()
    {
        _graph.RemoveNode(transform);
    }
    
}
