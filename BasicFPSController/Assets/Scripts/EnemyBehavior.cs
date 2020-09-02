using Packages.Rider.Editor.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyBehavior : MonoBehaviour
    {
        public int maxLife = 10;
        [SerializeField] private int currentLife;

        private Rigidbody enemyRb;
        public GameObject player;
        public float throwableTakeDmgSpeed;

        public Material baseMaterial;
        public Material hitMaterial;

        private void Awake()
        {
            currentLife = maxLife;
            enemyRb = GetComponent<Rigidbody>();
        }

        void Start()
        {

        }

        void Update()
        {

        }

        public void TakeDamage(int dmg, float knockback)
        {
            if (currentLife > 0)
            {
                currentLife -= dmg;

                StartCoroutine(HitFrames());

                Vector3 vecDir = (transform.position - player.transform.position).normalized;
                enemyRb.AddForce(vecDir * knockback, ForceMode.Impulse);
            }
            //ce qu'il ce passe quand l'ennemi meurt 
            else if(currentLife <= 0)
            {
                GetComponent<EnemyMovement>().enabled = false;
                GetComponent<EnemyDetection>().enabled = false;
                GetComponentInChildren<Animator>().enabled = false;
                //GetComponent<EnemyBehavior>().enabled = false;
            }

        }

        /// <summary>
        /// Change the material brefiely - hit feedback - called in TakeDamage
        /// </summary>
        /// <returns></returns>
        private IEnumerator HitFrames()
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material = hitMaterial;
            transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = hitMaterial;

            yield return new WaitForSecondsRealtime(0.2f);

            transform.GetChild(0).GetComponent<MeshRenderer>().material = baseMaterial;
            transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = baseMaterial;
        }
    }
}

