using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyStats stats;
    [SerializeField] private GameManager manager;

    [SerializeField]
    private float Speed = 5f;

    private bool isMoving = false;

    [SerializeField] private bool drawDebug = false;
    [SerializeField, ShowIf("drawDebug")] private Color pathColor = Color.red;
    [SerializeField, ShowIf("drawDebug")] private Color spawnNodeColor = Color.green;
    [SerializeField, ShowIf("drawDebug")] private Color endNodeColor = Color.red;

    private Transform _spawnNode;
    private Transform _endNode;
    private PathFinder _pathFinder;
    private PathMap _pathMap; 
    private Graph<Transform> _graph;
    private List<Transform> _path = new();

    private void Update()
    {
        HitHive();

        if (stats.startStats.hp > 0) return;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (!manager.alive) return;
        //gain coins
        manager.GainCoins(stats.startStats.coinBounty);

        //enemy spawning smaller ones
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
    private void SetStuff(Transform spawnPointP)
    {
        if (manager == null)
        {
            manager = FindAnyObjectByType<GameManager>();
        }
        stats = Instantiate(stats);
        
        _spawnNode = spawnPointP;
        
        Debug.Log("SpawnNode: " + _spawnNode);
        GraphNode graphNode = _spawnNode.gameObject.GetComponent<GraphNode>();
        Debug.Log("GraphNode: " + graphNode.name);
        _pathFinder = GetComponent<PathFinder>();
        _pathMap = graphNode.PathMap;
        _graph = graphNode.Graph;
        _endNode = _pathMap.EndNode;
        
        GoToDestination();
    }

    [Button]
    public void GoToDestination()
    {
        if (!isMoving)
        {
            _path = _pathFinder.CalculatePath(_graph, _spawnNode, _endNode);
            StartCoroutine(FollowPathCoroutine(_path));
        }
    }

    IEnumerator FollowPathCoroutine(List<Transform> path)
    {
        if (path == null || path.Count == 0)
        {
            Debug.Log("No Path found");
            yield break;
        }
        isMoving = true;
        for (int i = 0; i < path.Count; i++)
        {
            Vector3 target = path[i].position;
            // Move towards the target position
            while (Vector3.Distance(transform.position, target) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * Speed);
                yield return null;
            }

            //Debug.Log($"Reached target: {target}");
        }
        isMoving = false;
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