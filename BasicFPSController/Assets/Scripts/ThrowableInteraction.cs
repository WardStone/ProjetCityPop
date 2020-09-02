using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using Player;

namespace Item
{
    public class ThrowableInteraction : MonoBehaviour
    {
        public bool canInflictingDmg = true;
        public bool canBreak = true;
        //Pour pas confondre avec l'autre c'est un int mais en vrai ça aurait pu être un bool
        private int canBreakAtStart = 1;
        public GameObject playerCamera;

        public float breakThreshold;
        public int itemDmg;
        private Rigidbody objectRb;
        public GameObject deathParticleEffect;
        private float objectSpeed;


        [HideInInspector] public bool isAlive = true; 

        private void Awake()
        {
            objectRb = GetComponent<Rigidbody>();
            isAlive = true;

            if (canBreak)
            {
                canBreakAtStart = 1;
            }
            else
            {
                canBreakAtStart = 0;
            }
        }


        private void Update()
        {
            if (canBreakAtStart == 1)
            {
                if (playerCamera.GetComponent<ObjectInteraction>().isHolding && playerCamera.GetComponent<ObjectInteraction>().selectedObject == this.gameObject)
                {
                    canBreak = false;
                }
                else
                {
                    canBreak = true;
                }
            }
            

            objectSpeed = objectRb.velocity.magnitude;
        }

        private void OnCollisionEnter(Collision other)
        {
            //Détection des dégats + effet de destruction quand l'objet est lancé sur un autre
            if (objectSpeed >= breakThreshold)
            {
                //check la collision avec l'enemy
                if (other.gameObject.tag == "Enemy")
                {
                    if (canInflictingDmg)
                    {
                        other.gameObject.GetComponentInParent<EnemyBehavior>().TakeDamage(itemDmg, 0f);
                    } 
                }
                ObjectDeath();
            }

            if (other.gameObject.tag == "Throwable")
            {
                if(objectSpeed >= other.gameObject.GetComponent<ThrowableInteraction>().breakThreshold)
                {
                    other.gameObject.GetComponent<ThrowableInteraction>().ObjectDeath();
                }
            }
        }

        private void ObjectDeath()
        {
            if (canBreak)
            {
                Instantiate(deathParticleEffect, transform.position, Quaternion.identity);
                isAlive = false;
                Destroy(gameObject);
            }
        }
    }
}

