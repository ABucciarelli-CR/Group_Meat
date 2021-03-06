﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class Life : MonoBehaviour
{
    public int maxLife = 1000;
    public int startLife = 200;

    //[ReadOnly]
    public int actualLife;
    //[ReadOnly]
    public int maxLifeBarValue;
    [ReadOnly]
    public Slider[] lifeBar;
    [ReadOnly]
    public Slider[] secondLifeBar;
    [ReadOnly]
    public Slider[] maxLifeBar;
    [ReadOnly]
    public GameObject rightSideHealthbar;
    
    private bool waited = false;
    [ReadOnly]
    public SpriteRenderer playerSprite;
    [ReadOnly]
    public GameObject body;

    private Color playerOffenseStateStandardColor;
    private Color playerOffenseStateDamagedColor;

    private GlobalVariables globalVariables;
    private bool inDecrementation = false;

    // Use this for initialization
    void Awake()
    {
        //playerSprite = body.GetComponent<SpriteRenderer>();

        actualLife = startLife;

        playerOffenseStateStandardColor = Color.white;
        playerOffenseStateDamagedColor = Color.red;

        globalVariables = GameObject.Find("GameManager").GetComponent<GlobalVariables>();
        
        //Defining the starting life
        lifeBar[0].maxValue = maxLife;
        lifeBar[1].maxValue = maxLife;
        secondLifeBar[0].maxValue = maxLife;
        secondLifeBar[1].maxValue = maxLife;
        maxLifeBar[0].maxValue = maxLife;
        maxLifeBar[1].maxValue = maxLife;

        lifeBar[0].value = startLife;
        lifeBar[1].value = startLife;
        secondLifeBar[0].value = startLife;
        secondLifeBar[1].value = startLife;
        maxLifeBar[0].value = startLife;
        maxLifeBar[1].value = startLife;

        actualLife = startLife;
    }

    public void Damage(int dmg)
    {
        actualLife -= dmg;
        //Debug.Log(actualLife);
    }

    public void Heal(int heal)
    {
        if((actualLife + heal) >= maxLifeBar[0].value)
        {
                
            actualLife = (int)maxLifeBar[0].value;
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
        rightSideHealthbar.GetComponent<RightPiecesLifebarMover>().SpawnTheBloodyXplosion();
        maxLifeBar[0].value += increment;
        maxLifeBar[1].value += increment;
    }

    public void DecrementLife(int decrement)
    {
        maxLifeBar[0].value -= decrement;
        maxLifeBar[1].value -= decrement;
        if (lifeBar[0].value > maxLifeBar[0].value)
        {
            lifeBar[0].value = maxLifeBar[0].value;
            lifeBar[1].value = maxLifeBar[1].value;
        }
    }

    private void Update()
    {
        maxLifeBarValue = (int)maxLifeBar[0].value;

        if (actualLife > maxLifeBar[0].value)
        {
            actualLife = (int)maxLifeBar[0].value;
        }

        if (lifeBar[0].value > actualLife)
        {
            playerSprite.color = playerOffenseStateDamagedColor;
            StartCoroutine(Wait());
        }
        else if(waited)
        {
            waited = false;
            playerSprite.color = playerOffenseStateStandardColor;
        }

        lifeBar[0].value = actualLife;
        lifeBar[1].value = actualLife;

        if (actualLife <= 0)
        {
            //Debug.Log("Sei Morto");
            ResetGlobalVariables();
            GameObject.Find("GameManager").GetComponent<GlobalVariables>().DeathCount++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if(secondLifeBar[0].value > lifeBar[0].value)
        {
            if(!inDecrementation)
            {
                inDecrementation = true;
                StartCoroutine(GraduallyDecrementLife());
            }
        }
    }

    private void ResetGlobalVariables()
    {
        //globalVariables.enemyDead = 0;
        //globalVariables.closeDoor = false;
    }

    IEnumerator GraduallyDecrementLife()
    {
        float incrementItself = .1f;
        for(;secondLifeBar[0].value > lifeBar[0].value; secondLifeBar[0].value -= 1.5f)
        {
            yield return new WaitForSeconds(incrementItself);
            incrementItself -= incrementItself / 3;
        }
        inDecrementation = false;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(.15f);
        waited = true;
    }
}

