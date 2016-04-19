//--------------------------------------------------------------------------------//
// File:        RaycastController.cs
// Authors:     
// 
//
//
// Purpose:
//
//
//--------------------------------------------------------------------------------//



using UnityEngine;
using System.Collections;

public class GroundSpawner : MonoBehaviour {

    public GameObject ground;

	// Use this for initialization
	void Start () {
	    InvokeRepeating("PlaceGround",0f,3f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void PlaceGround(){
        Instantiate(ground);
    }
}
