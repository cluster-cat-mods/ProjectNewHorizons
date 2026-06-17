using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject _spawnPoint;
    private GraphNode _graphNode;
    private PathFinder _pathFinder;
    private Graph<Transform> _graph;
    

    private void OnAwake()
    {
        _pathFinder = _spawnPoint.GetComponent<GraphNode>().pathMap.gameObject.GetComponent<PathFinder>();
    }

    [Button]
    public void SetGraphNode()
    {
        _graphNode = _spawnPoint.GetComponent<GraphNode>();
    }
    
    public void SetSpawnPoint(GameObject spawnPointP)
    {
        _spawnPoint = spawnPointP;
        _graphNode = _spawnPoint.GetComponent<GraphNode>();
    }

    [Button]
    public void Move()
    {
        List<Transform> path = _pathFinder.CalculatePath(_graph, _spawnPoint.transform, _graphNode.transform);
        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i].position, path[i + 1].position, Color.red);
        }
    }
    
    
    
}
