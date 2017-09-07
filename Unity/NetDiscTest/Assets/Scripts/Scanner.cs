using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scanner : MonoBehaviour, IPinger
{

    public Text txtStatus;
    private NetworkUtils netUtil = null;
    private Dictionary<string, Host> hosts = null;
    private string strStatus = string.Empty;

    // Use this for initialization
    void Start () {
        hosts = new Dictionary<string, Host>();
        netUtil = new NetworkUtils(this);
    }

    void Update()
    {
        //string strStatus = string.Empty;
        //foreach (KeyValuePair<string, Host> kvpHost in hosts)
        //{
        //    strStatus += kvpHost.Value.ToString() + "\r\n";
        //}
        txtStatus.text = strStatus;
    }

    public void StartScanningButtonHandler()
    {
        netUtil.Ping_all();
    }

    public void PingComplete(Host host)
    {
        if (!hosts.ContainsKey(host.hostname)) hosts.Add(host.hostname, host);
        strStatus += host.ToString() + "\r\n";
    }
}
