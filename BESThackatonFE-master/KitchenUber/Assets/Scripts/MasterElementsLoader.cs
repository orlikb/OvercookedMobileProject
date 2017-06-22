using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterElementsLoader : MonoBehaviour {


    
    void Awake()
    {
        if (FindObjectsOfType<CommonElements>().Length < 1)
        {
            SceneManager.LoadScene("CommonElements", LoadSceneMode.Additive);
        }
    }
    
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
