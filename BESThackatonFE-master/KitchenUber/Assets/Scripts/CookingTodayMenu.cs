using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CookingTodayMenu : MonoBehaviour
{

    [SerializeField]
    InputField inpTitle, inpSize, inpIntg, inpDetail;
    [SerializeField]
    Button removeBTN, backBTN, publicBTN, rlyAddintgBTN, addBTN, switchBTN;

    string titleTXT, descriptionTXT;
    int sizeTXT, ingCout = 0;
    bool switchBool = false;
    [SerializeField]
    GameObject addNewIngrePanel, intgPanel, detailPanel;
    [SerializeField]
    Transform ingListParent;
    [SerializeField]
    GameObject ingPos;
    Button selectedIngredient = null;


    void Update()
    {
        if (inpDetail.text == "" || inpSize.text == "" || inpTitle.text == "" || ingCout <= 0)
            publicBTN.interactable = false;
        else
            publicBTN.interactable = true;


        if (selectedIngredient != null)
            removeBTN.interactable = true;
        else
            removeBTN.interactable = false;


        if (switchBTN)
        {
            if (inpIntg.text == "")
                rlyAddintgBTN.interactable = false;
            else
                rlyAddintgBTN.interactable = true;
        }
    }
    #region New ingredient
    public void IngerBTN(GameObject btn)
    {
        selectedIngredient = btn.GetComponent<Button>();
    }
    public void IngBTN(bool set)
    {
        addNewIngrePanel.SetActive(set);
        backBTN.interactable = !addNewIngrePanel.activeSelf;
        publicBTN.interactable = !addNewIngrePanel.activeSelf;
        inpSize.interactable = !addNewIngrePanel.activeSelf;
        inpTitle.interactable = !addNewIngrePanel.activeSelf;
    }

    public void RemBTN()
    {
        Destroy(selectedIngredient.gameObject);
        ingCout--;
    }
public void AddNewIng()
{
        var but = (Instantiate(ingPos, ingListParent));
        but.transform.SetParent(ingListParent);
        but.GetComponentInChildren<Text>().text = inpIntg.text;
        inpIntg.text = null;
        but.GetComponent<Button>().onClick.AddListener(() => myButtonDelegate(but));
        IngBTN(false);
        ingCout++;
}
    void myButtonDelegate(GameObject but)
    {
        IngerBTN(but);
    }
    #endregion
    #region MainPanel
    public void SetTitle()
    {
        titleTXT = inpTitle.text;
    }
    public void SetSize()
    {
        string txt = inpSize.text;
        if (int.TryParse(txt, out sizeTXT))
        {
            sizeTXT = int.Parse(txt);
            if (sizeTXT <= 0 || sizeTXT > 255)
            {
                sizeTXT = 4;
                inpSize.text = sizeTXT.ToString();
            }
        }
        else
            inpSize.text = null;
    }
    public void ShowIngPanel()
    {
        addBTN.interactable = true;
        removeBTN.interactable = true;
        intgPanel.SetActive(true);
        detailPanel.SetActive(false);
    }
    public void ShowDetailPanel()
    {
        selectedIngredient = null;
        addBTN.interactable = false;
        removeBTN.interactable = false;
        intgPanel.SetActive(false);
        detailPanel.SetActive(true);
    }
    #endregion
    #region Detail panel
    public void DetailSet()
    {
        descriptionTXT = inpDetail.text;
    }
    #endregion
    #region Lower Panel
    public void CreateRoomBTN()
    {
        print(titleTXT);
        print(sizeTXT);
        print(descriptionTXT);
        CreateRequest();
    }
    public void BackBTN()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void CreateRequest()
    {
        CookingTodayMenuConnector connector = new CookingTodayMenuConnector();
        connector.fnConnectResult(titleTXT, 4.ToString(), descriptionTXT, 3.ToString(), sizeTXT.ToString());
    }
    public class CookingTodayMenuConnector : Connector
    {
        public string fnConnectResult(string t_title, string t_hID, string t_descr, string t_hLVL, string t_maxUsr)
        {
            try
            {
                if (client == null || !client.Connected)
                {
                    client = new TcpClient(NetIP, PORT_NUM);
                }
                client.GetStream().BeginRead(readBuffer, 0, READ_BUFFER_SIZE, new AsyncCallback(DoRead), null);
                SendInvitationRequest(t_title, t_hID, t_descr, t_hLVL, t_maxUsr);
                return "Connection Succeeded";
            }
            catch (Exception ex)
            {
                return "Server is not active.  Please start server and try again.      " + ex.ToString();
            }
        }

        public void SendInvitationRequest(string t_title, string t_hID, string t_descr, string t_hLVL, string t_maxUsr)
        {
            string jsonString = JsonUtility.ToJson(new Invitation(t_title, t_hID, t_descr, t_hLVL, t_maxUsr));
            SendData(jsonString);
        }
        public void SendRoomRequest(string t_rid, string t_hID)
        {
            string jsonString = JsonUtility.ToJson(new RoomRequest(t_rid, t_hID));
            SendData(jsonString);
        }


        public class Invitation
        {
            public string title;
            public string hID;
            public string descr;
            public string hLVL;
            public string maxUsr;
            public string ID;
            public Invitation(string t_title, string t_hID, string t_descr, string t_hLVL, string t_maxUsr)
            {
                ID = "ADD";
                title = t_title;
                hID = t_hID;
                descr = t_descr;
                hLVL = t_hLVL;
                maxUsr = t_maxUsr;
            }
        }

        public class RoomRequest
        {
            public string rID;
            public string usr1;
            public string usr2;
            public string usr3; 
            public string usr4;
            public string usr5;

            public RoomRequest(string t_rid, string t_usr1)
            {
                rID = t_rid;
                usr1 = t_usr1;
            }
        }

    }
#endregion
}
