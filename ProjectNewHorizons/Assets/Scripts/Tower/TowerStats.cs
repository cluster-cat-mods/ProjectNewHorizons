using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerStats", menuName = "Scriptable Objects/TowerStats")]
public class TowerStats : ScriptableObject
{
    [SerializeField] public StartStats startStats;

    //Seperat vars
    [Space(30)]

    [SerializeField] public AntAllocation antAllocation;

    public void SetMinimumAllocated(int antsAvailable)
    {
        if (antsAvailable < antAllocation.minimumAntsAllocated) return;
        antAllocation.currentAntsAllocated = antAllocation.minimumAntsAllocated;
    }
    public void IncreaseAntsAllocated(int amount, int antsAvailable)
    {
        if (antsAvailable < amount) return;
        if (antAllocation.currentAntsAllocated + amount > antAllocation.maximumAntsAllocated) return;
        antAllocation.currentAntsAllocated += amount;
    }

    public void DecreaseAntsAllocated(int amount, int antsAvailable)
    {
        if (antsAvailable < amount) return;
        if (antAllocation.currentAntsAllocated - amount < antAllocation.minimumAntsAllocated) return;
        antAllocation.currentAntsAllocated -= amount;
    }
}

[System.Serializable]
public struct StartStats
{
    [SerializeField] private bool pathPlacement;
    [SerializeField] private bool needDamage;
    [SerializeField] private bool AOE_Effect;

    [Space(30)]

    //TowerSpots
    [SerializeField, HideIf("pathPlacement"), AllowNesting] private float attackSpeed;
    [SerializeField, HideIf("pathPlacement"), AllowNesting] private float projectileSpeed;
    [SerializeField, HideIf("pathPlacement"), AllowNesting] private float range;

    //PathSpots
    [SerializeField, ShowIf("pathPlacement"), AllowNesting] private float hp;

    //Damage
    [SerializeField, ShowIf("needDamage"), AllowNesting] private float damage;

    //AOE
    [Space(30)]

    [SerializeField, ShowIf("AOE_Effect"), AllowNesting] private float AOE_Range;
}

[System.Serializable]
public struct AntAllocation
{
    public float minimumAntsAllocated;
    public float currentAntsAllocated;
    public float maximumAntsAllocated;
}
