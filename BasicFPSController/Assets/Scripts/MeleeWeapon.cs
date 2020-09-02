using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Enemy;

namespace Item
{
    public class MeleeWeapon : MonoBehaviour
    {

        public int dmg;
        public float knockback;
        public float slowMultiplicator;
        public float dmgMultiplicator;

        public bool canAttack = true;
        public bool isAttacking = false;
        public bool isCheckingForCollider = false;
        private Animator weaponAnimator;

        public bool isInPlayerHand;
        private bool isInEnemyHand;

        public GameObject playerCamera;
        private Collider weaponCollider;


        private void Awake()
        {
            weaponAnimator = GetComponent<Animator>();
            weaponAnimator.enabled = false;
            weaponCollider = GetComponent<CapsuleCollider>();
        }


        void Update()
        {
            if(playerCamera.GetComponent<ObjectInteraction>().selectedObject == this.gameObject)
            {
                isInPlayerHand = true;
            }

            //Quand l'arme est dans les mains du joueur
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
                        weaponAnimator.SetBool("isCharging", true);
                    }
                    else if (Input.GetMouseButtonUp(0))
                    {
                        weaponAnimator.SetBool("isCharging", false);
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

        /// <summary>
        /// For Animator Event, to send signal to other scripts 
        /// </summary>
        public void AttackStateAnimEvent()
        {
            if (isAttacking)
            {
                isAttacking = false;
            }
            else
            {
                isAttacking = true;
            }
        }

        public void DmgCheckAnimEvent()
        {
            if (isCheckingForCollider)
            {
                isCheckingForCollider = false;
            }
            else
            {
                isCheckingForCollider = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isCheckingForCollider)
            {
                if (other.CompareTag("Enemy"))
                {
                    other.GetComponentInParent<EnemyBehavior>().TakeDamage(dmg, knockback);
                }
            }
        }
    }
}

