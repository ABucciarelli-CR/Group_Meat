using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class RightPiecesLifebarMover : MonoBehaviour
{
    public Slider realHealth;
    public GameObject startLifebarPoint;
    public GameObject bloodExplosion;
    private Vector3 startingPosition;

	// Use this for initialization
	void Start ()
    {
        startingPosition = gameObject.transform.localPosition;
    }
    // * realHealth.value
    // Update is called once per frame
    void Update ()
    {
        float toUnit = ((1 / realHealth.maxValue) * realHealth.value);
        //Debug.Log((startingPosition.x - startLifebarPoint.transform.localPosition.x) * toUnit);
        gameObject.transform.localPosition = new Vector2((startingPosition.x - startLifebarPoint.transform.localPosition.x) * toUnit, gameObject.transform.localPosition.y);
	}

    public void SpawnTheBloodyXplosion()
    {
        gameObject.GetComponent<TheAnimator>().enabled = true;
        //Instantiate(bloodExplosion, gameObject.transform);
    }
}
