using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class DiscoveryDirector : NetworkDiscovery
{

    public static DiscoveryDirector instance = null;

    private int minPort = 10000;
    private int maxPort = 10010;
    private int defaultPort = 10000;
    private bool serverStarted = false;

    // Use this for initialization
    void Start () {
        DiscoveryDirector.instance = this;
        Application.runInBackground = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void startClient()
    {
        GuiManager.instance.setStatus("calling Initialize()");
        Initialize();
        GuiManager.instance.setStatus("callig StartAsClient()");
        StartAsClient();
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        string strStatus = string.Format("Server Found   address {0}   data {1}", fromAddress, data);
        GuiManager.instance.setStatus(strStatus);
    }

    //Call to create a server
    public void startServer()
    {
        if (serverStarted) return;
        serverStarted = true;
        int serverPort = createServer();
        if (serverPort != -1)
        {
            GuiManager.instance.setStatus("Server created on port : " + serverPort);
            broadcastData = serverPort.ToString();
            GuiManager.instance.setStatus("calling Initialize()");
            Initialize();
            GuiManager.instance.setStatus("calling StartAsServer()");
            StartAsServer();
        }
        else
        {
            GuiManager.instance.setStatus("Failed to create Server");
        }
    }


    //Creates a server then returns the port the server is created with. Returns -1 if server is not created
    private int createServer()
    {
        int serverPort = -1;
        //Connect to default port
        bool serverCreated = NetworkServer.Listen(defaultPort);
        if (serverCreated)
        {
            serverPort = defaultPort;
            GuiManager.instance.setStatus("Server Created with deafault port");
        }
        else
        {
            GuiManager.instance.setStatus("Failed to create with the default port");
            //Try to create server with other port from min to max except the default port which we trid already
            for (int tempPort = minPort; tempPort <= maxPort; tempPort++)
            {
                //Skip the default port since we have already tried it
                if (tempPort != defaultPort)
                {
                    //Exit loop if successfully create a server
                    if (NetworkServer.Listen(tempPort))
                    {
                        serverPort = tempPort;
                        break;
                    }

                    //If this is the max port and server is not still created, show, failed to create server error
                    if (tempPort == maxPort)
                    {
                        GuiManager.instance.setStatus("Failed to create server");
                    }
                }
            }
        }
        return serverPort;
    }

}
