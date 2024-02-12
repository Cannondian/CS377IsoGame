using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// /-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\
//
// 							TargetMaker 2.1, Copyright © 2020, Ripcord Development
//										  DEMO_PresetCarousel.cs
//										   info@ripcorddev.com
//
// \-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/

//ABOUT - This script is for demonstration purposes only and is not needed for the actual functionality of TargetMaker
//	    - This script selects a cursor object from a list and instances it into the scene, demonstrating some of the cursor design options possible with TargetMaker

namespace TargetMaker
{
    public class DEMO_PresetCarousel : MonoBehaviour
    {
        public List<Transform> cursorPresets;
        float moveSpeed = 0.5f;
        public float originPoint;

        void Awake ()
        {
            originPoint = cursorPresets[cursorPresets.Count - 1].position.x - 5.4f;
        }

        void Update ()
        {
            for (int x = 0; x < cursorPresets.Count; x++)
            {
                if (cursorPresets[x].position.x < 5.4f)
                {
                    cursorPresets[x].Translate(Vector3.right * Time.deltaTime * moveSpeed);

                    if (cursorPresets[x].position.x > 5.4f)
                    {
                        cursorPresets[x].position = new Vector3(cursorPresets[x].position.x + originPoint, cursorPresets[x].position.y, cursorPresets[x].position.z);
                    }
                }
            }
        }
    }
}
