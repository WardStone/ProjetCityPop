using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UIElements;

namespace Enemy
{
    public class EnemyDetection : MonoBehaviour
    {
        public bool canSee = true;
        public bool canHear = true;

        //for sight
        public float sightSphereRadius;
        public float fieldOfViewAngle;
        public float fieldOfViewDistance;

        //for earing
        public float hearSphereRadius;

        public bool playerIsDetected = false;

        public GameObject player;
        public LayerMask playerLayer;


        private void Awake()
        {
            
        }


        void Start()
        {

        }

        void Update()
        {
            if (!playerIsDetected)
            {
                if (canSee)
                {
                    fieldOfViewCone();
                    fieldOfViewSphere();
                }

                if (canHear)
                {
                    //fonction de l'écoute
                }
               
            }
            
        }


        /// <summary>
        /// check si le joueur est dans le cone de vue de l'ennemi
        /// </summary>
        void fieldOfViewCone()
        {
            Vector3 direction = player.transform.position - transform.position;
            float angle = Vector3.Angle(direction.normalized, transform.forward);

            //On met la moitié de l'angle car on compare avec l'angle en le vecteur joueur-ennemi et celui qui par tout droti devant l'ennemi
            if (angle < fieldOfViewAngle * 0.5f)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, direction.normalized, out hit, fieldOfViewDistance))
                {
                    if (hit.collider.gameObject == player)
                    {
                        playerIsDetected = true;
                    }
                }
            }
        }

        /// <summary>
        /// check si le joueur est dans la sphere autour de l'ennemi
        /// </summary>
        void fieldOfViewSphere()
        {

            Collider[] hitCollider = Physics.OverlapSphere(transform.position, sightSphereRadius, playerLayer);

            if (hitCollider.Length != 0)
            {
                RaycastHit hit;
                Vector3 direction = player.transform.position - transform.position;

                if(Physics.Raycast(transform.position, direction.normalized, out hit))
                {
                    if (hit.collider.gameObject == player)
                    {
                        playerIsDetected = true;
                        
                    }
                }
            }

        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, transform.forward * fieldOfViewDistance);
            Gizmos.DrawWireSphere(transform.position, sightSphereRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, player.transform.position);
        }
    }
}

