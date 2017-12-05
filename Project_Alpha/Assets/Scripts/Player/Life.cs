using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    [RequireComponent(typeof(PlayerMain))]
    public class Life : MonoBehaviour
    {
        public int maxLife = 1000;
        

        public Slider lifeBar;

        private int actualLife = 1000;

        // Use this for initialization
        void Awake()
        {
            //lifeBar = GetComponent<Slider>();
            lifeBar.maxValue = maxLife;
            lifeBar.value = maxLife;
            actualLife = maxLife;

        }

        public void Damage(int dmg)
        {
            actualLife -= dmg;
            //Debug.Log("Player Damaged");
        }

        public void Heal(int heal)
        {
            if((actualLife + heal) >= maxLife)
            {
                actualLife = maxLife;
            }
            else
            {
                actualLife += heal;
            }
        }

        private void Update()
        {
            if(lifeBar.value > actualLife)
            {
                lifeBar.value = actualLife;
            }
            if(actualLife <= 0)
            {
                Debug.Log("Sei Morto");
            }
        }
    }
}
