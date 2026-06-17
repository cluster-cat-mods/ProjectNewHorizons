using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject endPoint;
    private GraphNode _graphNode;
    private PathFinder _pathFinder;
    private Graph<Transform> _graph;
    private List<Transform> _path;
    
    [SerializeField] private bool drawDebug = false;
    [SerializeField, ShowIf("drawDebug")] private Color pathColor = Color.red;
    [SerializeField, ShowIf("drawDebug")] private Color spawnNodeColor = Color.green;
    [SerializeField, ShowIf("drawDebug")] private Color endNodeColor = Color.red;
    
    [Button]
    private void SetStuff()
    {
        _pathFinder = GetComponent<PathFinder>();
        _graph = spawnPoint.GetComponent<GraphNode>().pathMap.gameObject.GetComponent<PathMap>().Graph;
    }

    [Button]
    public void SetGraphNode()
    {
        _graphNode = spawnPoint.GetComponent<GraphNode>();
    }
    
    public void SetSpawnPoint(GameObject spawnPointP)
    {
        spawnPoint = spawnPointP;
        _graphNode = spawnPoint.GetComponent<GraphNode>();
    }

    [Button]
    public void Move()
    {
        Debug.Log(_pathFinder);
        Debug.Log(_graph);
        Debug.Log(spawnPoint);
        Debug.Log(endPoint);
        _path = _pathFinder.CalculatePath(_graph, spawnPoint.transform, endPoint.transform);
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _path.Count - 1; i++)
        {
            Debug.DrawLine(_path[i].position, _path[i + 1].position, pathColor);
            Gizmos.color = pathColor;
            Gizmos.DrawWireSphere((_path[i].position + _path[i + 1].position) / 2, 0.1f);
            Gizmos.DrawWireSphere(_path[i].position, 0.1f);
        }
        Gizmos.color = spawnNodeColor;
        Gizmos.DrawWireSphere(spawnPoint.transform.position, 0.1f);
        Gizmos.color = endNodeColor;
        Gizmos.DrawWireSphere(endPoint.transform.position, 0.1f);
    }
}
