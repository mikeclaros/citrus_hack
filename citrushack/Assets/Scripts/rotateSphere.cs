using UnityEngine;
using System.Collections;

public class rotateSphere : MonoBehaviour {
    public float revs;
    public float spinx = 0;
    public float spiny = 0;
    public float spinz = 5;
	// Use this for initialization
	void Start () {
	
	}
	
	
	void FixedUpdate() {
	    transform.Rotate(spinx,spiny,spinz*Time.deltaTime, Space.Self);
	}
}
