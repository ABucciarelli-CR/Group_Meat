using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    [RequireComponent(typeof(PlayerMain))]
    public class Life : MonoBehaviour
    {

        public int life = 1000;

        public Slider lifeBar;

        // Use this for initialization
        void Awake()
        {
            //lifeBar = GetComponent<Slider>();
            lifeBar.maxValue = life;
            lifeBar.value = life;

        }
    }
}
