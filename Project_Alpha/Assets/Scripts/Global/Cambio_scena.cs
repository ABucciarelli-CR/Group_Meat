using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Cambio_scena : MonoBehaviour
{
	public int id;
	
	public void ChangeScene()
    {
		SceneManager.LoadScene (id, LoadSceneMode.Single);
	}

}