using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyStats stats;
    [SerializeField] private GameManager manager;

    private Transform _spawnNode;
    private Transform _endNode;
    private PathFinder _pathFinder;
    private PathMap _pathMap; 
    private Graph<Transform> _graph;
    private List<Transform> _path;
    
    [SerializeField] private bool drawDebug = false;
    [SerializeField, ShowIf("drawDebug")] private Color pathColor = Color.red;
    [SerializeField, ShowIf("drawDebug")] private Color spawnNodeColor = Color.green;
    [SerializeField, ShowIf("drawDebug")] private Color endNodeColor = Color.red;

    private void OnAwake()
    {
        SetStuff();
        Move();
    }
    private void Start()
    {
        if (manager == null)
        {
            manager = FindAnyObjectByType<GameManager>();
        }

        stats = Instantiate(stats);
    }

    private void Update()
    {
        HitHive();

        if (stats.startStats.hp > 0) return;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (!manager.alive) return;
        if (!stats.startStats.canSpawnEnemy) return;
        for (int i = 0; i < stats.startStats.enemySpawnCount; i++)
        {
            Instantiate(stats.startStats.enemy, transform.position, Quaternion.identity);
        }
    }

    public void LoseHp(int amount)
    {
        stats.startStats.hp -= amount;
    }

    public void HitHive()
    {
        if (!(Vector3.Distance(transform.position, manager.transform.position) < .1f)) return;
        manager.LoseHP(stats.startStats.damage);
        Destroy(gameObject);
    }

    [Button]
    private void SetStuff()
    {
        Debug.Log("SpawnNode: " + _spawnNode);
        GraphNode graphNode = _spawnNode.gameObject.GetComponent<GraphNode>();
        Debug.Log("GraphNode: " + graphNode.name);
        _pathFinder = GetComponent<PathFinder>();
        _pathMap = graphNode.PathMap;
        _graph = graphNode.Graph;
        _endNode = _pathMap.EndNode;
    }

    [Button]
    public void Move()
    {
        _path = _pathFinder.CalculatePath(_graph, _spawnNode, _endNode);
    }

    public void SetSpawnPoint(Transform spawnPointP)
    {
        _spawnNode = spawnPointP;
    }
    
    private void OnDrawGizmos()
    {
        if (_path != null) for (int i = 0; i < _path.Count - 1; i++)
        {
            Debug.DrawLine(_path[i].position, _path[i + 1].position, pathColor);
            Gizmos.color = pathColor;
            Gizmos.DrawWireSphere((_path[i].position + _path[i + 1].position) / 2, 0.1f);
            Gizmos.DrawWireSphere(_path[i].position, 0.1f);
        }
        Gizmos.color = spawnNodeColor;
        if (_spawnNode != null) Gizmos.DrawWireSphere(_spawnNode.transform.position, 0.1f);
        Gizmos.color = endNodeColor;
        if (_endNode != null) Gizmos.DrawWireSphere(_endNode.transform.position, 0.1f);
    }
}
