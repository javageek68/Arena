using CircularBuffer;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour {

    public static GuiManager instance = null;

    public int intStatusBufferLines = 5;
    public Text txtStatus;

    private CircularBuffer<string> buffer = null;

    void Awake()
    {
        GuiManager.instance = this;
    }

    // Use this for initialization
    void Start () {
        buffer = new CircularBuffer<string>(intStatusBufferLines);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setStatus(string strStatus)
    {
        Debug.Log(strStatus);
        buffer.PushBack(strStatus);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < buffer.Size; i++)
        {
            sb.Append(string.Format("{0}\r\n", buffer[i]));
        }
        this.txtStatus.text = sb.ToString();
        
    }

    public void StartServerButtonHandler()
    {
        setStatus("server started");
        DiscoveryDirector.instance.startServer();
    }

    public void StartClientButtonHandler()
    {
        setStatus("client started");
        DiscoveryDirector.instance.startClient();
    }
}
