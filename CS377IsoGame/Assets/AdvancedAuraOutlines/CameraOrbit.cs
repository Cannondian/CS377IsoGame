using UnityEngine;
using System.Collections;


namespace AuraOutlines
{
    public class CameraOrbit : MonoBehaviour
    {
        public GameObject obj;
        public float timeToChange = 3f;
        private float cTime = 0;
        private int i = 0;

        public Transform target;
        public Vector3 AddPos;

        public float xRot = 0f;
        public float yRot = 0f;

        public float distance = 5f;
        public float sensitivity = 10f;


        void Update()
        {
            cTime += Time.deltaTime;

            //if(cTime >= timeToChange)
            //{
            //    cTime = 0;
            //    i++;
            //
            //    obj.layer = 6 + i;
            //    foreach (var com in obj.GetComponentsInChildren<Component>())
            //    {
            //        com.gameObject.layer = 6 + i;
            //    }
            //
            //}

            yRot += sensitivity * Time.deltaTime;

            transform.position = target.position + Quaternion.Euler(xRot, yRot, 0f) * (distance * -Vector3.back);
            transform.LookAt(target.position, Vector3.up);

            transform.position += AddPos;
        }
    }
}