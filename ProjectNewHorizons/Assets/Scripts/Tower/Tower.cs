using NaughtyAttributes;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private GameManager manager;
    [SerializeField] private AliveEnemyManager aliveEnemyManager;

    [SerializeField] private TowerStats stats;
    [SerializeField] private Projectile projectile;

    [SerializeField] private bool ShowRange;
    [SerializeField, ShowIf("ShowRange")] private Color rangeDrawingColor = new Color(0,.2f,1,.4f);

    private Enemy _closestEnemy;

    private void Start()
    {
        if (manager == null)
        {
            manager = FindAnyObjectByType<GameManager>();
        }

        if (aliveEnemyManager == null)
        {
            aliveEnemyManager = FindAnyObjectByType<AliveEnemyManager>();
        }

        stats = Instantiate(stats);
        SetMinimumAnts();
    }


    public IEnumerator Shoot()
    {
        while (stats.antAllocation.currentAntsAllocated >= stats.antAllocation.minimumAntsAllocated)
        {
            yield return new WaitForSeconds(1 / stats.startStats.attackSpeed);


            var closestEnemyObject = GetClosestEnemy();

            if (closestEnemyObject == null) continue;
            if (!string.IsNullOrEmpty(stats.startStats.shootSoundPath)) RuntimeManager.PlayOneShot(stats.startStats.shootSoundPath);
            _closestEnemy = closestEnemyObject.GetComponent<Enemy>();
            var spawnOffset = ((_closestEnemy.transform.position - transform.position).normalized * 3);
            spawnOffset.y = 0;
            Projectile spawnedProjectile = Instantiate(projectile, transform.position + spawnOffset, Quaternion.identity, transform);
            spawnedProjectile._hitSoundPath = stats.startStats.hitSoundPath;
            spawnedProjectile.SetTarget(_closestEnemy, stats.startStats.isRangedTower);
            spawnedProjectile._towerRange = stats.startStats.range;
            spawnedProjectile._movementSpeed = stats.startStats.projectileSpeed;
            spawnedProjectile._damage = CalculateDamage(stats.startStats.damage);
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

        var AliveEnemyTransforms = aliveEnemyManager.AliveEnemyTransforms();

        for (int i = aliveEnemyManager.AliveEnemiesCount() - 1; i >= 0; i--)
        {
            Transform t = AliveEnemyTransforms[i];

            if (t == null)
            {
                aliveEnemyManager.RemoveEnemy(t.gameObject);
                continue;
            }

            if (aliveEnemyManager.AliveEnemies()[i].GetComponent<Enemy>().runtimeStats.isFlyingEnemy)
            {
                if (stats.startStats.isRangedTower)
                {
                    float dist = Vector3.Distance(t.position, currentPos);

                    if (dist < minDist)
                    {
                        closestEnemy = t;
                        minDist = dist;
                    }
                }
                else
                {
                    Debug.Log($"tower {gameObject.name} cannot hit flying enemies");
                }
            }
            else
            {
                float dist = Vector3.Distance(t.position, currentPos);

                if (dist < minDist)
                {
                    closestEnemy = t;
                    minDist = dist;
                }
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

    public void setStats(TowerStats runtimeStats)
    {
        stats = runtimeStats;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = rangeDrawingColor;

        Gizmos.DrawSphere(transform.position, stats.startStats.range);
    }
}
