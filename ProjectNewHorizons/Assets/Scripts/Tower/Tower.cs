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

    private List<Transform> _enemyTransformList;
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
            //GameObject enemies = GameObject.FindObjectsOfType<Enemy>().game;
            yield return new WaitForSeconds(1 / stats.startStats.attackSpeed);
            Projectile spawnedProjectile = Instantiate(projectile, transform.forward, Quaternion.identity, transform);
            var allEnemyGameObjects = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var gameObject in allEnemyGameObjects)
            {
                _enemyTransformList.Add(gameObject.transform);
            }
            _closestEnemy = GetClosestEnemy(_enemyTransformList.ToArray()).GetComponent<Enemy>();
            spawnedProjectile.SetTarget(_closestEnemy);
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

    [Button]
    public void SetMinimumAnts()
    {
        stats.SetMinimumAllocated(manager.antCount.y - manager.antCount.x);
        manager.AllocateAnt(stats.antAllocation.minimumAntsAllocated);
        StartCoroutine(Shoot());
    }
}
