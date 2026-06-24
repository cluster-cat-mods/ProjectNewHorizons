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
        SetMinimumAnts();
        StartCoroutine(AliveEnemyUpdater());
    }

    public IEnumerator AliveEnemyUpdater()
    {
        while (manager.alive)
        {
            var allEnemyGameObjects = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var gameObject in allEnemyGameObjects)
            {
                _enemyTransformList.Add(gameObject.transform);
            }
            yield return null;
        }
    }
    public IEnumerator Shoot()
    {
        while (stats.antAllocation.currentAntsAllocated >= stats.antAllocation.minimumAntsAllocated)
        {
            yield return new WaitForSeconds(1 / stats.startStats.attackSpeed);


            var closestEnemyObject = GetClosestEnemy();

            if (closestEnemyObject == null) continue;
            _closestEnemy = closestEnemyObject.GetComponent<Enemy>();
            Projectile spawnedProjectile = Instantiate(projectile, transform.position, Quaternion.identity, transform);
            spawnedProjectile.SetTarget(_closestEnemy);
            spawnedProjectile._towerRange = stats.startStats.range;
            spawnedProjectile._movementSpeed = stats.startStats.projectileSpeed;
            spawnedProjectile._damage = CalculateDamage(stats.startStats.damage);
            _enemyTransformList.Clear();
        }
    }

    private int CalculateDamage(int startDamage)
    {
        // max ants allocated dmg 200% min ants allocated dmg 100% the rest is steps in between
        var damage = startDamage;
        var antDifference = stats.antAllocation.maximumAntsAllocated - stats.antAllocation.minimumAntsAllocated;
        damage *= (stats.antAllocation.currentAntsAllocated - stats.antAllocation.minimumAntsAllocated) / antDifference + 1;
        return damage;
    }

    Transform GetClosestEnemy()
    {
        Transform closestEnemy = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        for (int i = _enemyTransformList.Count - 1; i >= 0; i--)
        {
            Transform t = _enemyTransformList[i];

            if (t == null)
            {
                _enemyTransformList.RemoveAt(i);
                continue;
            }

            float dist = Vector3.Distance(t.position, currentPos);

            if (dist < minDist)
            {
                closestEnemy = t;
                minDist = dist;
            }


        }

        return minDist < stats.startStats.range ? closestEnemy : null;
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
