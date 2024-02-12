using UnityEngine;
using System.Collections;

// /-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\
//
// 						TargetMaker 2.0, Copyright © 2017, Ripcord Development
//											MarkerBehaviour.cs
//										   info@ripcorddev.com
//
// \-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/

//ABOUT - This script controls the basic movement of objects.  It can control the rotation as well as modify the scale.
//		  This script can also make the object shirnk away until it disappears.  This can be based on a timer, or as soon as the object is generated.

namespace TargetMaker
{
    public class MarkerBehaviour : MonoBehaviour
    {
        bool canSpin;									//If true, the object can rotate
        public float spinRate;							//The rate at which the object rotates.  Negative values will cause the object to rotate in the opposite direction

        float currentScale;								//The single value that will control the X, Y and Z scale of the object
        public float initialScale = 1.0f;				//The scale the object will inherit when it is initially generated

        bool canScale;									//If true, the object's scale will pulse up and down
        public float scalePulseAmount = 0.25f;			//How far the object will scale above and below the initialScale
        public float scalePulseSpeed = 3.0f;			//How fast the object will scale above and below the initialScale

        bool isShrinking = false;						//If true the object will gradually shrink down until it disappears
        float shrinkScale;								//The scale of the object as it is shrinking
        float scaleThreshold = 0.01f;					//Once the object starts shrinking, it will be removed from this scene after it scales below this value

        public float markerLifespan = 3.0f;				//The number of time, in seconds, the object will wait before shrinking down to nothing
        public float shrinkRate;						//The speed at which the object will shrink //Numbers above 1 will increase speed, numbers below 1 will decrease speed

        float timer;
        float markerTimer;
        
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // AWAKE
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        void Awake ()
        {
            shrinkScale = initialScale;
            currentScale = initialScale;

            //If a value has been set for the spinRate, allow the object to spin
            if (spinRate != 0)
            {
                canSpin = true;
            }

            //If values have been set for the scalePulse, allow the object to scale
            if (scalePulseAmount != 0 && scalePulseSpeed != 0)
            {
                canScale = true;
            }
        }
        
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // UPDATE
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        void Update ()
        {
            if (markerTimer < markerLifespan)                                                                                               //If the marker timer is less than the lifespan of the marker...
            {
                markerTimer += Time.deltaTime;								                                                                //...increase the marker timer
            }
            else															                                                                //Otherwise...
            {
                isShrinking = true;											                                                                //...set the isShrinking flag true so that the object will start shrinking
                canScale = false;											                                                                //...set the canScale flag false so that the object stops scaling
            }

            //If a value has been set for the spinRate, spin the object
            if (canSpin)
            {
                transform.Rotate(0.0f, (spinRate * Time.deltaTime), 0.0f);
            }

            //If values have been set for the scalePulse, scale the object
            if (canScale)
            {
                currentScale = Mathf.Sin(Time.time * scalePulseSpeed) * (scalePulseAmount) + initialScale;
                gameObject.transform.localScale = Vector3.one * currentScale;
            }

            //If the object is alowd to shrink, shrink the object and destroy it if it gets too small
            if (isShrinking)
            {
                shrinkScale = Mathf.Lerp(shrinkScale, 0.0f, Time.deltaTime * shrinkRate);

                //If the scale of the object is above the minimum scale threshold, keep updating it's scale
                if(shrinkScale > scaleThreshold)
                {
                    gameObject.transform.localScale = Vector3.one * shrinkScale;
                }
                //Once the object is below the specified threshold stop shrinking and destroy the object
                else
                {
                    isShrinking = false;
                    CursorManager.instance.activeMarkers.Remove(gameObject);
                    Destroy(gameObject);
                }
            }
        }
    }
}