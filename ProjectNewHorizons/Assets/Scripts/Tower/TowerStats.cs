using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerStats", menuName = "Scriptable Objects/TowerStats")]
public class TowerStats : ScriptableObject
{
    public TowerStartStats startStats;

    //Seperat vars
    [Space(30)]

    public AntAllocation antAllocation;

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
public struct TowerStartStats
{
    [SerializeField] private bool pathPlacement;
    [SerializeField] private bool needDamage;
    [SerializeField] private bool AOE_Effect;

    [Space(30)]

    //TowerSpots
    [HideIf("pathPlacement"), AllowNesting] public float attackSpeed;
    [HideIf("pathPlacement"), AllowNesting] public float projectileSpeed;
    [HideIf("pathPlacement"), AllowNesting] public float range;

    //PathSpots
    [ShowIf("pathPlacement"), AllowNesting] public int hp;

    //Damage
    [ShowIf("needDamage"), AllowNesting] public int damage;

    //AOE
    [Space(30)]

    [ShowIf("AOE_Effect"), AllowNesting] public float AOE_Range;
}

[System.Serializable]
public struct AntAllocation
{
    public int minimumAntsAllocated;
    public int currentAntsAllocated;
    public int maximumAntsAllocated;
}
