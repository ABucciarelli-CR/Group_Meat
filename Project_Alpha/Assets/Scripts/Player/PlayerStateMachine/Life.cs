using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Life : MonoBehaviour
{
    private int maxLife = 1000;
    private int startLife = 100;
        

    public Slider lifeBar;
    public Slider maxLifeBar;

    private int actualLife;

    private bool waited = false;

    public SpriteRenderer playerSprite;
    public GameObject body;

    private Color playerOffenseStateStandardColor;
    private Color playerOffenseStateDamagedColor;

    private GlobalVariables globalVariables;

    // Use this for initialization
    void Awake()
    {
        //playerSprite = body.GetComponent<SpriteRenderer>();

        actualLife = startLife;

        playerOffenseStateStandardColor = Color.white;
        playerOffenseStateDamagedColor = Color.red;

        globalVariables = GameObject.Find("GameManager").GetComponent<GlobalVariables>();
        
        //Defining the starting life
        lifeBar.maxValue = maxLife;
        maxLifeBar.maxValue = maxLife;

        lifeBar.value = startLife;
        maxLifeBar.value = startLife;
        actualLife = startLife;

    }

    public void Damage(int dmg)
    {
        actualLife -= dmg;
        //Debug.Log(actualLife);
    }

    public void Heal(int heal)
    {
        if((actualLife + heal) >= maxLifeBar.value)
        {
                
            actualLife = (int)maxLifeBar.value;
            //Debug.Log(actualLife);
        }
        else
        {
            actualLife += heal;
            //Debug.Log(actualLife);
        }
    }

    public void IncrementLife(int increment)
    {
        maxLifeBar.value += increment;
    }

    public void DecrementLife(int decrement)
    {
        maxLifeBar.value -= decrement;
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

