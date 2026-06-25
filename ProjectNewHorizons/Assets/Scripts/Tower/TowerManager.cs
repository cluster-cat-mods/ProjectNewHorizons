using UnityEngine;
using UnityEngine.Events;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private TouchController touchController;
    [SerializeField] private GameManager manager;
    [SerializeField] private GameObject[] towerObject;
    [SerializeField] private TowerStats[] towerStats;

    [SerializeField] private GameObject towerSelect;

    private RaycastHit _lastHit;
    private bool _ChooseTowerOpen;

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
        if (_ChooseTowerOpen) return;
        if (Input.touchCount == 1)
        {
            ChooseTower();
        }
    }

    public void ChooseTower()
    {
        _lastHit = touchController.GetHit();

        if (!_lastHit.collider.CompareTag("Mushroom")) return;

        towerSelect.SetActive(true);

        _ChooseTowerOpen = true;
    }

    public void PlaceTower(int towerIndex)
    {
        var cost = towerStats[towerIndex].antAllocation.minimumAntsAllocated;
        Debug.Log(cost);
        if (manager.antCount.y - manager.antCount.x < cost) return;

        Instantiate(towerObject[towerIndex], _lastHit.collider.transform.position, Quaternion.identity, transform);
        Debug.Log("spawn tower");
        Destroy(_lastHit.collider.gameObject);

        towerSelect.SetActive(false);

        _ChooseTowerOpen = false;
    }
}
