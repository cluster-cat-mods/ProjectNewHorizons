using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private TouchController touchController;
    [SerializeField] private GameManager manager;
    [SerializeField] private GameObject[] towerObject;
    [SerializeField] private TowerStats[] towerStats;
    public int towerIndex;

    void Start()
    {
        if (touchController == null)
        {
            touchController = FindAnyObjectByType<TouchController>();
        }

        if (manager == null)
        {
            manager = FindAnyObjectByType<GameManager>();
        }
    }


    void Update()
    {
        if (Input.touchCount == 1)
        {
            PlaceTower();
        }
    }

    public void PlaceTower()
    {
        RaycastHit hit = touchController.GetHit();

        if (!hit.collider.CompareTag("Mushroom")) return;
        var cost = towerStats[towerIndex].antAllocation.minimumAntsAllocated;
        Debug.Log(cost);
        if (manager.antCount.y - manager.antCount.x < cost) return;

        Instantiate(towerObject[towerIndex], hit.collider.transform.position, Quaternion.identity, transform);
        Debug.Log("spawn tower");
        Destroy(hit.collider.gameObject);
    }
}
