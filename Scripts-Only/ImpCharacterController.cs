using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Using http://wiki.unity3d.com/index.php/SmoothMouseLook for camera movement

public class ImpCharacterController : MonoBehaviour
{
    public float walkSpeed = 2;
    public float runSpeed = 6;

    //public float counterMovement = 0.1f;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public Transform playerCamera;
    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;

	private bool crouching = false;
    private bool clear = false;
	private Ray crouchRay;
    private RaycastHit crouchHit;

    public float crouchSmoothTime = 0.1f;
	private float crouchY = 0;
	private float crouchVelocity = 0;
	public float crouchHeight = 0.5f;
	public CapsuleCollider collider;

    private bool jumpReady = true;
    private float jumpCooldown = 0.25f;
    public float jumpForce = 800f;
 

    bool jump;

    public Rigidbody player;

    private int layerMask;

    bool isFree = true;

    void Start()
    {
        Cursor.visible = false;
		layerMask = LayerMask.NameToLayer("Player");
        layerMask = 1 << layerMask;
        layerMask = ~layerMask;
    }

    void Update()
    {
        Movement();
		Crouch();
        isClear();
        Jump();
        FreeCamera();
    }


    private void Movement() {
	Vector3 newForward = new Vector3(playerCamera.forward.x, 0, playerCamera.forward.z);
     Vector2 input = new Vector2(Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;   
        bool running = Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = ((running)?runSpeed:walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
        if(Input.GetKey(KeyCode.W) && isFree){
            
        transform.Translate (newForward* currentSpeed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey(KeyCode.S) && isFree){
        transform.Translate (-newForward * currentSpeed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey(KeyCode.A) && isFree){
        transform.Translate (-playerCamera.right * currentSpeed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey(KeyCode.D) && isFree){
        transform.Translate (playerCamera.right * currentSpeed * Time.deltaTime, Space.World);
        }
    }
	private void Crouch(){
		if(Input.GetKey("left ctrl")){
            crouching = true;
			crouchY = -crouchHeight;
            collider.center = new Vector3(0,-0.5f, 0);
			collider.height = 1;
                

		}
		if((Input.GetKeyUp("left ctrl")) && clear){
            Debug.Log("UnCrouching");
            crouching = false;
			crouchY = crouchHeight;
            collider.center = new Vector3(0,0, 0);
			collider.height = 2;

		}

		float newY = Mathf.SmoothDamp(playerCamera.localPosition.y, crouchY, ref crouchVelocity, crouchSmoothTime);
		playerCamera.localPosition = new Vector3(0, newY, 0);
		
		
	}

    private void isClear(){
        if(crouching == true){
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out crouchHit, 1)){
                Debug.Log("Hit careful");
                clear = false;
            }
            else{
                clear = true;
            }
        }
    }

    private void Jump(){
        if(Input.GetKey("space") && jumpReady &&  Physics.CheckCapsule(GetComponent<Collider>().bounds.center, new Vector3(GetComponent<Collider>().bounds.center.x, GetComponent<Collider>().bounds.min.y - 0.1f, GetComponent<Collider>().bounds.center.z), 0.02f, layerMask)){

            jumpReady = false;
            Debug.Log("Jump");
            player.AddForce(Vector2.up * jumpForce*1.5f);
            
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
	
    private void ResetJump(){
        jumpReady = true;
    }
    //Need movement to be in the 3D, and need to understand why the toggle isn't working as expected
    private void FreeCamera(){
        if(Input.GetKeyUp("tab")){
            isFree = !isFree;
            GetComponent<Rigidbody>().useGravity = !GetComponent<Rigidbody>().useGravity;
            Debug.Log("Fly");
        }
        
        if(isFree == false){
        if(Input.GetKey(KeyCode.W) && !isFree){
            
        transform.Translate (playerCamera.forward* currentSpeed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey(KeyCode.S) && !isFree){
        transform.Translate (-playerCamera.forward * currentSpeed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey(KeyCode.A) && !isFree){
        transform.Translate (-playerCamera.right * currentSpeed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey(KeyCode.D) && !isFree){
        transform.Translate (playerCamera.right * currentSpeed * Time.deltaTime, Space.World);
        }
        }


        }    
    
    
    }

