using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Host
{
    public string ip { get; set; }
    public string hostname { get; set; }
    public string macaddress { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="hostname"></param>
    /// <param name="macaddress"></param>
    public Host(string ip, string hostname, string macaddress)
    {
        this.ip = ip;
        this.hostname = hostname;
        this.macaddress = macaddress;
    }

    public override string ToString()
    {
        return string.Format("ip {0}   hostname {1}  macaddress {2}", ip, hostname, macaddress);
    }
}

