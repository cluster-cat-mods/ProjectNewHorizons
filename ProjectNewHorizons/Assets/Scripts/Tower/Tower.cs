using NaughtyAttributes;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private GameManager manager;

    [SerializeField] private TowerStats stats;
    [SerializeField] private Projectile projectile;

    [SerializeField] private bool ShowRange;
    [SerializeField, ShowIf("ShowRange")] private Color rangeDrawingColor = new Color(0,.2f,1,.4f);

    private List<Transform> _enemyTransformList = new();
    private Enemy _closestEnemy;

    private void Start()
    {
        if (manager == null)
        {
            manager = FindAnyObjectByType<GameManager>();
        }

        stats = Instantiate(stats);

        StartCoroutine(Shoot());
    }
    public IEnumerator Shoot()
    {
        while (stats.antAllocation.currentAntsAllocated >= stats.antAllocation.minimumAntsAllocated)
        {
            yield return new WaitForSeconds(1 / stats.startStats.attackSpeed);
            var allEnemyGameObjects = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var gameObject in allEnemyGameObjects)
            {
                _enemyTransformList.Add(gameObject.transform);
            }

            var closestEnemyObject = GetClosestEnemy(_enemyTransformList.ToArray());

            if (closestEnemyObject == null) continue;
            _closestEnemy = closestEnemyObject.GetComponent<Enemy>();
            Projectile spawnedProjectile = Instantiate(projectile, transform.position, Quaternion.identity, transform);
            spawnedProjectile.SetTarget(_closestEnemy);
            spawnedProjectile._towerRange = stats.startStats.range;
            _enemyTransformList.Clear();
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

        if (minDist < stats.startStats.range)
        {
            return closestEnemy;
        }
        else
        {
            return null;
        }
    }

    [Button]
    public void SetMinimumAnts()
    {
        stats.SetMinimumAllocated(manager.antCount.y - manager.antCount.x);
        manager.AllocateAnt(stats.antAllocation.minimumAntsAllocated);
        StartCoroutine(Shoot());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = rangeDrawingColor;

        Gizmos.DrawSphere(transform.position, stats.startStats.range);
    }
}
