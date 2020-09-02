using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PauseMenu : MonoBehaviour
    {
        public float pauseTimescale = 0f;
        public bool canPause = true;
        [SerializeField] private bool isPaused = false;
        public GameObject pauseText;

        private void Update()
        {
            //quand le joueur appuie sur p ou echap, le jeu se met en pause
            if(Input.GetKeyDown(KeyCode.P) && canPause)
            {
                if (!isPaused)
                {
                    Time.timeScale = pauseTimescale;
                    pauseText.SetActive(true);
                    isPaused = true;
                }
                else
                {
                    Time.timeScale = 1f;
                    pauseText.SetActive(false)
;                    isPaused = false;
                }
            }

#if UNITY_EDITOR

            //pour rendre le menu pause moins frustrant dans unity
            if (Input.GetKeyDown(KeyCode.Mouse0) && canPause && isPaused)
            {
                Time.timeScale = 1f;
                isPaused = false;
            }
#endif

        }
    }
}

