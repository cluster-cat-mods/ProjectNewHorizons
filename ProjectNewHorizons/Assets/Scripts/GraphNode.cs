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
        GameObject node = new()
        {
            name = "Node " + _graph.GetNodeCount(),
            tag = "Node",
            transform =
            {
                parent  = transform.parent,
                position = transform.position,
            }
        };
        GraphNode gn = node.AddComponent<GraphNode>();
        gn.SetPathmap(pathMap);
        
        _graph.AddEdge(transform, node.transform);
        
        #if UNITY_EDITOR
        Selection.SetActiveObjectWithContext(node, null);
        #endif
    }

    [Button]
    public void RemoveNode()
    {
        StartCoroutine(RemoveNodeCoroutine());
    }

    private IEnumerator RemoveNodeCoroutine()
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
