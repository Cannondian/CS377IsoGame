using UnityEngine;
using System.Collections;

// /-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\
//
// 							TargetMaker 2.0, Copyright © 2017, Ripcord Development
//										  DEMO_CursorSwapper.cs
//										   info@ripcorddev.com
//
// \-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/

//ABOUT - This script is for demonstration purposes only and is not needed for the actual functionality of TargetMaker
//		- This script will cycle backwards or forwards through the various example cursors contained in the demo.

namespace TargetMaker
{
    public class DEMO_CursorSwapper : MonoBehaviour
    {
        public GameObject[] cursors;
        public GameObject activeCursor;
        int cursorIndex;

        void Update ()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) )
            {
                if (cursorIndex < cursors.Length - 1)
                {
                    cursorIndex++;
                }
                else
                {
                    cursorIndex = 0;
                }

                SwapCursor();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) )
            {
                if (cursorIndex > 0)
                {
                    cursorIndex--;
                }
                else
                {
                    cursorIndex = cursors.Length - 1;
                }

                SwapCursor();
            }
        }

        void SwapCursor ()
        {
            GameObject newCursor = (GameObject)Instantiate(cursors[cursorIndex], activeCursor.transform.position, activeCursor.transform.rotation);

            for (int x = 0; x < activeCursor.GetComponent<CursorManager>().activeMarkers.Count; x++)
            {
                Destroy(activeCursor.GetComponent<CursorManager>().activeMarkers[x]);
            }
            Destroy(activeCursor);

            activeCursor = newCursor;
        }
    }
}
