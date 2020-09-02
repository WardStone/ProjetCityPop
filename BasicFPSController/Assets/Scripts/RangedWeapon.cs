using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Item
{
    public class RangedWeapon : MonoBehaviour
    {
        private Animator weaponAnimator;
        private Collider weaponCollider;

        public bool isInPlayerHand;
        public bool isInEnemyHand;

        public bool canAttack = true;
        public bool isReloading = false;

        public GameObject playerCamera;

        public float hitDistance;
        public LayerMask interactLayer;

        private void Awake()
        {
            weaponAnimator = GetComponent<Animator>();
            weaponCollider = GetComponent<Collider>();
            weaponAnimator.enabled = false;
        }

        private void Update()
        {
            if (playerCamera.GetComponent<ObjectInteraction>().selectedObject == this.gameObject)
            {
                isInPlayerHand = true;
            }

            if (isInPlayerHand)
            {
                weaponCollider.isTrigger = true;

                //si l'animator est désactiver (c'est le cas quand l'objet est au sol), active l'animator 
                if (!weaponAnimator.enabled)
                {
                    weaponAnimator.enabled = true;
                }

                //check si le joueur est en train de jeter l'objet
                if (!playerCamera.GetComponent<ObjectInteraction>().isThrowing)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        RaycastHit hit;

                        Vector3 shootRaycast = transform.TransformDirection(Vector3.forward);

                        if (Physics.Raycast(transform.position, shootRaycast, out hit, hitDistance, interactLayer))
                        {
                            if (hit.collider.gameObject.CompareTag("Enemy"))
                            {
                                Debug.Log("hit enemy");
                            }
                            else if (hit.collider.gameObject.CompareTag("Ground"))
                            {
                                Debug.Log("hit enviro");
                            }
                        }
                    }
                    
                }
            }
            else
            {
                weaponCollider.isTrigger = false;

                if (weaponAnimator.enabled)
                {
                    weaponAnimator.enabled = false;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, transform.TransformDirection(Vector3.forward));
        }
    }
}

