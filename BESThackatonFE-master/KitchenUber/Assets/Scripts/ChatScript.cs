using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChatScript : MonoBehaviour
{

    public GameObject rateHostWindowGameObject;
    public InputField inputField;
    private bool isAlreadyRated;
    public Text mainChatText;

    public void BackBTN()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void RateHostOnFaceClick()
    {
        if(!isAlreadyRated)
        rateHostWindowGameObject.SetActive(true);
       
    }

    public void ConfirmRate()
    {
        rateHostWindowGameObject.SetActive(false);
        isAlreadyRated = true;
    }

    public void OnClickTextInput()
    {
        inputField.text = String.Empty;
    }
    public void OnTypeEnd()
    {
        if (inputField.text != String.Empty)
        {
            mainChatText.text = "Konrad: " + inputField.text;
            inputField.text = String.Empty;
        }
    }
}
