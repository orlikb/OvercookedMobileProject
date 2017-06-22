using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonElements : MonoBehaviour {

    public static CommonElements instance;
    public GameObject mainUI;
    public GameObject camera;
    public Image backgroundImage;
    public bool isLoggedIn;

    [HideInInspector]
    public Connector.JsonData lastJsonData;

    [HideInInspector]
    public Connector.JsonDataWrapper lastJsonDataWrapper;
    

    public List<string> userNames = new List<string>();
    public List<Sprite> facesList = new List<Sprite>();
	void Awake() {
        DontDestroyOnLoad(gameObject);

        if (!instance)
            instance = this;



	}
	
	// Update is called once per frame
	void Update () {

	}
    public CommonElements GetInstance()
    {
        if (!instance)
            instance = this;
        return instance;
    }
    public void ChangeBackground(Sprite sprite)
    {
        backgroundImage.sprite = sprite;
    }
}
