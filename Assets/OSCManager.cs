using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Text;
using UnityOSC;

public class OSCManager : MonoBehaviour {
    private OSCServer myServer;

    public string outIP = "127.0.0.1";
    public int inPort = 9998;
    public int bufferSize = 100;

    public GameObject manager;


    string[] targetConvList = new string[]
    {
        "とり01", "とり02",
        "ぞう01", "ぞう02",
        "らいおん01", "らいおん02",
        "きりん01", "きりん02",
    };

    string[] directionConvList = new string[]
    {
        "↖", "↑", "↗", "←", "→", "↙", "↓", "↘"
    };

    Dictionary<Vector2Int, string> posConvList = new Dictionary<Vector2Int, string> {
        { new Vector2Int(0, 3), "A-1"  },
        { new Vector2Int(1, 3), "B-1"  },
        { new Vector2Int(2, 3), "C-1"  },
        { new Vector2Int(0, 2), "A-2"  },
        { new Vector2Int(1, 2), "B-2"  },
        { new Vector2Int(2, 2), "C-2"  },
        { new Vector2Int(0, 1), "A-3"  },
        { new Vector2Int(1, 1), "B-3"  },
        { new Vector2Int(2, 1), "C-3"  },
        { new Vector2Int(0, 0), "A-4"  },
        { new Vector2Int(1, 0), "B-4"  },
        { new Vector2Int(2, 0), "C-4"  },
    };


    // Use this for initialization
    void Start() {
        OSCHandler.Instance.Init();

        myServer = OSCHandler.Instance.CreateServer("myServer", inPort);
        myServer.ReceiveBufferSize = 1024;
        myServer.SleepMilliseconds = 10;
    }

    void Update() {
        for (var i = 0; i < OSCHandler.Instance.packets.Count; i++)
        {
            //Debug.Log(OSCHandler.Instance.packets.Count);
            try
            {
                receivedOSC(OSCHandler.Instance.packets[i]);
            }
            finally
            {
                OSCHandler.Instance.packets.Remove(OSCHandler.Instance.packets[i]);
                i--;
            }
        }
    }

    private void receivedOSC(OSCPacket pckt)
    {
        if (pckt == null) { Debug.Log("Empty packet"); return; }

        Debug.Log("Receive!");

        List<OSCMessage> dataList = new List<OSCMessage>();
        foreach (var data in pckt.Data)
        {
            dataList.Add((OSCMessage)data);
            Debug.Log("Adr:" + ((OSCMessage)data).Address + ", Data:" + ((OSCMessage)data).Data[0].ToString());
        }

        if (dataList[0].Address != "/shogi/msgtype") return;

        int msgtype = int.Parse((string)dataList[0].Data[0]);
        if (msgtype == 0)
        {
            if (dataList[1].Address != "/shogi/playerid") return;
            if (dataList[2].Address != "/shogi/target") return;
            if (dataList[3].Address != "/shogi/direction") return;

            int playerID = int.Parse((string)dataList[1].Data[0]);
            int targetType = int.Parse((string)dataList[2].Data[0]);
            int directionType = int.Parse((string)dataList[3].Data[0]);

            Debug.Log(string.Format("msgtype:{0}, playerID:{1}, target:{2}, direction{3}", msgtype, playerID, targetType, directionType));

            string target = targetConvList[targetType];
            string direction = directionConvList[directionType];

            manager.GetComponent<Manager>().OnMovementDisided(playerID, target, direction);

        } else if (msgtype == 1)
        {
            if (dataList[1].Address != "/shogi/playerid") return;
            if (dataList[2].Address != "/shogi/target") return;
            if (dataList[3].Address != "/shogi/posx") return;
            if (dataList[4].Address != "/shogi/posy") return;

            int playerID = int.Parse((string)dataList[1].Data[0]);
            int targetType = int.Parse((string)dataList[2].Data[0]);
            Vector2Int pos = new Vector2Int(int.Parse((string)dataList[3].Data[0]), int.Parse((string)dataList[4].Data[0]));

            Debug.Log(string.Format("msgtype:{0}, playerID:{1}, target:{2}, pos:{3}", msgtype, playerID, targetType, pos));

            string target = targetConvList[targetType];
            string posStr = posConvList[pos];

            manager.GetComponent<Manager>().OnTegomaUchi(playerID, target, posStr);
        }

    }
}
