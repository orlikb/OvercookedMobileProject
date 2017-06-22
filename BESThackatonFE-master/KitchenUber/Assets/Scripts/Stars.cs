using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stars : MonoBehaviour
{

    public List<GameObject> startsList;

    public Sprite greyStar;
    public Sprite goldenStar;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable()
    {
        for (int i = 0; i < 5; i++)
        {
            startsList[i].GetComponent<Image>().sprite = greyStar;
        }
    }

    public void OnSelectStar(int numberOfStars)
    {
        foreach (GameObject o in startsList)
        {
            o.GetComponent<Image>().color = new Color(149 / 255f, 149 / 255f, 149 / 255f, 1);
            o.GetComponent<Image>().sprite = greyStar;
        }
        for (int i = 0; i < numberOfStars; i++)
        {
            startsList[i].GetComponent<Image>().color = Color.white;
            startsList[i].GetComponent<Image>().sprite = goldenStar;

        }
    }

    public void OnClickStar(int numberOfStars)
    {
        foreach (GameObject o in startsList)
        {
            o.GetComponent<Image>().color = new Color(149 / 255f, 149 / 255f, 149 / 255f, 1);
            o.GetComponent<Image>().sprite = greyStar;
        }
        for (int i = 0; i < numberOfStars; i++)
        {
            startsList[i].GetComponent<Image>().color = Color.white;
            startsList[i].GetComponent<Image>().sprite = goldenStar;
           
        }
    }
}
