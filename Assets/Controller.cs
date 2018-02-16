using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Controller : MonoBehaviour {

    public GameObject manager;
    public int id;

    Dictionary<string, GameObject> DobutsuButtons = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> MovementButtons = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> BanmeButtons = new Dictionary<string, GameObject>();

    enum State
    {
        DobutsuSelecting,
        MovementSelecting,
    };

    State state = State.DobutsuSelecting;

    // Use this for initialization
    void Start () {
        DobutsuButtons.Add("とり01", transform.Find("DobutsuButtons/Tori01Button").gameObject);
        DobutsuButtons.Add("ぞう01", transform.Find("DobutsuButtons/Elephant01Button").gameObject);
        DobutsuButtons.Add("らいおん01", transform.Find("DobutsuButtons/Lion01Button").gameObject);
        DobutsuButtons.Add("きりん01", transform.Find("DobutsuButtons/Kirin01Button").gameObject);
        DobutsuButtons.Add("とり02", transform.Find("DobutsuButtons/Tori02Button").gameObject);
        DobutsuButtons.Add("ぞう02", transform.Find("DobutsuButtons/Elephant02Button").gameObject);
        DobutsuButtons.Add("らいおん02", transform.Find("DobutsuButtons/Lion02Button").gameObject);
        DobutsuButtons.Add("きりん02", transform.Find("DobutsuButtons/Kirin02Button").gameObject);

        MovementButtons.Add("↖", transform.Find("DirectionButtons/UpLeftButton").gameObject);
        MovementButtons.Add("↑", transform.Find("DirectionButtons/UpButton").gameObject);
        MovementButtons.Add("↗", transform.Find("DirectionButtons/UpRightButton").gameObject);
        MovementButtons.Add("←", transform.Find("DirectionButtons/LeftButton").gameObject);
        MovementButtons.Add("→", transform.Find("DirectionButtons/RightButton").gameObject);
        MovementButtons.Add("↙", transform.Find("DirectionButtons/LeftDownButton").gameObject);
        MovementButtons.Add("↓", transform.Find("DirectionButtons/DownButton").gameObject);
        MovementButtons.Add("↘", transform.Find("DirectionButtons/RightDownButton").gameObject);

        BanmeButtons.Add("A-1", transform.Find("BanmeButtons/A-1").gameObject);
        BanmeButtons.Add("B-1", transform.Find("BanmeButtons/B-1").gameObject);
        BanmeButtons.Add("C-1", transform.Find("BanmeButtons/C-1").gameObject);
        BanmeButtons.Add("A-2", transform.Find("BanmeButtons/A-2").gameObject);
        BanmeButtons.Add("B-2", transform.Find("BanmeButtons/B-2").gameObject);
        BanmeButtons.Add("C-2", transform.Find("BanmeButtons/C-2").gameObject);
        BanmeButtons.Add("A-3", transform.Find("BanmeButtons/A-3").gameObject);
        BanmeButtons.Add("B-3", transform.Find("BanmeButtons/B-3").gameObject);
        BanmeButtons.Add("C-3", transform.Find("BanmeButtons/C-3").gameObject);
        BanmeButtons.Add("A-4", transform.Find("BanmeButtons/A-4").gameObject);
        BanmeButtons.Add("B-4", transform.Find("BanmeButtons/B-4").gameObject);
        BanmeButtons.Add("C-4", transform.Find("BanmeButtons/C-4").gameObject);

        foreach (var pair in DobutsuButtons)
        {
            pair.Value.GetComponent<Button>().interactable = true;
        }

        foreach (var pair in MovementButtons)
        {
            pair.Value.GetComponent<Button>().interactable = false;
        }
    }

    // Update is called once per frame
    void Update () {
        if (state == State.DobutsuSelecting)
        {
            var komaList = manager.GetComponent<Manager>().GetKomaList(id);
            var mochigomaList = manager.GetComponent<Manager>().GetMochigomaList(id);
            foreach (var pair in DobutsuButtons)
            {
                pair.Value.GetComponent<Button>().interactable = 
                    (komaList.Any(key => pair.Key == key)) || (mochigomaList.Any(key => pair.Key == key));
            }

            foreach (var pair in MovementButtons)
            {
                pair.Value.GetComponent<Button>().interactable = false;
            }

            foreach (var pair in BanmeButtons)
            {
                pair.Value.GetComponent<Button>().interactable = false;
            }
        } else
        {
            var komaList = manager.GetComponent<Manager>().GetKomaList(id);
            var mochigomaList = manager.GetComponent<Manager>().GetMochigomaList(id);
            foreach (var pair in DobutsuButtons)
            {
                pair.Value.GetComponent<Button>().interactable =
                    (komaList.Any(key => pair.Key == key)) || (mochigomaList.Any(key => pair.Key == key));
            }

            if (komaList.Any(name => name == selectedKomaName))
            {
                var canMoveList = manager.GetComponent<Manager>().GetCanMovePositions(id, selectedKomaName);
                foreach (var pair in MovementButtons)
                {
                    pair.Value.GetComponent<Button>().interactable = canMoveList.Any(key => pair.Key == key);
                }
            } else
            {
                foreach (var pair in MovementButtons)
                {
                    pair.Value.GetComponent<Button>().interactable = false;
                }
            }

            if (mochigomaList.Any(name => name == selectedKomaName))
            {
                var canUchiPosList = manager.GetComponent<Manager>().GetCanUchiPositions(id);
                foreach (var pair in BanmeButtons)
                {
                    pair.Value.GetComponent<Button>().interactable = canUchiPosList.Any(s => s == pair.Key);
                }
            } else
            {
                foreach (var pair in BanmeButtons)
                {
                    pair.Value.GetComponent<Button>().interactable = false;
                }
            }
        }
	}

    string selectedKomaName;

    public void OnKomaSelected(int komaID)
    {
        List<string> komaNameList = new List<string>
        {
            "とり01",
            "ぞう01",
            "らいおん01",
            "きりん01",
            "とり02",
            "ぞう02",
            "らいおん02",
            "きりん02",
        };
        selectedKomaName = komaNameList[komaID];

        state = State.MovementSelecting;
    }

    public void OnMovementSelected(int movementID)
    {
        List<string> movementNameList = new List<string>
        {
            "↖", "↑", "↗", "←", "→", "↙", "↓", "↘"
        };
        var selectedMovementName = movementNameList[movementID];

        manager.GetComponent<Manager>().OnMovementDisided(id, selectedKomaName, selectedMovementName);

        state = State.DobutsuSelecting;
    }

    public void OnBanmeSelected(int banmeID)
    {
        Debug.Log(id + "/" + banmeID);
        List<string> banmeNameList = new List<string>
        {
            "A-1", "B-1", "C-1",
            "A-2", "B-2", "C-2",
            "A-3", "B-3", "C-3",
            "A-4", "B-4", "C-4",
        };
        var selectedBanmeName = banmeNameList[banmeID];

        manager.GetComponent<Manager>().OnTegomaUchi(id, selectedKomaName, selectedBanmeName);

        state = State.DobutsuSelecting;
    }
}
