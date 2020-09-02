using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    public class MouseLook : MonoBehaviour
    {
        public float mouseSensitivity = 100f;
        [HideInInspector] public float sensitivityModifier = 1f;

        private Transform fpcBody;

        private float xRotation;

        void Awake()
        {
            fpcBody = transform.parent; 
        }


        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * sensitivityModifier * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * sensitivityModifier * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            fpcBody.Rotate(Vector3.up, mouseX);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);


        }
    }
}

