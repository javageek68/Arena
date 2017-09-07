using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;



class NetworkUtils
{
    private IPinger ipinger = null;

    public NetworkUtils(IPinger ipinger)
    {
        this.ipinger = ipinger;
    }

    public static string NetworkGateway()
    {
        string ip = null;

        foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (networkInterface.OperationalStatus == OperationalStatus.Up)
            {
                foreach (GatewayIPAddressInformation ipInfo in networkInterface.GetIPProperties().GatewayAddresses)
                {
                    ip = ipInfo.Address.ToString();
                }
            }
        }

        return ip;
    }

    public void Ping_all()
    {
        try
        {
            string gate_ip = NetworkGateway();

            //Extracting and pinging all other ip's.
            string[] array = gate_ip.Split('.');

            for (int i = 2; i <= 255; i++)
            {

                string ping_var = array[0] + "." + array[1] + "." + array[2] + "." + i;

                Debug.Log("pinging " + ping_var);
                //time in milliseconds           
                Ping(ping_var, 4, 4000);

            }
        }
        catch (Exception)
        {
        }

    }

    public void Ping(string host, int attempts, int timeout)
    {
        for (int i = 0; i < attempts; i++)
        {
            new Thread(delegate ()
            {
                try
                {
                    System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
                    ping.PingCompleted += new PingCompletedEventHandler(PingCompleted);
                    ping.SendAsync(host, timeout, host);
                }
                catch
                {
                    // Do nothing and let it try again until the attempts are exausted.
                    // Exceptions are thrown for normal ping failurs like address lookup
                    // failed.  For this reason we are supressing errors.
                }
            }).Start();
        }
    }


    private void PingCompleted(object sender, PingCompletedEventArgs e)
    {
        try
        {
            string ip = (string)e.UserState;
            if (e.Reply != null && e.Reply.Status == IPStatus.Success)
            {
                string hostname = GetHostName(ip);
                string macaddres = GetMacAddress(ip);

                Host host = new Host(ip, hostname, macaddres);
                Debug.Log(host.ToString());
                ipinger.PingComplete(host);
            }
            else
            {
                // MessageBox.Show(e.Reply.Status.ToString());
            }
        }
        catch (Exception )
        {

            
        }
    }

    public string GetHostName(string ipAddress)
    {
        try
        {
            IPHostEntry entry = Dns.GetHostEntry(ipAddress);
            if (entry != null)
            {
                return entry.HostName;
            }
        }
        catch (SocketException)
        {
            // MessageBox.Show(e.Message.ToString());
        }

        return null;
    }

    //Get MAC address
    public string GetMacAddress(string ipAddress)
    {
        string macAddress = string.Empty;
        System.Diagnostics.Process Process = new System.Diagnostics.Process();
        Process.StartInfo.FileName = "arp";
        Process.StartInfo.Arguments = "-a " + ipAddress;
        Process.StartInfo.UseShellExecute = false;
        Process.StartInfo.RedirectStandardOutput = true;
        Process.StartInfo.CreateNoWindow = true;
        Process.Start();
        string strOutput = Process.StandardOutput.ReadToEnd();
        string[] substrings = strOutput.Split('-');
        if (substrings.Length >= 8)
        {
            macAddress = substrings[3].Substring(Math.Max(0, substrings[3].Length - 2))
                        + "-" + substrings[4] + "-" + substrings[5] + "-" + substrings[6]
                        + "-" + substrings[7] + "-"
                        + substrings[8].Substring(0, 2);
            return macAddress;
        }
        else
        {
            return "OWN Machine";
        }
    }

}

