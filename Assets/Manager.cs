using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

    public GameObject Field;
    public GameObject[] MochigomaFields = new GameObject[2];
    public GameObject Canvas;

    GameObject WinnerText;

    Dictionary<string, GameObject>[] KomaLists = new Dictionary<string, GameObject>[2];
    Dictionary<string, GameObject>[] MochigomaLists = new Dictionary<string, GameObject>[2];

    Dictionary<string, Vector2Int> UchiPosList = new Dictionary<string, Vector2Int> {
            { "A-1", new Vector2Int(0, 3) },
            { "B-1", new Vector2Int(1, 3) },
            { "C-1", new Vector2Int(2, 3) },
            { "A-2", new Vector2Int(0, 2) },
            { "B-2", new Vector2Int(1, 2) },
            { "C-2", new Vector2Int(2, 2) },
            { "A-3", new Vector2Int(0, 1) },
            { "B-3", new Vector2Int(1, 1) },
            { "C-3", new Vector2Int(2, 1) },
            { "A-4", new Vector2Int(0, 0) },
            { "B-4", new Vector2Int(1, 0) },
            { "C-4", new Vector2Int(2, 0) },
        };

    // Use this for initialization
    void Start () {
        KomaLists[0] = new Dictionary<string, GameObject>();
        KomaLists[0].Add("とり01", Koma.Generate(Koma.Kind.Chick, new Vector2Int(1, 1), Koma.Direction.Up, Field));
        KomaLists[0].Add("ぞう01", Koma.Generate(Koma.Kind.Elephant, new Vector2Int(0, 0), Koma.Direction.Up, Field));
        KomaLists[0].Add("らいおん01", Koma.Generate(Koma.Kind.Lion, new Vector2Int(1, 0), Koma.Direction.Up, Field));
        KomaLists[0].Add("きりん01", Koma.Generate(Koma.Kind.Kirin, new Vector2Int(2, 0), Koma.Direction.Up, Field));

        KomaLists[1] = new Dictionary<string, GameObject>();
        KomaLists[1].Add("とり02", Koma.Generate(Koma.Kind.Chick, new Vector2Int(1, 2), Koma.Direction.Down, Field));
        KomaLists[1].Add("ぞう02", Koma.Generate(Koma.Kind.Elephant, new Vector2Int(2, 3), Koma.Direction.Down, Field));
        KomaLists[1].Add("らいおん02", Koma.Generate(Koma.Kind.Lion, new Vector2Int(1, 3), Koma.Direction.Down, Field));
        KomaLists[1].Add("きりん02", Koma.Generate(Koma.Kind.Kirin, new Vector2Int(0, 3), Koma.Direction.Down, Field));

        MochigomaLists[0] = new Dictionary<string, GameObject>();
        MochigomaLists[1] = new Dictionary<string, GameObject>();

        WinnerText = Canvas.transform.Find("WinnerText").gameObject;
        WinnerText.active = false;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public List<string> GetKomaList(int playerID)
    {
        var list = new List<string>();
        foreach (var pair in KomaLists[playerID])
        {
            list.Add(pair.Key);
        }

        return list;
    }

    public List<string> GetMochigomaList(int playerID)
    {
        var list = new List<string>();
        foreach (var pair in MochigomaLists[playerID])
        {
            list.Add(pair.Key);
        }
        return list;
    }

    public List<string> GetCanMovePositions(int playerID, string komaName)
    {
        var KomaMovement = KomaLists[playerID][komaName].GetComponent<Koma>().CanMovePositions;
        var canMoveList = new List<string>();

        foreach (var pair in KomaMovement)
        {
            if (canMove(pair.Value, playerID))
            {
                canMoveList.Add(pair.Key);
            }
        }

        return canMoveList;
    }

    public List<string> GetCanUchiPositions(int playerID)
    {
        var canUchiPositions = new List<string>();
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                var pos = new Vector2Int(x, y);
                if (!KomaLists[0].Any(pair => pair.Value.GetComponent<Koma>().Pos == pos))
                {
                    if (!KomaLists[1].Any(pair => pair.Value.GetComponent<Koma>().Pos == pos))
                    {
                        foreach (var pair in UchiPosList)
                        {
                            if (pair.Value == pos)
                            {
                                canUchiPositions.Add(pair.Key);
                                break;
                            }
                        }
                    }
                }
            }
        }
        return canUchiPositions;
    }

    public void OnMovementDisided(int playerID, string komaID, string movementID)
    {
        Debug.Log("Player:" + playerID + " Koma:" + komaID + " Movement:" + movementID);

        var movementList = KomaLists[playerID][komaID].GetComponent<Koma>().CanMovePositions;

        Vector2Int newPos = movementList[movementID];
        KomaLists[playerID][komaID].GetComponent<Koma>().Pos = newPos;

        if (KomaLists[playerID][komaID].GetComponent<Koma>().MyKind == Koma.Kind.Chick)
        {
            if (((playerID == 0) && (newPos.y == 3)) || ((playerID == 1) && (newPos.y == 0)))
            {
                KomaLists[playerID][komaID].GetComponent<Koma>().MyKind = Koma.Kind.Chicken;
            }
        }

        int enemyID = playerID == 0 ? 1 : 0;
        foreach (var pair in KomaLists[enemyID])
        {
            if (pair.Value.GetComponent<Koma>().Pos == newPos)
            {
                MochigomaLists[playerID].Add(pair.Key, pair.Value);
                pair.Value.transform.parent = MochigomaFields[playerID].transform;

                Vector2Int tegomaPos = new Vector2Int(0, 0);
                switch (pair.Value.GetComponent<Koma>().MyKind) {
                    case Koma.Kind.Chick:
                    case Koma.Kind.Chicken:
                        tegomaPos = new Vector2Int(0, 0);
                        break;

                    case Koma.Kind.Elephant:
                        tegomaPos = new Vector2Int(1, 0);
                        break;

                    case Koma.Kind.Lion:
                        tegomaPos = new Vector2Int(0, 1);
                        break;

                    case Koma.Kind.Kirin:
                        tegomaPos = new Vector2Int(1, 1);
                        break;
                }
                pair.Value.GetComponent<Koma>().Pos = tegomaPos;
                pair.Value.GetComponent<Koma>().Dir = playerID == 0 ? Koma.Direction.Up : Koma.Direction.Down;
                if (pair.Value.GetComponent<Koma>().MyKind == Koma.Kind.Chicken)
                {
                    pair.Value.GetComponent<Koma>().MyKind = Koma.Kind.Chick;
                }
                KomaLists[enemyID].Remove(pair.Key);

                if (pair.Value.GetComponent<Koma>().MyKind == Koma.Kind.Lion)
                {
                    WinnerText.active = true;
                    WinnerText.GetComponent<Text>().text = "Player" + (playerID + 1).ToString() + " Win!";
                }

                break;
            }
        }
    }

    public void OnTegomaUchi(int playerID, string komaID, string posID)
    {
        Vector2Int newPos = UchiPosList[posID];

        var target = MochigomaLists[playerID][komaID];
        KomaLists[playerID].Add(komaID, target);
        target.transform.parent = Field.transform;
        target.GetComponent<Koma>().Pos = newPos;
        MochigomaLists[playerID].Remove(komaID);
    }

    private bool canMove(Vector2Int pos, int playerID)
    {
        if ((pos.x < 0) || (pos.x > 2)) return false;

        if ((pos.y < 0) || (pos.y > 3)) return false;

        foreach (var pair in KomaLists[playerID])
        {
            if (pair.Value.GetComponent<Koma>().Pos == pos) return false;
        }

        return true;
    }
}
