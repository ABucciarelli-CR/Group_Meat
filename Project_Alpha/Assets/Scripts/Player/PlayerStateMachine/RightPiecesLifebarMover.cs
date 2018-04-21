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
    private Vector3 startingPosition;

	// Use this for initialization
	void Start ()
    {
        startingPosition = gameObject.transform.position;
    }
    // * realHealth.value
    // Update is called once per frame
    void Update ()
    {
        float toUnit = ((1 / realHealth.maxValue) * realHealth.value);
        Debug.Log((startLifebarPoint.transform.position.x - startingPosition.x) * toUnit);
        gameObject.transform.position = new Vector2((startLifebarPoint.transform.position.x - startingPosition.x) * toUnit, gameObject.transform.position.y);
	}
}
