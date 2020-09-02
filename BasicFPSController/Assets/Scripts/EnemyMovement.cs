using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        private Rigidbody enemyRb;

        public float speed = 30f;
        public float life = 100f;

        public GameObject player;
        public GameObject playerCamera;
        public GameObject enemyHead;


        private void Awake()
        {
            enemyRb = GetComponent<Rigidbody>();
        }

        private void Update()
        {

            if (GetComponent<EnemyDetection>().playerIsDetected)
            {
                Vector3 vecDir = (player.transform.position - transform.position).normalized;

                enemyRb.AddForce(new Vector3(vecDir.x, 0, vecDir.z) * speed, ForceMode.Force);

                transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
                enemyHead.transform.LookAt(playerCamera.transform.position);

                //a voir plus tard si ça c'est pas mieux
                //enemyRb.velocity = new Vector3(vecDir.x, enemyRb.velocity.y, vecDir.z);
            }
            else
            {
                //transform.LookAt(transform.forward);
            }
        }

    }
}

