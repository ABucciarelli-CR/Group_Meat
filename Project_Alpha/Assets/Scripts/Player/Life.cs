using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Player
{
    [RequireComponent(typeof(PlayerMain))]
    public class Life : MonoBehaviour
    {
        private int maxLife = 100;
        

        public Slider lifeBar;

        private int actualLife = 100;

        public SpriteRenderer playerSprite;

        private Color playerOffenseStateStandardColor;
        private Color playerOffenseStateDamagedColor;

        // Use this for initialization
        void Awake()
        {
            playerSprite = GetComponent<SpriteRenderer>();

            playerOffenseStateStandardColor = Color.white;
            playerOffenseStateDamagedColor = Color.red;

            //lifeBar = GetComponent<Slider>();
            lifeBar.maxValue = maxLife;
            lifeBar.value = maxLife;
            actualLife = maxLife;

        }

        public void Damage(int dmg)
        {
            actualLife -= dmg;
            //Debug.Log(actualLife);
        }

        public void Heal(int heal)
        {
            if((actualLife + heal) >= maxLife)
            {
                
                actualLife = maxLife;
                //Debug.Log(actualLife);
            }
            else
            {
                actualLife += heal;
                //Debug.Log(actualLife);
            }
        }

        private void Update()
        {
            lifeBar.value = actualLife;

            if (lifeBar.value > actualLife)
            {
                playerSprite.color = playerOffenseStateDamagedColor;
                
            }
            else
            {
                playerSprite.color = playerOffenseStateStandardColor;
            }
            if(actualLife <= 0)
            {
                //Debug.Log("Sei Morto");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
