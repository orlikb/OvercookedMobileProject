using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

public class LinkSyncSCR : MonoBehaviour
{
    public Connector connector = new Connector();
    string lastMessage;
    //public Transform PlayerCoord;
    string ipAddress = "192.168.140.101";
    int port = 1236;

    void Start()
    {
        //Debug.Log(connector.fnConnectResult(ipAddress, port));
        //if (connector.res != "")
        //{
        //    Debug.Log(connector.res);
        //}

    }
    void Update()
    {
      //  Debug.Log(connector.strMessage);
        //Debug.Log(connector.fnConnectResult(ipAddress, port));
        //if (Input.GetKeyUp(KeyCode.Space))
        //{

        //    //connector.ProcessCommands(connector.tempString);
        //    Debug.Log(connector.fnConnectResult(ipAddress, port));
        //    if (connector.res != "")
        //    {
        //        Debug.Log(connector.res);
        //    }
        //}
        //if (test.strMessage != "JOIN")
        //{
        //    if (test.res != lastMessage)
        //    {
        //        Debug.Log(test.res);
        //        lastMessage = test.res;
        //    }
        //}
      //  test.fnPacketTest(PlayerCoord.position[0] + "," + PlayerCoord.position[1] + "," + PlayerCoord.position[2]);
    }

    void OnApplicationQuit()
    {
        try { connector.fnDisconnect(); }
        catch { }
    }
}