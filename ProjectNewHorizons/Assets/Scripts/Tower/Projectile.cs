using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Enemy _targetEnemy;

    [SerializeField] private Transform _targetTransform;

    private Vector3 _startPosition;
    [HideInInspector] public float _towerRange;
    [HideInInspector] public float _movementSpeed;
    [HideInInspector] public int _damage;

    private Vector3 _lookVector;
    private Vector3 _crossVector;

    private float _degreesBetweenVectors;

    private List<Transform> _enemyTransformList = new();

    void Start()
    {
        //SetTarget(_targetEnemy);
        _startPosition = transform.position;
    }
    void Update()
    {
        if (Vector3.Distance(_startPosition, transform.position) >= _towerRange)
        {
            Destroy(gameObject);
            return;
        }

        if (_targetTransform == null)
        {
            FindNewTarget();
        }
        LookAtTarget();
        MoveToTarget();
    }

    public void SetTarget(Enemy target)
    {
        _targetEnemy = target;
        _targetTransform = _targetEnemy.transform;
    }

    public void MoveToTarget()
    {
        if (_targetTransform == null) return;
        //transform.position = Vector3.Lerp(_startPosition, _targetTransform.position, 1f);
        var dist = Vector3.Distance(transform.position, _targetTransform.position);
        var step = Time.deltaTime * _movementSpeed;

        if (dist > 0.1) 
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetTransform.position, step);
            //transform.position += Vector3.Slerp(_startPosition, _targetTransform.position, step);
            //transform.position += transform.forward * .01f;
        }
        else
        {
            _targetEnemy.LoseHp(_damage);
            Destroy(gameObject);
        }
    }

    public void LookAtTarget()
    {
        if (_targetTransform == null) return;
        _lookVector = (_targetTransform.position - transform.position).normalized;
        _lookVector.y = 0;
        _crossVector = Vector3.Cross(transform.forward, _lookVector);

        float dot = Vector3.Dot(transform.forward, _lookVector);
        _degreesBetweenVectors = Mathf.Acos(Mathf.Clamp(dot, -1f, 1f)) * Mathf.Rad2Deg;

        var targetRotation = Quaternion.AngleAxis(_degreesBetweenVectors, _crossVector.normalized) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
    }

    private void FindNewTarget()
    {
        var allEnemyGameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var gameObject in allEnemyGameObjects)
        {
            _enemyTransformList.Add(gameObject.transform);
        }

        var closestEnemyObject = GetClosestEnemy(_enemyTransformList.ToArray());

        if (closestEnemyObject != null)
        {
            var closestEnemy = closestEnemyObject.GetComponent<Enemy>();
            if (closestEnemy == null) return;
            SetTarget(closestEnemy);
            _enemyTransformList.Clear();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    Transform GetClosestEnemy(Transform[] enemies)
    {
        Transform closestEnemy = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Transform t in enemies)
        {
            float dist = Vector3.Distance(t.position, currentPos);
            if (dist < minDist)
            {
                closestEnemy = t;
                minDist = dist;
            }
        }
        return closestEnemy;
    }
}
