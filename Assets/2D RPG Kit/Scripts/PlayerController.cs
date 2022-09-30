using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

//self add
public enum PlayerDirection
{
    Down,
    Left, 
    Up,
    Right
}
//self add

public class PlayerController : MonoBehaviour {

    [HideInInspector]
    public Rigidbody2D rigidBody;
    [HideInInspector]
    public Animator animator;
    public float moveSpeed;
    
    //Make instance of this script to be able reference from other scripts!
    public static PlayerController instance;

    [HideInInspector]
    public string areaTransitionName;
    private Vector3 boundary1;
    private Vector3 boundary2;

    [HideInInspector]
    public bool canMove = true;

    //self add
    [Header("Id")]
    public CharacterID id;

    [Header("Sprite")]
    public Sprite dialogBoxProfileSprite;

    [Header("MinimapArrow")]
    public Transform minimapArrowTrans;

	// Use this for initialization
	void Awake () {

        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (instance == null)
        {
            instance = this;
        } else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);
	}

    ///*
    // MOBILE INPUT
    // Uncomment this complete Update() function to enable mobile controls. But comment out the whole Update() function below this one.
    // Update is called once per frame
    //void Update () {
    //    if (ControlManager.instance.mobile)
    //    {
    //        if (canMove)
    //        {
    //            rigidBody.velocity = new Vector2(Mathf.RoundToInt(CrossPlatformInputManager.GetAxis("Horizontal")), Mathf.RoundToInt(CrossPlatformInputManager.GetAxis("Vertical"))) * moveSpeed;
    //        }
    //        else
    //        {
    //            rigidBody.velocity = Vector2.zero;

    //        }

    //        animator.SetFloat("moveX", rigidBody.velocity.x);
    //        animator.SetFloat("moveY", rigidBody.velocity.y);

    //        if (CrossPlatformInputManager.GetAxisRaw("Horizontal") == 1 || CrossPlatformInputManager.GetAxisRaw("Horizontal") == -1 || CrossPlatformInputManager.GetAxisRaw("Vertical") == 1 || CrossPlatformInputManager.GetAxisRaw("Vertical") == -1)
    //        {
    //            if (canMove)
    //            {
    //                animator.SetFloat("lastMoveX", CrossPlatformInputManager.GetAxisRaw("Horizontal"));
    //                animator.SetFloat("lastMoveY", CrossPlatformInputManager.GetAxisRaw("Vertical"));
    //            }
    //        }

    //        transform.position = new Vector3(Mathf.Clamp(transform.position.x, boundary1.x, boundary2.x), Mathf.Clamp(transform.position.y, boundary1.y, boundary2.y), transform.position.z);
    //    }

    //    if (!ControlManager.instance.mobile)
    //    {
    //        if (canMove)
    //        {
    //            rigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    //            rigidBody.velocity = rigidBody.velocity.normalized * moveSpeed;
    //        }
    //        else
    //        {
    //            rigidBody.velocity = Vector2.zero;

    //        }

    //        animator.SetFloat("moveX", rigidBody.velocity.x);
    //        animator.SetFloat("moveY", rigidBody.velocity.y);

    //        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
    //        {
    //            if (canMove)
    //            {
    //                animator.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
    //                animator.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
    //            }
    //        }

    //        //This calculates the bounds and doesn't let the player go beyond the defined bounds
    //        transform.position = new Vector3(Mathf.Clamp(transform.position.x, boundary1.x, boundary2.x), Mathf.Clamp(transform.position.y, boundary1.y, boundary2.y), transform.position.z);


    //        //self add
    //        if (Input.GetAxisRaw("Horizontal") == 1)
    //        {
    //            //right
    //            minimapArrowTrans.eulerAngles = new Vector3(0, 0, 90);
    //        }
    //        else if (Input.GetAxisRaw("Horizontal") == -1)
    //        {
    //            //left
    //            minimapArrowTrans.eulerAngles = new Vector3(0, 0, -90);
    //        }
    //        else if (Input.GetAxisRaw("Vertical") == 1)
    //        {
    //            //up
    //            minimapArrowTrans.eulerAngles = new Vector3(0, 0, 180);
    //        }
    //        else if (Input.GetAxisRaw("Vertical") == -1)
    //        {
    //            //down
    //            minimapArrowTrans.eulerAngles = new Vector3(0, 0, 0);
    //        }
    //        //self add

    //    }

    //}

    //self add. New update
    void Update()
    {

        if (canMove)
        {
            rigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            rigidBody.velocity = rigidBody.velocity.normalized * moveSpeed;
        }
        else
        {
            rigidBody.velocity = Vector2.zero;

        }

        animator.SetFloat("moveX", rigidBody.velocity.x);
        animator.SetFloat("moveY", rigidBody.velocity.y);

        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            if (canMove)
            {
                animator.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
                animator.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
            }
        }

        //This calculates the bounds and doesn't let the player go beyond the defined bounds
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, boundary1.x, boundary2.x), Mathf.Clamp(transform.position.y, boundary1.y, boundary2.y), transform.position.z);


        //self add
        //if (Input.GetAxisRaw("Horizontal") == 1)
        //{
        //    //right
        //    minimapArrowTrans.eulerAngles = new Vector3(0, 0, 90);
        //}
        //else if (Input.GetAxisRaw("Horizontal") == -1)
        //{
        //    //left
        //    minimapArrowTrans.eulerAngles = new Vector3(0, 0, -90);
        //}
        //else if (Input.GetAxisRaw("Vertical") == 1)
        //{
        //    //up
        //    minimapArrowTrans.eulerAngles = new Vector3(0, 0, 180);
        //}
        //else if (Input.GetAxisRaw("Vertical") == -1)
        //{
        //    //down
        //    minimapArrowTrans.eulerAngles = new Vector3(0, 0, 0);
        //}

        //if (canMove)
        //{
        //    if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        //    {
        //        SoundManager.instance.Play_Walk();
        //    }
        //    else
        //    {
        //        SoundManager.instance.FadeOutStop_Walk(0.3f);
        //    }
        //}
        //else
        //{
        //    SoundManager.instance.FadeOutStop_Walk(0.3f);
        //}

        //self add

    }
    //self add

    //Method to set up the bounds which the player can not cross
    public void SetBounds(Vector3 bound1, Vector3 bound2)
    {
        boundary1 = bound1 + new Vector3(.5f, 1f, 0f);
        boundary2 = bound2 + new Vector3(-.5f, -1f, 0f);
    }


    //self add
    public void SetDirection(PlayerDirection dir)
    {
        if (dir == PlayerDirection.Down)
        {
            animator.SetFloat("lastMoveX", 0f);
            animator.SetFloat("lastMoveY", -1f);
            minimapArrowTrans.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (dir == PlayerDirection.Left)
        {
            animator.SetFloat("lastMoveX", -1f);
            animator.SetFloat("lastMoveY", 0f);
            minimapArrowTrans.eulerAngles = new Vector3(0, 0, -90);
        }
        if (dir == PlayerDirection.Up)
        {
            animator.SetFloat("lastMoveX", 0f);
            animator.SetFloat("lastMoveY", 1f);
            minimapArrowTrans.eulerAngles = new Vector3(0, 0, 180);
        }
        if (dir == PlayerDirection.Right)
        {
            animator.SetFloat("lastMoveX", 1f);
            animator.SetFloat("lastMoveY", 0f);
            minimapArrowTrans.eulerAngles = new Vector3(0, 0, 90);
        }
    }

    public PlayerDirection GetDirection()
    {
        if (animator.GetFloat("lastMoveY") == -1f)
        {
            return PlayerDirection.Down;
        }
        if (animator.GetFloat("lastMoveX") == -1f)
        {
            return PlayerDirection.Left;
        }
        if (animator.GetFloat("lastMoveY") == 1f)
        {
            return PlayerDirection.Up;
        }
        if (animator.GetFloat("lastMoveX") == 1f)
        {
            return PlayerDirection.Right;
        }
        else
        {
            return PlayerDirection.Down;
        }
    }
    //self add
}

