using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class PathHighlight : MonoBehaviour
{

    private bool isMoving = false;
    [SerializeField] private float speed = 5;

    [SerializeField] private bool drawDebug = false;
    [SerializeField, ShowIf("drawDebug")] private Color pathColor = Color.purple;
    [SerializeField, ShowIf("drawDebug")] private Color spawnNodeColor = Color.green;
    [SerializeField, ShowIf("drawDebug")] private Color endNodeColor = Color.red;

    private Transform _spawnNode;
    private Transform _endNode;
    private PathFinder _pathFinder;
    private PathMap _pathMap; 
    private Graph<Transform> _graph;
    private List<Transform> _path = new();

    private GameManager _manager;
    private GameObject[] _allNodes;

    private void Start()
    {
        StartCoroutine(HitNestCheck());
    }
    

    public IEnumerator HitNestCheck()
    {

        while (!(Vector3.SqrMagnitude(transform.position - _manager.nest.transform.position) < 1f)) yield return null;
        Destroy(gameObject);
    }
    
    public void SetStuff(Transform spawnPointP, GameManager manager, GameObject[] allNodes)
    {
        _manager = manager;
        _allNodes = allNodes;
        _spawnNode = spawnPointP;
        GraphNode graphNode = _spawnNode.gameObject.GetComponent<GraphNode>();
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
            while (Vector3.Distance(transform.position, target + Vector3.up * .5f) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target + Vector3.up * .5f, Time.deltaTime * speed);
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