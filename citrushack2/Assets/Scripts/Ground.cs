using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {

    public float speed = 4;
	void Update () {
	    transform.Translate(new Vector3(-speed*Time.deltaTime,0));
	}
}
