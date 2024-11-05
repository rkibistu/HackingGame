using System.Collections.Generic;
using System;
using UnityEngine;

public class InterpreterWifi : InterpreterBase
{
    [SerializeField]
    private bool _donglePlugged = false;

    public bool DonglePlugged
    {
        get { return _donglePlugged; }
        set { _donglePlugged = value; }
    } 

    private void Start()
    {
        _commandDictionary["iwconfig"] = iwconfig;
        _commandDictionary["hcxdumptool"] = hcxdumptool;
        _commandDictionary["hcxpcapngtool"] = hcxpcapngtool;
        _commandDictionary["hashcat"] = hashcat;
    }

    private List<string> iwconfig(List<string> args)
    {
        _response.Clear();

        if (_donglePlugged)
        {
            _response.Add("lo\t\tno wireless extensions.");
            _response.Add("eth0\t\tno wireless extensions.");
            _response.Add("wlan0\tIEEE 802.11\tESSID:off/any");
            _response.Add("Mode:Managed\tAcces Point: Not-Associated\tTX-Power=20 dBm");
            _response.Add("Retry short limit:7\tRTS thr=2347 B\tFragment thr:off");
            _response.Add("Power Management:off");
        }
        else
        {
            _response.Add("lo\t\tno wireless extensions.");
            _response.Add("eth0\t\tno wireless extensions.");
        }


        return _response;
    }

    private List<string> hcxdumptool(List<string> args)
    {
        _response.Clear();

        if (!_donglePlugged)
        {
            _response.Add("Command not recognized.");
            return _response;
        }

        if (args.Count < 5)
        {
            _response.Add("Wrong number of args.");
            return _response;
        }

        if (args[0] != "-i")
        {
            _response.Add("Wrong args: " + args[0]);
            return _response;
        }
        else if (args[1] != "wlan0")
        {
            _response.Add("Wrong interface: " + args[1]);
            return _response;
        }
        else if (args[2] != "-o")
        {
            _response.Add("Wrong args: " + args[2]);
            return _response;
        }
        else if (args[3] != "dumpfile.pcapng")
        {
            _response.Add("Wrong filename: " + args[3]);
            return _response;
        }
        else if (args[4] != "--active_beacon")
        {
            _response.Add("Wrong args: " + args[4]);
            return _response;
        }

        _response.Add("  CHA   LAST   R 1 3 P S    MAC-AP    ESSID (last seen on top)     SCAN-FREQUENCY:   2412");
        _response.Add("-----------------------------------------------------------------------------------------");
        _response.Add("");
        _response.Add("  1\t09:07:27  + + +   +\t42AF69B00ABA John's WiFi 2.4 GHz");
        _response.Add("  2\t09:39:46  + +     +\tc2252f240c46 halo");
        _response.Add("  7\t09:39:46  + +     +\td8e84483ff7a Orange-7FPkbK-2G");
        _response.Add("");
        _response.Add("  ...........");
        _response.Add("<<<<<<<<sleep(5))>>>>>>>>>");
        _response.Add("1365 Packet(s) captured by kernel");
        _response.Add("214 Packet(s) dropped by kernel");
        _response.Add("1 SHB written to pcapng dumpfile");
        _response.Add("1 IDB written to pcapng dumpfile");
        _response.Add("1 ECB written to pcapng dumpfile");
        _response.Add("146 EPB written to pcapng dumpfile");

        return _response;
    }

