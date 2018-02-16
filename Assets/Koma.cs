using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koma : MonoBehaviour {
    public enum Kind
    {
        Chick,
        Elephant,
        Lion,
        Kirin,
        Chicken
    };

    public enum Direction
    {
        Up,
        Down,
    };

    Kind kind;
    Vector2Int pos;
    Direction dir;

    public Vector2Int Pos
    {
        get { return pos; }
        set { pos = value; }
    }

    public Direction Dir
    {
        set { dir = value; }
    }

    public Kind MyKind
    {
        get { return kind; }
        set {
            kind = value;
            switch (kind)
            {
                case Kind.Chick:
                    transform.Find("KomaObj").GetComponent<Renderer>().material = (Material)Resources.Load("M_Chick");
                    break;

                case Kind.Elephant:
                    transform.Find("KomaObj").GetComponent<Renderer>().material = (Material)Resources.Load("M_Elephant");
                    break;

                case Kind.Lion:
                    transform.Find("KomaObj").GetComponent<Renderer>().material = (Material)Resources.Load("M_Lion");
                    break;

                case Kind.Kirin:
                    transform.Find("KomaObj").GetComponent<Renderer>().material = (Material)Resources.Load("M_Kirin");
                    break;

                case Kind.Chicken:
                    transform.Find("KomaObj").GetComponent<Renderer>().material = (Material)Resources.Load("M_Chicken");
                    break;
            }
        }
    }

    public Dictionary<string, Vector2Int> CanMovePositions
    {
        get
        {
            var positions = new Dictionary<string, Vector2Int>();
            int xMove = dir == Direction.Up ? 1 : -1;
            int yMove = dir == Direction.Up ? 1 : -1;
            switch (kind)
            {
                case Kind.Chick:
                    positions.Add("↑", new Vector2Int(pos.x, pos.y + yMove));
                    break;

                case Kind.Elephant:
                    positions.Add("↖", new Vector2Int(pos.x - xMove, pos.y + yMove));
                    positions.Add("↗", new Vector2Int(pos.x + xMove, pos.y + yMove));
                    positions.Add("↙", new Vector2Int(pos.x - xMove, pos.y - yMove));
                    positions.Add("↘", new Vector2Int(pos.x + xMove, pos.y - yMove));
                    break;

                case Kind.Lion:
                    positions.Add("↑", new Vector2Int(pos.x, pos.y + yMove));
                    positions.Add("↓", new Vector2Int(pos.x, pos.y - yMove));
                    positions.Add("←", new Vector2Int(pos.x - xMove, pos.y));
                    positions.Add("→", new Vector2Int(pos.x + xMove, pos.y));
                    positions.Add("↖", new Vector2Int(pos.x - xMove, pos.y + yMove));
                    positions.Add("↗", new Vector2Int(pos.x + xMove, pos.y + yMove));
                    positions.Add("↙", new Vector2Int(pos.x - xMove, pos.y - yMove));
                    positions.Add("↘", new Vector2Int(pos.x + xMove, pos.y - yMove));
                    break;

                case Kind.Kirin:
                    positions.Add("↑", new Vector2Int(pos.x, pos.y + yMove));
                    positions.Add("↓", new Vector2Int(pos.x, pos.y - yMove));
                    positions.Add("←", new Vector2Int(pos.x - xMove, pos.y));
                    positions.Add("→", new Vector2Int(pos.x + xMove, pos.y));
                    break;

                case Kind.Chicken:
                    positions.Add("↑", new Vector2Int(pos.x, pos.y + yMove));
                    positions.Add("↓", new Vector2Int(pos.x, pos.y - yMove));
                    positions.Add("←", new Vector2Int(pos.x - xMove, pos.y));
                    positions.Add("→", new Vector2Int(pos.x + xMove, pos.y));
                    positions.Add("↖", new Vector2Int(pos.x - xMove, pos.y + yMove));
                    positions.Add("↗", new Vector2Int(pos.x + xMove, pos.y + yMove));
                    break;
            }


            return positions;
        }
    }

    public static GameObject Generate(Kind kind, Vector2Int pos, Direction dir, GameObject parent)
    {
        var komaPrefab = Resources.Load("Koma");
        var koma = Instantiate(komaPrefab, parent.transform) as GameObject;
        koma.GetComponent<Koma>().kind = kind;
        koma.GetComponent<Koma>().pos = pos;
        koma.GetComponent<Koma>().dir = dir;

        switch (kind)
        {
            case Kind.Chick:
                koma.transform.Find("KomaObj").GetComponent<Renderer>().material = (Material)Resources.Load("M_Chick");
                break;

            case Kind.Elephant:
                koma.transform.Find("KomaObj").GetComponent<Renderer>().material = (Material)Resources.Load("M_Elephant");
                break;

            case Kind.Lion:
                koma.transform.Find("KomaObj").GetComponent<Renderer>().material = (Material)Resources.Load("M_Lion");
                break;

            case Kind.Kirin:
                koma.transform.Find("KomaObj").GetComponent<Renderer>().material = (Material)Resources.Load("M_Kirin");
                break;

            case Kind.Chicken:
                koma.transform.Find("KomaObj").GetComponent<Renderer>().material = (Material)Resources.Load("M_Chicken");
                break;
        }

        return koma;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = new Vector3(pos.x, 0, pos.y);
        transform.rotation = Quaternion.AngleAxis(dir == Direction.Up ? 0 : 180, new Vector3(0, 1, 0));
	}

    
}
