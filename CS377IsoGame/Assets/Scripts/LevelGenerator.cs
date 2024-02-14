using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public int levelGenerate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        //levelGenerate = Random.Range(0, 4);
        Debug.Log("Loading Level " + levelGenerate);
        SceneManager.LoadScene(levelGenerate);

    }
}
