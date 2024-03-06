using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMineBuff : MonoBehaviour
{
    public GameObject pickUpEffect;
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
        ConditionState playerCondition = other.GetComponent<ConditionState>();
        if (playerCondition != null)
        {
            playerCondition.SetCondition(StatusConditions.statusList.TailwindBoost, 8);
            
            gameObject.SetActive(false);
        }
    }
}