using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game
{
    public class FpsLimiter : MonoBehaviour
    {
        public int frameRate = 60;

        private void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = frameRate;
        }
    }
}

