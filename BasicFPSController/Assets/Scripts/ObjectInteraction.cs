using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.XR;
using Item;
using UnityEditorInternal;

namespace Player
{
    public class ObjectInteraction : MonoBehaviour
    {
        public bool canInteract = true;
        public bool holdToCharge;
        public float interactRange = 10f;
        public LayerMask InteractableLayer;

        [SerializeField] private GameObject raycastObject;
        public Image crosshair;
        public Slider throwPowerSlider;

        public GameObject selectedObject;
        public bool isHolding = false;

        public GameObject handObject;
        public GameObject weaponHandObject;

        public float dragSpeed = 10f;
        public float throwForce = 100f;
        public float torqueForce = 50f;
        [SerializeField] private float throwMultiplicator = 0f;
        public float thRateOfIncrease = 1f;
        public float maxHoldDistance = 10f;

        public bool isThrowing;

        void Start()
        {

        }

        void Update()
        {
            RaycastHit hit;

            //raycast qui par de la caméra et qui va tout droit 
            Vector3 interactRaycast = transform.TransformDirection(Vector3.forward);

            if (Physics.Raycast(transform.position, interactRaycast, out hit, interactRange, InteractableLayer) && !isHolding)
            {
                //register object in front on crosshair
                raycastObject = hit.collider.gameObject;

                //change crosshair color
                crosshair.color = Color.red;

            }
            else
            {
                raycastObject = null;

                //reset crosshair color to normal
                crosshair.color = Color.white;
            }

            
            //quand le joueur appuie sur E, il ramssase l'objet ou le repose s'il en a déjà un dans ses mains
            if(Input.GetButtonDown("Interact") && canInteract)
            {

                if (!isHolding && raycastObject != null)
                {
                    selectedObject = raycastObject;
                    isHolding = true;

                    //désactiver le curseur seulement si l'objet ramasser n'est pas une arme
                    if (raycastObject.CompareTag("Throwable"))
                    {
                        crosshair.enabled = false;
                    }
                    else if (raycastObject.CompareTag("Pickable"))
                    {
                        //ranger l'objet dans l'inventaire et le faire disparaitre
                    }

                    raycastObject = null;
                    
                }
                else if (isHolding)
                {
                    RemoveFromHand();
                }
                
            }

            if (isHolding)
            {

                Rigidbody selectedRb = selectedObject.GetComponent<Rigidbody>();
                float selectedMass = selectedRb.mass;

                if (selectedObject.CompareTag("Throwable"))
                {
                    Vector3 holdDir = (handObject.transform.position - selectedObject.transform.position).normalized;
                    float currentHoldDistance = Vector3.Distance(handObject.transform.position, selectedObject.transform.position);

                    //l'objet se dirige constament sur un point en face de la caméra à une vitesse qui dépend de a vitesse de base * la distance avec les mains / la masse de l'objet
                    selectedRb.velocity = (holdDir * dragSpeed * currentHoldDistance * Time.deltaTime) / selectedMass;

                    //pour que l'objet garde toujours la même rotation
                    selectedObject.transform.LookAt(transform.position);

                    //si l'objet est trop loin du milieu de la caméra, le chara le repose
                    if (currentHoldDistance > maxHoldDistance)
                    {
                        RemoveFromHand();
                        return;
                    }

                }
                else if (selectedObject.CompareTag("MeleeWeapon") || selectedObject.CompareTag("RangedWeapon"))
                {
                    //réduit l'impact du rigidbody tant que l'objet est dans la main du joueur
                    selectedRb.isKinematic = true;

                    //met l'objet dans la main du joueur
                    selectedObject.transform.parent.parent = weaponHandObject.transform;

                    //reset la position et la rotation de l'objet pour qu'il soit au bonne endroit sur l'écran / dans la main
                    selectedObject.transform.localPosition = new Vector3(0, 0, 0);
                    selectedObject.transform.localEulerAngles = new Vector3(0, 0, 0);
                    selectedObject.transform.parent.localPosition = new Vector3(0, 0, 0);
                    selectedObject.transform.parent.localEulerAngles = new Vector3(0, 0, 0);
                }

                #region Unused code to throw object without having to hold the right click button
                if (!holdToCharge)
                {
                    //Si l'objet est une arme, un consommable ou un props jetable, le joueur pourra le jeter
                    if(selectedObject.CompareTag("Throwable"))
                    {
                        //appuyer sur clic gauche pour lancer l'objet en face de soi
                        if (Input.GetMouseButtonDown(1))
                        {
                            //reset la velocité de l'objet pour le throw en face de soi
                            selectedRb.velocity = Vector3.zero;

                            //utilisation du vecteur du raycast d'interaction par qu'il pointe en face du joueur
                            selectedRb.AddForce(interactRaycast * throwForce);

                            //enlève l'objet des mains du character
                            selectedObject = null;
                            isHolding = false;
                            crosshair.enabled = true;
                        }
                    }
                    
                }
                #endregion

                else
                {
                    //Si l'objet est une arme, un consommable ou un props jetable, le joueur pourra le jeter
                    if (selectedObject.CompareTag("Throwable")  || selectedObject.CompareTag("RangedWeapon") || (selectedObject.CompareTag("MeleeWeapon") && !selectedObject.GetComponent<MeleeWeapon>().isAttacking))
                    {
                        //tant que le joueur reste appuyer sur clic droit il charge son lancer
                        if (Input.GetMouseButton(1))
                        {
                            //Fait apparaitre le slider de puissance sur l'écran
                            if (!throwPowerSlider.gameObject.activeSelf)
                            {
                                throwPowerSlider.gameObject.SetActive(true);
                            }

                            isThrowing = true;
                            throwMultiplicator += thRateOfIncrease * Time.deltaTime;
                        }
                        //quand il relache le bouton, l'objet part 
                        else if (Input.GetMouseButtonUp(1))
                        {
                            //Empeche le multiplicateur d'être trop fort ou trop faible
                            if (throwMultiplicator > 1)
                            {
                                throwMultiplicator = 1;
                            }
                            else if (throwMultiplicator < 0.4)
                            {
                                throwMultiplicator = 0.4f;
                            }

                            if (selectedObject.CompareTag("MeleeWeapon") || selectedObject.CompareTag("RangedWeapon"))
                            {
                                selectedRb.isKinematic = false;
                                selectedObject.transform.parent.parent = null;

                                if (selectedObject.CompareTag("MeleeWeapon"))
                                {
                                    selectedRb.GetComponent<MeleeWeapon>().isInPlayerHand = false;
                                }
                                else if (selectedObject.CompareTag("RangedWeapon"))
                                {
                                    selectedRb.GetComponent<RangedWeapon>().isInPlayerHand = false;
                                }
                            }

                            selectedRb.velocity = Vector3.zero;

                            //utilisation du vecteur du raycast d'interaction par qu'il pointe en face du joueur
                            selectedRb.AddForce(interactRaycast * throwForce * throwMultiplicator);

                            if (selectedObject.CompareTag("MeleeWeapon"))
                            {
                                //add rotation if the object is a weapon
                                selectedRb.AddTorque(transform.right * torqueForce);
                            }
                            

                            //enlève l'objet des mains du character
                            RemoveFromHand();
                        }

                        throwPowerSlider.value = throwMultiplicator;
                    }
                    
                }

            }
        }

        public void RemoveFromHand()
        {
            selectedObject = null;
            isHolding = false;
            crosshair.enabled = true;
            throwPowerSlider.gameObject.SetActive(false);
            throwMultiplicator = 0f;
            isThrowing = false;
        }
    }
}

