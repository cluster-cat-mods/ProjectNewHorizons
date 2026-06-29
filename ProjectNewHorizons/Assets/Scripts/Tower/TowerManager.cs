using UnityEngine;
using UnityEngine.Events;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private TouchController touchController;
    [SerializeField] private GameManager manager;
    [SerializeField] private GameObject[] towerObject;
    [SerializeField] private TowerStats[] TowerStats;

    [SerializeField] private GameObject towerSelect;
    [SerializeField] private GameObject[] unlockedTowers;

    private RaycastHit _lastHit;
    private bool _ChooseTowerOpen;

    private TowerStats[] _originalStats;
    private UpgradeDataSaver upgradeDataSaver = new();

    void Start()
    {
        if (_originalStats == null)
        {
            _originalStats = new TowerStats[TowerStats.Length];

            for (int i = 0; i < TowerStats.Length; i++)
            {
                if (TowerStats[i] != null)
                {
                    _originalStats[i] = Instantiate(TowerStats[i]);
                }
            }
        }

        if (touchController == null)
        {
            touchController = FindAnyObjectByType<TouchController>();
        }

        if (manager == null)
        {
            manager = FindAnyObjectByType<GameManager>();
        }

        var upgradeDataList = upgradeDataSaver.GetUpgrades();
        if (upgradeDataList != null)
        {
            foreach (var upgrade in upgradeDataList)
            {
                int towerIndex = (int)Mathf.Floor(upgrade.ID / 2) - 1;
                
                switch (upgrade.ID % 2)
                {
                    case 0:
                        if (upgrade.ID < 2 || upgrade.count <= 0) return;
                        TowerStats[towerIndex].startStats.damage = _originalStats[towerIndex].startStats.damage + upgrade.count;
                        break;
                    case 1:
                        if (upgrade.count <= 0) return;
                        TowerStats[towerIndex].startStats.towerUnlocked = true;
                        break;

                }

            }
        }


        for (int i = 0; i < unlockedTowers.Length; i++)
        {
            if (TowerStats[i].startStats.towerUnlocked)
            {
                unlockedTowers[i].SetActive(true);
            }
            else
            {
                unlockedTowers[i].SetActive(false);
            }
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

    public void ChooseTowerUIOpenToggle()
    {
        _ChooseTowerOpen = !_ChooseTowerOpen;
    }

    public void ChooseTower()
    {
        _lastHit = touchController.GetHit();
        if (_lastHit.collider == null) return;
        if (!_lastHit.collider.CompareTag("Mushroom")) return;

        towerSelect.SetActive(true);

        _ChooseTowerOpen = true;
    }

    public void PlaceTower(int towerIndex)
    {
        var cost = TowerStats[towerIndex].antAllocation.minimumAntsAllocated;
        Debug.Log(cost);
        if (manager.antCount.y - manager.antCount.x < cost) return;

        Instantiate(towerObject[towerIndex], _lastHit.collider.transform.position, Quaternion.identity, transform);
        Debug.Log("spawn tower");
        Destroy(_lastHit.collider.gameObject);

        towerSelect.SetActive(false);

        _ChooseTowerOpen = false;
    }
}
