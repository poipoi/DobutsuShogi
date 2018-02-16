using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKomaManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onKomaMovementDisided(uint komaID, uint movementID)
    {
        GameObject komaObj;

        switch (komaID)
        {
            case 0:
                komaObj = transform.Find("Tori").gameObject;
                break;

            case 1:
                komaObj = transform.Find("Elephant").gameObject;
                break;

            case 2:
                komaObj = transform.Find("Lion").gameObject;
                break;

            case 3:
                komaObj = transform.Find("Kirin").gameObject;
                break;

            default:
                return;
        }

        switch (movementID)
        {
            case 0:
                komaObj.transform.localPosition = komaObj.transform.localPosition + new Vector3(-1, 0, 1);
                break;

            case 1:
                komaObj.transform.localPosition = komaObj.transform.localPosition + new Vector3(0, 0, 1);
                break;

            case 2:
                komaObj.transform.localPosition = komaObj.transform.localPosition + new Vector3(1, 0, 1);
                break;

            case 3:
                komaObj.transform.localPosition = komaObj.transform.localPosition + new Vector3(-1, 0, 0);
                break;

            case 4:
                komaObj.transform.localPosition = komaObj.transform.localPosition + new Vector3(1, 0, 0);
                break;

            case 5:
                komaObj.transform.localPosition = komaObj.transform.localPosition + new Vector3(-1, 0, -1);
                break;

            case 6:
                komaObj.transform.localPosition = komaObj.transform.localPosition + new Vector3(0, 0, -1);
                break;

            case 7:
                komaObj.transform.localPosition = komaObj.transform.localPosition + new Vector3(1, 0, -1);
                break;

            default:
                return;
        }
    }
}
