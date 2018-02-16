using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KomaButton : MonoBehaviour {

    public uint id;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        Debug.Log("Koma:" + id.ToString());
       // transform.parent.parent.GetComponent<Controller>().OnKomaSelected(id);
    }
}
