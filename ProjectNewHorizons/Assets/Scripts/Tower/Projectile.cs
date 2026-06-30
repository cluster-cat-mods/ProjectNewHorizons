using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Enemy _targetEnemy;
    private Transform _targetTransform;

    private Vector3 _startPosition;
    [HideInInspector] public float _towerRange;
    [HideInInspector] public float _movementSpeed;
    [HideInInspector] public int _damage;
    [HideInInspector] public string _hitSoundPath;
    
    private Vector3 _lookVector;
    private Vector3 _crossVector;
    private float _degreesBetweenVectors;
    

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
            Destroy(gameObject);
        }
        LookAtTarget();
        MoveToTarget();
    }

    public void SetTarget(Enemy target)
    {
        Debug.Log($"Setting projectile target to: {target.name}");
        _targetEnemy = target;
        Debug.Log($"Projectile target set to: {_targetEnemy.name}");
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
            if (!string.IsNullOrEmpty(_hitSoundPath)) RuntimeManager.PlayOneShot(_hitSoundPath);
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
}
