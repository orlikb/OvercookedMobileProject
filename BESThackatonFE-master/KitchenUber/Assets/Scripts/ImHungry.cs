using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class ImHungry : MonoBehaviour
{
    [SerializeField]
    Toggle[] searchToggle; // [0] - Pod spodem [1] - Na gorze <- po zmianie wysyla zapytanie
    [SerializeField]
    Button contactBTN, singInBTN, leftBTN, rightBTN, backBTN;
    [SerializeField]
    InputField inpSearch;
    [SerializeField]
    GameObject offerPrefab;
    [SerializeField]
    GameObject detailedPanel;

    public GameObject offerOrigin;

    private List<GameObject> generatedOffers = new List<GameObject>();
    private int activeOfferNumber = 0;

    void Start()
    {
        searchToggle[0].isOn = true;
        searchToggle[1].isOn = false;
        detailedPanel.SetActive(false);
    }

    void Update()
    {
        leftBTN.gameObject.SetActive(generatedOffers.Count != 0);
        rightBTN.gameObject.SetActive(generatedOffers.Count != 0);
    }
    public void OnChangeToggle(int j)
    {
        if(j ==1 && searchToggle[1].isOn)
        {
            CreateAvlRequest();
        }

        for (int i = 0; i < searchToggle.Length; i++)
        {
            if (j != i)
            {
                searchToggle[i].isOn = !searchToggle[j].isOn;
                searchToggle[i].interactable = true;
            }
            else
            {
                searchToggle[i].interactable = false;
            }
        }
        inpSearch.interactable = !searchToggle[1].isOn;
        if (!inpSearch.interactable)
            inpSearch.text = null;
    }
    public void MoveBTN(bool right)
    {
        activeOfferNumber += right ? 1 : -1;

        if (activeOfferNumber > generatedOffers.Count - 1)
            activeOfferNumber = 0;
        if (activeOfferNumber < 0)
            activeOfferNumber = generatedOffers.Count - 1;

        foreach (GameObject generatedOffer in generatedOffers)
        {
            generatedOffer.SetActive(false);
        }
        generatedOffers[activeOfferNumber].SetActive(true);

    }
    public void BackBTN()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void SingInBTN()
    {

    }
    public void DetailsBTN()
    {
        detailedPanel.SetActive(true);
    }
    public void BackDetBTN()
    {
        detailedPanel.SetActive(false);
    }
    public void SearchInBase()
    {
        CreatSpeceAvlRequest(inpSearch.text);
    }

    public void CreateAvlRequest()
    {
        ImHungryMenuConnector connector = new ImHungryMenuConnector();
        connector.fnConnectResult();
        StartCoroutine(LoadOffersNextFrame());
    }
    public void CreatSpeceAvlRequest(string descriptionTXT)
    {
        ImHungryMenuConnector connector = new ImHungryMenuConnector();
        connector.fnConnectResult(descriptionTXT);
        StartCoroutine(LoadOffersNextFrame());
    }

    public IEnumerator LoadOffersNextFrame()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        CreateOffers();

    }
    public void CreateOffers()
    {
        Vector3 originPosition = offerOrigin.transform.localPosition;
        Transform parentTransform = offerOrigin.transform.parent;
        activeOfferNumber = 0;
        foreach (GameObject generatedOffer in generatedOffers)
        {
            Destroy(generatedOffer);
        }
        generatedOffers.Clear();
        CommonElements common = CommonElements.instance;

        foreach (Connector.JsonData.Room room in common.lastJsonDataWrapper.jsonDatas[0].ROOMS)
        {
            GameObject offer = Instantiate(offerPrefab, originPosition, Quaternion.identity) as GameObject;
            offer.transform.SetParent(parentTransform, false);

            OfferPanel offerPanel = offer.GetComponent<OfferPanel>();
            offerPanel.title.text = room.title;
            offerPanel.actualUsers.text = Random.Range(1, Int32.Parse(room.maxUsr)) + "/" + room.maxUsr;
            offerPanel.userName.text = CommonElements.instance.userNames[Int32.Parse(room.hID)-1];
            offerPanel.backImage.sprite = CommonElements.instance.facesList[Int32.Parse(room.hID)-1];
            offerPanel.description.text = room.descr;
            generatedOffers.Add(offer);
            offer.SetActive(false);
        }
        offerOrigin = generatedOffers[activeOfferNumber];
        generatedOffers[activeOfferNumber].SetActive(true);
    }
    public class ImHungryMenuConnector : Connector
    {
        public string fnConnectResult(string t_descr)
        {
            try
            {
                if (client == null || !client.Connected)
                {
                    client = new TcpClient(NetIP, PORT_NUM);
                }
                client.GetStream().BeginRead(readBuffer, 0, READ_BUFFER_SIZE, new AsyncCallback(DoRead), null);
                SendSpecAvlRequest( t_descr);
                return "Connection Succeeded";
            }
            catch (Exception ex)
            {
                return "Server is not active.  Please start server and try again.      " + ex.ToString();
            }
        }
        public string fnConnectResult()
        {
            try
            {
                if (client == null || !client.Connected)
                {
                    client = new TcpClient(NetIP, PORT_NUM);
                }
                client.GetStream().BeginRead(readBuffer, 0, READ_BUFFER_SIZE, new AsyncCallback(DoRead), null);
                SendAvlRequest();
                return "Connection Succeeded";
            }
            catch (Exception ex)
            {
                return "Server is not active.  Please start server and try again.      " + ex.ToString();
            }
        }
        public void SendSpecAvlRequest(string t_descr)
        {
            string jsonString = JsonUtility.ToJson(new SpecAvlRequest(t_descr));
            SendData(jsonString);
        }
        public void SendAvlRequest()
        {
            string jsonString = JsonUtility.ToJson(new AvlRequest());
            SendData(jsonString);
        }
        public class AvlRequest
        {
           
            public string ID;
            public AvlRequest()
            {
                ID = "AVL";
               
            }
        }
        public class SpecAvlRequest
        {
        
            public string ID;
            public string descr;

            public SpecAvlRequest(string t_descr)
            {
                ID = "SPEC_AVL";
                descr = t_descr;
            }
        }
    }
}