    private List<string> hcxpcapngtool(List<string> args)
    {
        _response.Clear();

        if (!_donglePlugged)
        {
            _response.Add("Command not recognized.");
            return _response;
        }

        if (args.Count < 3)
        {
            _response.Add("Wrong number of args.");
            return _response;
        }

        if (args[0] != "-o")
        {
            _response.Add("Wrong args: " + args[0]);
            return _response;
        }
        else if (args[1] != "hash.hc22000")
        {
            _response.Add("Wrong args: " + args[1]);
            return _response;
        }
        else if (args[2] != "dumpfile.pcapng")
        {
            _response.Add("Wrong args: " + args[2]);
            return _response;
        }


        _response.Add("hcxpcapngtool 6.3.4 reading from dumpfile.pcapng...");
        _response.Add("");
        _response.Add("summary capture file");
        _response.Add("--------------------");
        _response.Add("file name................................: dumpfile.pcapng");
        _response.Add("version (pcapng).........................: 1.0");
        _response.Add("operating system.........................: Linux 6.10.11-amd64");
        _response.Add("application..............................: hcxdumptool 6.3.4");
        _response.Add("interface name...........................: wlan0");
        _response.Add("interface vendor.........................: 3c52a1");
        _response.Add("openSSL version..........................: 1.1");
        _response.Add("weak candidate...........................: 12345678");
        _response.Add("MAC ACCESS POINT.........................: 60fcf17c767b (incremented on every new client)");
        _response.Add("MAC CLIENT...............................: a4a6a98b9b9a");
        _response.Add("REPLAYCOUNT..............................: 63283");
        _response.Add("ANONCE...................................: 6100f5dc5bec242e4b42e704e2d5a74eaae003e828809377ca6cf95a14b48945");
        _response.Add("SNONCE...................................: 52d634c7ea223eaa9c0d41272358d809cda0bddd55cc2edf39b371dcafbc82b3");
        _response.Add("timestamp minimum (GMT)..................: 03.11.2024 09:39:44");
        _response.Add("timestamp maximum (GMT)..................: 03.11.2024 09:39:47");
        _response.Add("duration of the dump tool (seconds)......: 2");
        _response.Add("used capture interfaces..................: 1");
        _response.Add("link layer header type...................: DLT_IEEE802_11_RADIO (127)");
        _response.Add("endianness (capture system)..............: little endian");
        _response.Add("packets inside...........................: 146");
        _response.Add("packets received on 2.4 GHz..............: 133");
        _response.Add("ESSID (total unique).....................: 9");
        _response.Add("BEACON (total)...........................: 12");
        _response.Add("BEACON on 2.4 GHz channel (from IE_TAG)..: 1");
        _response.Add("BEACON (SSID wildcard/unset).............: 4");
        _response.Add("ACTION (total)...........................: 1");
        _response.Add("ACTION (containing ESSID)................: 1");
        _response.Add("PROBEREQUEST (undirected)................: 5");
        _response.Add("PROBERESPONSE (total)....................: 6");
        _response.Add("AUTHENTICATION (total)...................: 21");
        _response.Add("AUTHENTICATION (OPEN SYSTEM).............: 21");
        _response.Add("ASSOCIATIONREQUEST (total)...............: 6");
        _response.Add("ASSOCIATIONREQUEST (PSK).................: 6");
        _response.Add("EAPOL messages (total)...................: 95");
        _response.Add("EAPOL RSN messages.......................: 95");
        _response.Add("EAPOLTIME gap (measured maximum msec)....: 804");
        _response.Add("EAPOL ANONCE error corrections (NC)......: not detected");
        _response.Add("EAPOL M1 messages (total)................: 73");
        _response.Add("EAPOL M2 messages (total)................: 16");
        _response.Add("EAPOL M3 messages (total)................: 2");
        _response.Add("EAPOL M4 messages (total)................: 4");
        _response.Add("EAPOL M4 messages (zeroed NONCE).........: 4");
        _response.Add("EAPOL pairs (total)......................: 34");
        _response.Add("EAPOL pairs (best).......................: 6");
        _response.Add("EAPOL ROGUE pairs........................: 5");
        _response.Add("EAPOL pairs written to 22000 hash file...: 6 (RC checked)");
        _response.Add("EAPOL M12E2 (challenge)..................: 6");
        _response.Add("");
        _response.Add("frequency statistics from radiotap header (frequency: received packets)");
        _response.Add("-----------------------------------------------------------------------");
        _response.Add("2412: 133");
        _response.Add("");
        _response.Add("session summary");
        _response.Add("---------------");
        _response.Add("processed pcapng files................: 1");


        return _response;
    }

