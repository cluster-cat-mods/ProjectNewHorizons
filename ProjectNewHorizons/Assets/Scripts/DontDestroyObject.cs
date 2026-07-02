using UnityEngine;

public class DontDestroyObject : MonoBehaviour
{
    [SerializeField] private GameObject dontDestroyObject;
    private bool _objectMade;

    void Start()
    {
        if (!_objectMade)
        {
            GameObject Object = Instantiate(gameObject);
            DontDestroyOnLoad(Object);
            _objectMade = true;
        }
    }
}
