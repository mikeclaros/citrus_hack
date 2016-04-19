//--------------------------------------------------------------------------------//
// File:        Player.cs
// Authors:     Carson Tan, Mike Claros
// 
//
//
// Purpose:
//  Handle player movements and characters physics on walls
//
//--------------------------------------------------------------------------------//


using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {

    //attributes for player physics
	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = .4f;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	float moveSpeed = 6;
    //------------------------------------


    //Coords to track wall climb attributes
	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;

	public float wallSlideSpeedMax = 3;
	public float wallStickTime = .5f;
	float timeToWallUnstick;

    //--------------------------------



    

    //gravity and velocity attributes
	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;
    //--------------------------------

    
	Controller2D controller;        //Controller object
    Animator am;                    //Animator object
        
	void Start() {
        // instantiate objects
		controller = GetComponent<Controller2D> ();
        am = GetComponentInChildren<Animator>();

        //instantiate gravity and velocity physics
		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
		print ("Gravity: " + gravity + "  Jump Velocity: " + maxJumpVelocity); // for debug purposes
	}

	void Update() {
        //get controller input
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
        int wallDirX = (controller.collisions.left) ? -1 : 1;                   //check direction of wall, refer to controller2d collisions struct

		float targetVelocityX = input.x * moveSpeed;
        
        //gradually change velocityx to to target velocity
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);

		bool wallSliding = false;
        // check that the player is against a wall and falling
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
			wallSliding = true;

            // cap the fall speed on wall
			if (velocity.y < -wallSlideSpeedMax) {
				velocity.y = -wallSlideSpeedMax;
			}

            // check time to stick on walls
			if (timeToWallUnstick > 0) {
				velocityXSmoothing = 0;
				velocity.x = 0;
                // count time while on wall
				if (input.x != wallDirX && input.x != 0) {
					timeToWallUnstick -= Time.deltaTime;
				}
				else {
                    //not on wall reset time
					timeToWallUnstick = wallStickTime;
				}
			}
			else {
				timeToWallUnstick = wallStickTime;
			}

		}else{
            timeToWallUnstick = wallStickTime;
        }

        //check collisions above and below
        if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}

        //check for jump actions while on a wall
		if (Input.GetKeyDown (KeyCode.Space)) {
			
            if (wallSliding) {
				if (wallDirX == input.x) {
					velocity.x = -wallDirX * wallJumpClimb.x;
					velocity.y = wallJumpClimb.y;
				}
				else if (input.x == 0) {
					velocity.x = -wallDirX * wallJumpOff.x;
					velocity.y = wallJumpOff.y;
				}
				else {
					velocity.x = -wallDirX * wallLeap.x;
					velocity.y = wallLeap.y;
				}
			}
			if (controller.collisions.below) {
				velocity.y = maxJumpVelocity;
                am.SetTrigger("jumped");
			}
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			if (velocity.y > minJumpVelocity) {
				velocity.y = minJumpVelocity;
			}
		}

	
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime, input);

        //handle animations
        if (!controller.collisions.below && !(controller.collisions.left || controller.collisions.right)) {
			am.SetBool("falling",true);
		}else{
            am.SetBool("falling",false);
            am.SetBool("isSliding",true);
        }

        if(!(controller.collisions.left || controller.collisions.right)) am.SetBool("isSliding",false);

        if(targetVelocityX == 0){
            am.SetBool("isRunning",false);
        }else if(targetVelocityX > 0){
            am.SetBool("isRunning",true);
            transform.Find("PlayerSprite").localScale = new Vector3(1,1,1);
        }else{
            am.SetBool("isRunning",true);
            transform.Find("PlayerSprite").localScale = new Vector3(-1,1,1);
        }

	}
}
