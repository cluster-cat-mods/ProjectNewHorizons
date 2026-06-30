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
    private TowerStats[] _runtimeStats;
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

        if (_runtimeStats == null)
        {
            _runtimeStats = new TowerStats[TowerStats.Length];

            for (int i = 0; i < TowerStats.Length; i++)
            {
                if (TowerStats[i] != null)
                {
                    _runtimeStats[i] = Instantiate(TowerStats[i]);
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

        SetUpgradeValues();
    }


    void Update()
    {
        if (_ChooseTowerOpen) return;
        if (Input.touchCount == 1)
        {
            ChooseTower();
        }
    }

    public void SetUpgradeValues()
    {
        var upgradeDataList = upgradeDataSaver.GetUpgrades();
        if (upgradeDataList != null)
        {
            foreach (var upgrade in upgradeDataList)
            {
                Debug.Log(upgrade.ToString());

                int towerIndex = (int) Mathf.Floor(upgrade.ID / 2f - .5f);

                switch (upgrade.ID % 2)
                {
                    case 0:
                        if (upgrade.ID < 2 || upgrade.count <= 0) continue;
                        _runtimeStats[towerIndex].startStats.damage = _originalStats[towerIndex].startStats.damage + upgrade.count;
                        Debug.Log($"tower {towerObject[towerIndex].name} now has {_originalStats[towerIndex].startStats.damage + upgrade.count} damage");
                        break;
                    case 1:
                        if (upgrade.count <= 0) continue;
                        _runtimeStats[towerIndex].startStats.towerUnlocked = true;
                        break;

                }

            }
        }


        for (int i = 0; i < unlockedTowers.Length; i++)
        {
            if (_runtimeStats[i].startStats.towerUnlocked)
            {
                unlockedTowers[i].SetActive(true);
            }
            else
            {
                unlockedTowers[i].SetActive(false);
            }
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
        var cost = _runtimeStats[towerIndex].antAllocation.minimumAntsAllocated;
        Debug.Log(cost);
        if (manager.antCount.y - manager.antCount.x < cost) return;

        Tower tower = Instantiate(towerObject[towerIndex], _lastHit.collider.transform.position, Quaternion.identity, transform).GetComponentInChildren<Tower>();
        tower.setStats(_runtimeStats[towerIndex]);

        Debug.Log("spawn tower");
        Destroy(_lastHit.collider.gameObject);

        towerSelect.SetActive(false);

        _ChooseTowerOpen = false;
    }
}
