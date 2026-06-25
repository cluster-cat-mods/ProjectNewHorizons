using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyStats stats;
    [SerializeField] private GameManager manager;

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

    private EnemyStartStats runtimeStats;
    private bool diedByTower = false;

    private void Start()
    {
        Debug.Log($"{name} HP on spawn: {runtimeStats.hp}");

        if (manager == null)
        {
            manager = FindAnyObjectByType<GameManager>();
        }

        Debug.Log($"enemy {gameObject.name} just spawned");

        StartCoroutine(HitHiveCheck());
    }
    private void OnDestroy()
    {
        manager = FindAnyObjectByType<GameManager>();
        if (!manager.alive) return;
        if (diedByTower)
        {
            //gain corpse
            manager.GainCoins(runtimeStats.coinBounty);
        }
        //enemy spawning smaller ones
        if (!runtimeStats.canSpawnEnemy) return;
        for (int i = 0; i < runtimeStats.enemySpawnCount; i++)
        {
            GameObject littleEnemy = Instantiate(runtimeStats.enemy, transform.position, Quaternion.identity);

            Enemy enemy = littleEnemy.GetComponent<Enemy>();

            if (enemy != null)
            {
                Debug.Log($"Setting up spawned enemy {enemy.name}");
                enemy.SetStuff(GetClosestNode());
            }
        }

    }

    public void LoseHp(int amount)
    {
        runtimeStats.hp -= amount;

        if (runtimeStats.hp > 0) return;
        diedByTower = true;
        Destroy(gameObject);
    }

    public IEnumerator HitHiveCheck()
    {
        Debug.Log($"{name} started hive check");

        while (!(Vector3.Distance(transform.position, manager.hive.transform.position) < 1f))
        {
            yield return null;
            //Debug.Log("not around the hive yet");
            Debug.Log($"{name}: {Vector3.Distance(transform.position, manager.hive.transform.position)} distance to hive");
            //Debug.Log($"distance to the hive = {Vector3.Distance(transform.position, manager.hive.transform.position)}");
        }
        Debug.Log("close to the hive");
        manager.LoseHP(runtimeStats.damage);
        Destroy(gameObject);
    }
    
    public void SetStuff(Transform spawnPointP)
    {
        if (manager == null)
        {
            manager = FindAnyObjectByType<GameManager>();
        }

        runtimeStats = stats.BaseStats;

        _spawnNode = spawnPointP;
        
        GraphNode graphNode = _spawnNode.gameObject.GetComponent<GraphNode>();
        _pathFinder = GetComponent<PathFinder>();
        _pathMap = graphNode.PathMap;
        _graph = graphNode.Graph;
        _endNode = _pathMap.EndNode;

        Debug.Log($"{name} received spawn node {_spawnNode.name}");

        GoToDestination();
    }

    public Transform GetClosestNode()
    {
        var allNodes = GameObject.FindGameObjectsWithTag("Node");
        var dist = Mathf.Infinity;
        var closestNode = allNodes[0];
        foreach (var node in allNodes)
        {
            var newDistance = Vector3.Distance(transform.position, node.transform.position);
            if (newDistance < dist)
            {
                dist = newDistance;
                closestNode = node;
            }
        }
        return closestNode.transform;
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
                transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * runtimeStats.speed);
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