    private List<string> hashcat(List<string> args)
    {
        _response.Clear();

        if (!_donglePlugged)
        {
            _response.Add("Command not recognized.");
            return _response;
        }

        if (args[0] == "-m")
        {
            if (args[1] != "22000")
            {
                _response.Add("Wrong args: " + args[1]);
                return _response;
            }
            else if (args[2] != "hash.hc22000")
            {
                _response.Add("Wrong args: " + args[2]);
                return _response;
            }
            else if (args[3] != "rockyou.txt")
            {
                _response.Add("Wrong args: " + args[3]);
                return _response;
            }


            _response.Add("hashcat (v6.2.6) starting");
            _response.Add("");
            _response.Add("OpenCL API (OpenCL 3.0 PoCL 6.0+debian  Linux, None+Asserts, RELOC, LLVM 17.0.6, SLEEF, DISTRO, POCL_DEBUG) - Platform #1 [The pocl project]");
            _response.Add("=============================================================");
            _response.Add("* Device #1: cpu-penryn-AMD Ryzen 7 5700X 8-Core Processor, 1438/2941 MB (512 MB allocatable), 4MCU");
            _response.Add("");
            _response.Add("Minimum password length supported by kernel: 8");
            _response.Add("Maximum password length supported by kernel: 63");
            _response.Add("");
            _response.Add("Hashes: 6 digests; 6 unique digests, 3 unique salts");
            _response.Add("Bitmaps: 16 bits, 65536 entries, 0x0000ffff mask, 262144 bytes, 5/13 rotates");
            _response.Add("Rules: 1");
            _response.Add("");
            _response.Add("Optimizers applied:");
            _response.Add("* Zero-Byte");
            _response.Add("* Slow-Hash-SIMD-LOOP");
            _response.Add("");
            _response.Add("Watchdog: Temperature abort trigger set to 90c");
            _response.Add("");
            _response.Add("Initializing backend runtime for device #1. Please be patient...");
            _response.Add("");
            _response.Add("<<<<<<<<sleep(7)>>>>>>>>>");
            _response.Add("");
            _response.Add("Session..........: hashcat");
            _response.Add("Status...........: Quit");
            _response.Add("Hash.Mode........: 22000 (WPA-PBKDF2-PMKID+EAPOL)");
            _response.Add("Hash.Target......: hash.hc22000");
            _response.Add("Time.Started.....: Sun Nov  3 09:44:29 2024 (57 min, 4 secs)");
            _response.Add("Time.Estimated...: Sun Nov  3 10:45:40 2024 (1 hour, 0 mins)");
            _response.Add("Kernel.Feature...: Pure Kernel");
            _response.Add("Guess.Base.......: File (rockyou.txt)");
            _response.Add("Guess.Queue......: 1/1 (100.00%)");
            _response.Add("Speed.#1.........:     7653 H/s (7.69ms) @ Accel:64 Loops:1024 Thr:1 Vec:4");
            _response.Add("Recovered........: 1/3 (33.33%) Digests (total), 2/6 (33.33%) Digests (new), 1/3 (33.33%) Salts");
            _response.Add("Progress.........: 43033155/43033155 (100%)");
            _response.Add("Rejected.........: 905175/1620439 (55.86%)");
            _response.Add("Restore.Point....: 14344385/14344385 (100%)");
            _response.Add("Restore.Sub.#1...: Salt:1 Amplifier:0-1 Iteration:0-1");
            _response.Add("Candidate.Engine.: Device Generator");
            _response.Add("Candidates.#1....: Melissa06 -> LOVEOFMYLIFE");
            _response.Add("Hardware.Mon.#1..: Util: 78%");
            _response.Add("");
            _response.Add("Started: Sun Nov  3 09:43:57 2024");
            _response.Add("Stopped: Sun Nov  3 09:45:35 2024");

            return _response;
        }
        else if (args[0] == "hash.hc22000")
        {
            if (args[1] != "--show")
            {
                _response.Add("Wrong args: " + args[1]);
                return _response;
            }

            _response.Add("22000 | WPA-PBKDF2-PMKID+EAPOL | Network Protocol");
            _response.Add("");
            _response.Add("c78e84dedc416f53f62acccf5f44fd23:42AF69B00ABA:308398897d37:John's WiFi 2.4 GHz:johnissmart");
        }
        else
        {
            _response.Add("Wrong args: " + args[0]);
            return _response;
        }

        return _response;
    }

}
