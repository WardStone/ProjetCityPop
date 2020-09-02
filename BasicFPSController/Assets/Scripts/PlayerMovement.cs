using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private CharacterController fpController;
        public GameObject controllerCamera;
        public float baseSpeed = 12f;
        public float sprintSpeed = 18f;
        public float sprintMouseSensitivity = 0.6f;
        public float crouchSpeed = 6f;
        [SerializeField] private float effectiveSpeed;

        [SerializeField] private Vector3 currentVelocity;
        public float gravity = -9.81f;

        public Transform groundCheck;
        public float groundDistance = 0.4f;
        public LayerMask groundMask;
        [SerializeField] private bool isGrounded = false;
        [SerializeField] private bool isCrouch = false;
        [SerializeField] private bool isSprinting = false;
        [SerializeField] private bool isHittingHead = false;
        public float crouchSafeDistance = 0.4f;

        public float jumpHeight = 3f;
        private float baseHeight;
        public Animator playerAnimator;

        public bool canJump = true;
        public bool canCrouch = true;
        public bool canSprint = true;

        

        void Awake()
        {
            fpController = GetComponent<CharacterController>();
            baseHeight = fpController.height;
            //groundCheck = transform.GetChild(2);
        }

        void Update()
        {
            //check si le controller touche le sol
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            isHittingHead = Physics.CheckSphere(controllerCamera.transform.position, crouchSafeDistance, groundMask);
            //isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundDistance, groundMask);

            if (isGrounded && currentVelocity.y < 0)
            {
                currentVelocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            //set la vitesse du joueur selon ses actions ainsi que sa taille quand il s'accroupi
            if (Input.GetButton("Sprint") && canSprint && z > 0 && !isCrouch /*&& isGrounded*/)
            {
                effectiveSpeed = sprintSpeed;
                controllerCamera.GetComponent<MouseLook>().sensitivityModifier = sprintMouseSensitivity;
                isSprinting = true;
            }
            else if (Input.GetButton("Crouch") && canCrouch && isGrounded || isHittingHead && canCrouch && isGrounded && isCrouch)
            {
                effectiveSpeed = crouchSpeed;

                fpController.height = 1.35f;
                isCrouch = true;

                // check if there's something above player head when he's crouching - if yes = stay crouch
            }
            else
            {
                effectiveSpeed = baseSpeed;
                controllerCamera.GetComponent<MouseLook>().sensitivityModifier = 1f;

                fpController.height = baseHeight;
                isCrouch = false;
                isSprinting = false;
            }

            fpController.Move(move * effectiveSpeed * Time.deltaTime);

            if(Input.GetButtonDown("Jump") && isGrounded && canJump && !isCrouch)
            {
                currentVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }

            currentVelocity.y += gravity * Time.deltaTime;

            fpController.Move(currentVelocity * Time.deltaTime);

            //Si la tête de l'avatar touche le plafond pendant le saut, il retombe directement sans finir son saut
            if (isHittingHead && !isGrounded)
            {
                if (currentVelocity.y > 0)
                {
                    currentVelocity.y = 0f;
                }
            }

            

        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            //groundcheck gizmos
            Gizmos.DrawWireSphere(new Vector3(groundCheck.position.x, groundCheck.position.y, groundCheck.position.z), groundDistance);

            Gizmos.color = Color.green;
            //safe crouch gizmos
            Gizmos.DrawWireSphere(new Vector3(controllerCamera.transform.position.x, controllerCamera.transform.position.y, controllerCamera.transform.position.z), crouchSafeDistance);

        }
    }


    
}

