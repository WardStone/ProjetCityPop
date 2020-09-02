using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using EZCameraShake;
using TMPro;

namespace Item
{
    public class RangedWeapon : MonoBehaviour
    {
        //Gun Stats
        public int damage;
        public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
        public int magazineSize, bulletPerTap;
        public bool allowButtonHold;
        private int bulletleft, bulletsShot;

        //bools
        private bool shooting, readyToShoot, reloading;

        //References
        public Transform attackPoint;
        public LayerMask interactLayer;
        public GameObject playerCamera;

        //CameraShake
        public float shakeMagnitude, shakeRoughness, shakeFadeInTime, shakeFadeOutTime;

        //Graphics
        public GameObject MuzzleFlashPE, bulletHoleGraphic;
        public TextMeshPro text;



        private Animator weaponAnimator;
        private Collider weaponCollider;

        public bool isInPlayerHand;
        public bool isInEnemyHand;

        public bool canAttack = true;

        private void Awake()
        {
            weaponAnimator = GetComponent<Animator>();
            weaponCollider = GetComponent<Collider>();
            weaponAnimator.enabled = false;

            bulletleft = magazineSize;
            readyToShoot = true;
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
                weaponAnimator.enabled = true;

                //Active la detection d'input
                MyInput();

                //SetText
                //text.SetText(bulletleft +" / "+ magazineSize);
            }
            else
            {
                weaponCollider.isTrigger = false;
                weaponAnimator.enabled = false;
            }
        }

        private void MyInput()
        {
            
            if (allowButtonHold) shooting = Input.GetMouseButton(0);
            else shooting = Input.GetMouseButtonDown(0);

            if (Input.GetKeyDown(KeyCode.R) && bulletleft < magazineSize && !reloading)
                Reload();

            //Shoot
            if (readyToShoot && shooting && !reloading && bulletleft > 0 && !playerCamera.GetComponent<ObjectInteraction>().isThrowing)
            {
                bulletsShot = bulletPerTap;
                Shoot();
            }
        }

        private void Shoot()
        {
            Debug.Log("enter Shoot");
            readyToShoot = false;

            //Spread
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);

            //Raycast
            RaycastHit hit;

            Vector3 shootRaycast = playerCamera.transform.forward + new Vector3(x, y, 0);

            if (Physics.Raycast(playerCamera.transform.position, shootRaycast, out hit, range, interactLayer))
            {
                Debug.Log("raycast du cul");
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    Debug.Log("hit enemy");
                }
            }

            //ShakeCamera
            //CameraShaker.Instance.ShakeOnce(shakeMagnitude, shakeRoughness, shakeFadeInTime, shakeFadeOutTime);

            //Graphics
            Instantiate(bulletHoleGraphic, hit.point, Quaternion.Euler(0, 180, 0));
            Instantiate(MuzzleFlashPE, attackPoint.position, Quaternion.identity);

            bulletleft--;
            bulletsShot--;

            Invoke("ResetShot", timeBetweenShooting);

            Invoke("Shoot", timeBetweenShots);
        }

        private void ResetShot()
        {
            readyToShoot = true;
        }

        private void Reload()
        {
            reloading = true;
            Invoke("ReloadFinished", reloadTime);
        }

        private void ReloadFinished()
        {
            bulletleft = magazineSize;
            reloading = false;
        }
    }
}

