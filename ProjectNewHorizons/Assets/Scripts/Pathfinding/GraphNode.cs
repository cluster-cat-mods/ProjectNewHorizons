using System;
using System.Collections;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GraphNode : MonoBehaviour
{
    public PathMap PathMap { get; private set; }
    public Graph<Transform> Graph { get; private set; }

    #if UNITY_EDITOR
    [Button]
    public void AddConnection()
    {
        GameObject node = new()
        {
            name = "Node " + Graph.GetNodeCount(),
            tag = "Node",
            transform =
            {
                parent  = transform.parent,
                position = transform.position,
            }
        };
        GraphNode gn = node.AddComponent<GraphNode>();
        gn.SetPathmap(PathMap);
        
        Graph.AddEdge(transform, node.transform);
        
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
    
    private void OnDestroy()
    {
        Graph.RemoveNode(transform);
    }
    
    #endif
    
    public void SetPathmap(PathMap pathMapP)
    {
        PathMap = pathMapP;
        Graph = PathMap.Graph;
    }
    
    
}
