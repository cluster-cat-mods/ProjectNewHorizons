using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Enemy _targetEnemy;

    [SerializeField] private Transform _targetTransform;
    [SerializeField] private float movementSpeed;

    private Vector3 _startPosition;

    private Vector3 _lookVector;
    private Vector3 _crossVector;

    private float _degreesBetweenVectors;

    void Start()
    {
        SetTarget(_targetEnemy);
        _startPosition = transform.position;
    }
    void Update()
    {
        LookAtTarget();
        MoveToTarget();
    }

    public void SetTarget(Enemy target)
    {
        _targetEnemy = target;
        Debug.Log($"target = {target}");
        Debug.Log($"target E = {_targetEnemy}");
        Debug.Log($"transform E = {_targetEnemy.transform}");
        Debug.Log(_targetTransform);
        _targetTransform = _targetEnemy.transform;
    }

    public void MoveToTarget()
    {
        //transform.position = Vector3.Lerp(_startPosition, _targetTransform.position, 1f);
        var dist = Vector3.Distance(transform.position, _targetTransform.position);
        if (dist > 0.1) 
        {
            transform.position += Vector3.Slerp(_startPosition, _targetTransform.position, Time.deltaTime * movementSpeed);
            //transform.position += transform.forward * .01f;
        }
        else
        {
            Destroy(_targetEnemy);
            Destroy(gameObject);
        }
    }

    public void LookAtTarget()
    {
        _lookVector = (_targetTransform.position - transform.position).normalized;
        _lookVector.y = 0;
        _crossVector = Vector3.Cross(transform.forward, _lookVector);
        //Debug.Log(_crossVector);

        float dot = Vector3.Dot(transform.forward, _lookVector);
        _degreesBetweenVectors = Mathf.Acos(Mathf.Clamp(dot, -1f, 1f)) * Mathf.Rad2Deg;

        var targetRotation = Quaternion.AngleAxis(_degreesBetweenVectors, _crossVector.normalized) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
    }
}
