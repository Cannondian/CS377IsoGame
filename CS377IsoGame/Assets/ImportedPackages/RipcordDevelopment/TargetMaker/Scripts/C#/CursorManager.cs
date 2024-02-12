using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// /-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\
//
// 						TargetMaker 2.1, Copyright © 2020, Ripcord Development
//											CursorManager.cs
//										   info@ripcorddev.com
//
// \-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/

//ABOUT - This script contains all the rules for the cursor.  Audio and effects related to the cursor are generated from this script.

namespace TargetMaker
{
    [RequireComponent(typeof(AudioSource))]
    public class CursorManager : MonoBehaviour
    {
        public static CursorManager instance;			//A reference to this CursorManager that will be used by other scripts.  Only one can exist at a time.

        [Header("Click Effects")]
        public AudioClip clickAudio;					//The audio that will play when the click is detected
        public GameObject clickMarker;					//The object that will be generated when the mouse is clicked
        public GameObject clickEffect;					//An additional effect that will be generated when the mouse is clicked

        [Header("Click Marker")]
        public int markerLimit = 20;					//The number of click markers that can exist in the scene at any given time.  If set to 0, there will be no limit
        public bool limitMarkers;						//If true, the number of markers on allowed on screen will be limited to the marker limit.  After that, the oldest marker will be removed
        [HideInInspector]
        public List<GameObject> activeMarkers;			//A list of all the click markers currently active in the scene
        public bool destroyOldestMarker;				//If true, any active click marker will be immediately destroyed when a new one is created

        [Space(10)]
        public bool hideMouseCursor;					//If true, hide the system cursor

        public enum AxisPair {XY, YZ, XZ}				//The axes that the target will move along
        public AxisPair axisPair;

        Transform effectContainer;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // AWAKE
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        void Awake ()
        {
            if (!instance)
            {
                instance = this;
            }

            if (!GameObject.Find("EffectContainer"))
            {
                effectContainer = new GameObject("EffectContainer").transform;
            }
            else
            {
                effectContainer = GameObject.Find("EffectContainer").transform;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // START
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        void Start ()
        {
            if (hideMouseCursor)                                                                                                            //If hideMouseCursor is true...
            {
                Cursor.visible = false;							                                                                            //...hide the mouse cursor
            }

            if (markerLimit <= 0) {								                                                                            //If the markerLimit is 0 or less...
                limitMarkers = false;							                                                                            //...there is no limit on the number of click markers that can exist in the scene
            }
            else {												                                                                            //Otherwise...
                limitMarkers = true;							                                                                            //...there is a limit to the number of click markers that canexist in the scene
            }
        }
        
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // UPDATE
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        void Update ()
        {
            if (limitMarkers == false) {						                                                                            //If there is no limit to the number of markers allowed in the scene...
                
                if (Input.GetButtonDown("Fire1") )                                                                                          //...Generate a marker when the left mouse button is pressed
                {
                    GenerateMarker();							
                }
            }
            else                                                                                                                            //Otherwise...
            {
                if (activeMarkers.Count - 1 < markerLimit)                                                                                  //...if the number of active markers is less than the marker limit...
                {        
                    if(Input.GetButtonDown("Fire1") )                                                                                       //......Generate a marker when the left mouse button is pressed
                    {
                        GenerateMarker();
                    }
                }
                else                                                                                                                        //...otherwise is the number of active markers is at or beyond the marker limit...
                {
                    if (destroyOldestMarker)                                                                                                //......if destroy oldest marker is enabled...
                    {
                        GameObject temp = activeMarkers[0];		                                                                            //......create a reference to the oldest marker...
                        activeMarkers.RemoveAt(0);				                                                                            //......remove that marker from the active marker list...
                        Destroy(temp);							                                                                            //......then destroy the oldest marker
                    }
                }
            }

            //Attach this gameObject to the cursor - - - - - - - - - - - - - - - - - - - -
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //If the raycast hits a collider, start updating the hit.point and update the gameObject's position accordingly
            if (Physics.Raycast (ray, out hit))
            { 	
                switch (axisPair)
                {
                    case AxisPair.XY:
                        //Position the target using the X and Y axes of the hit point
                        transform.position = new Vector3(hit.point.x, hit.point.y, transform.position.z);
                        break;

                    case AxisPair.YZ:
                        //Position the target using the Y and Z axes of the hit point
                        transform.position = new Vector3(transform.position.x, hit.point.y, hit.point.z);
                        break;

                    case AxisPair.XZ:
                        //Position the target using the X and Z axes of the hit point
                        transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                        break;
                }
            }
        }
        
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // GENERATE MARKER
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public void GenerateMarker ()
        {
            //If a clickMarker object has been specified, generate it, add it to the list of active click markers in the scene, and give it a unique name
            if (clickMarker)
            {
                GameObject newMarker = (GameObject)Instantiate(clickMarker, gameObject.transform.position, gameObject.transform.rotation);
                newMarker.transform.parent = effectContainer;
                activeMarkers.Add(newMarker);
                newMarker.name += "." + activeMarkers.Count;
            }

            //If a clickAudio clip has been specified, play it
            if (clickAudio)
            {
                gameObject.GetComponent<AudioSource>().Play();
            }

            //If a clickEffect object has been specified, generate it
            if (clickEffect)
            {
                GameObject newEffect = (GameObject)Instantiate(clickEffect, gameObject.transform.position, gameObject.transform.rotation);
                newEffect.transform.parent = effectContainer;
            }
        }
    }
}
