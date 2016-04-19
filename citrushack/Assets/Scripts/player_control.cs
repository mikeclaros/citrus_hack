using UnityEngine;
using System.Collections;

public class player_control : MonoBehaviour {
    //player properties
    public float mSpeed;
    public float mJumpSpeed;
    public float mJumpCount;
    public bool isGrounded;
	//External to player properties
    public LayerMask GroundLayers;

    //Ground Check properties
    public Transform t_GroundCheck;
    
    //Initialization
	void Start () {
	    t_GroundCheck = transform.FindChild("GroundCheck");
        mSpeed = 5f;
        mJumpSpeed = 2.06f;
	}
	
	
    //Game Loop
    void FixedUpdate(){
        MoveCheck();
        if(Input.GetButton("Jump")){
           Jump();
        }

    }

    void MoveCheck(){
        float hSpeed = Input.GetAxis("Horizontal");
        if( hSpeed < 0){ //Moving Left
            this.transform.localScale = new Vector3(-1,1,1);
        }
        if(hSpeed > 0){ //Moving Right
            this.transform.localScale = new Vector3(1,1,1);
        }
        this.GetComponent<Rigidbody2D>().velocity = new Vector3(hSpeed * mSpeed, this.GetComponent<Rigidbody2D>().velocity.y, 0);
    }

    void Jump(){
        isGrounded = Physics2D.OverlapPoint(t_GroundCheck.position, GroundLayers);
        if(isGrounded){
           this.GetComponent<Rigidbody2D>().AddForce(Vector2.up * mJumpSpeed, ForceMode2D.Impulse);
           isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D coll){
        switch(coll.gameObject.tag){
            case "Ground":
                if(Input.GetButton("Jump")){
                    Jump();
                }
                break;
            default:
                //nothing here
                break;
        }
    }


    
}
