using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private Tower[] tower;
    public int towerIndex;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void PlaceTower()
    {
        RaycastHit hit;
        //Physics.Raycast(Camera.main.ScreenPointToRay(screenPoint), out hit);
        //if (hit.collider.CompareTag("Mushroom"))
        Instantiate(tower[towerIndex]);
    }
}
