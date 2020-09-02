using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    public class Flashlight : MonoBehaviour
    {
        public bool canFlashlight;
        public GameObject lightSource;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Flashlight") && canFlashlight)
            {
                if (!lightSource.activeSelf)
                {
                    lightSource.SetActive(true);
                }
                else
                {
                    lightSource.SetActive(false);
                }
                
            }
        }
    }
}

