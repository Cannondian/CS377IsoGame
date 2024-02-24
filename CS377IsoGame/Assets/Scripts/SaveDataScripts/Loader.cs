using UnityEngine;

public class Loader : MonoBehaviour
{
    [SerializeField] public GameObject dataManagerPrefab;

    private void Awake()
    {
        if (DataManager.Instance == null)
        {
            Instantiate(dataManagerPrefab);
        }
    }
}
