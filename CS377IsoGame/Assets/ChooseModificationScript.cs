using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseModificationScript : MonoBehaviour
{
    public GameObject chooseModificationPanel;
    public bool isChoosing;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ChooseModificationScript is running.");
        chooseModificationPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update is running.");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("pressed!");
            if (isChoosing)
            {
                stopActivateChoice();
            }
            else
            {
                ActivateChoice();
            }


        }
    }
    //void OnGUI()
    //{
    //    Event e = Event.current;
    //    if (e.type == EventType.KeyDown)
    //    {
    //        if (e.keyCode == KeyCode.Escape)
    //        {
    //            Debug.Log("Escape pressed via OnGUI!");
    //            // You can invoke your methods here
    //            if (isChoosing)
    //            {
    //                stopActivateChoice();
    //            }
    //            else
    //            {
    //                ActivateChoice();
    //            }
    //        }
    //    }
    //}


    public void ActivateChoice()
    {
        chooseModificationPanel.SetActive(true);
        Time.timeScale = 0f;
        isChoosing = true;
    }

    public void stopActivateChoice()
    {
        chooseModificationPanel.SetActive(false);
        Time.timeScale = 1f;
        isChoosing = false;
    }
}
