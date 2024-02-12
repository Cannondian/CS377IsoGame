using UnityEngine;
using System.Collections;

// /-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\
//
// 						TargetMaker 2.0, Copyright © 2017, Ripcord Development
//											 DestroyEmpty.cs
//										   info@ripcorddev.com
//
// \-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/

//ABOUT - Destroys the container object when it no longer has any child objects.

namespace TargetMaker
{
    public class DestroyEmpty : MonoBehaviour
    {
        void Update ()
        {
            if (transform.childCount == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}