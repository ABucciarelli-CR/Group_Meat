using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Life : MonoBehaviour
{
    private int maxLife = 100;
        

    public Slider lifeBar;

    private int actualLife = 100;

    private bool waited = false;

    public SpriteRenderer playerSprite;

    private Color playerOffenseStateStandardColor;
    private Color playerOffenseStateDamagedColor;

    private GlobalVariables globalVariables;

    // Use this for initialization
    void Awake()
    {
        playerSprite = GetComponent<SpriteRenderer>();

        playerOffenseStateStandardColor = Color.white;
        playerOffenseStateDamagedColor = Color.red;

        globalVariables = GameObject.Find("GameManager").GetComponent<GlobalVariables>();

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
        if (lifeBar.value > actualLife)
        {
            playerSprite.color = playerOffenseStateDamagedColor;
            StartCoroutine(Wait());
        }
        else if(waited)
        {
            waited = false;
            playerSprite.color = playerOffenseStateStandardColor;
        }

        lifeBar.value = actualLife;

        if (actualLife <= 0)
        {
            //Debug.Log("Sei Morto");
            ResetGlobalVariables();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void ResetGlobalVariables()
    {
        globalVariables.enemyDead = 0;
        globalVariables.closeDoor = false;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(.15f);
        waited = true;
    }
}

