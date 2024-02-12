using UnityEngine;
using System.Collections;

// /-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\
//
// 						TargetMaker 2.0, Copyright © 2017, Ripcord Development
//											     Arrow.cs
//										   info@ripcorddev.com
//
// \-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/

//ABOUT - This script controls the speed and life span of the target arrows.

namespace TargetMaker
{
    public class Arrow : MonoBehaviour
    {
        public float textureSpeed;		//How fast the texture will move across the object
        public float lifeSpan;			//The time, in seconds, it takes before the object is deleted from the scene
        
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // UPDATE
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        void Update ()
        {
            Vector2 textureOffset = new Vector2(Mathf.Lerp(GetComponent<Renderer>().material.mainTextureOffset.x, -2.0f, Time.deltaTime * textureSpeed), 0.0f);
            gameObject.GetComponent<Renderer>().material.mainTextureOffset = textureOffset;
            Destroy(gameObject, lifeSpan);
        }
    }
}
