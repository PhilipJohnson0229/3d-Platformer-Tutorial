using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    CharacterController characterController;

    public static PlayerController instance;

    [SerializeField]
    Camera cam;

    [SerializeField]
    Animator anim;

    public GameObject playerModel;

    public float moveSpeed;
    public float rotateSpeed;
    public float jumpForce;
    public float gravityScale = 5f;
    public Vector3 moveDirection;
    [SerializeField]
    bool grounded;

    void Awake()
    {
        instance = this; 
        characterController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        cam = Camera.main;
    }

    
    void Update()
    {
        float yStore = moveDirection.y;

        grounded = characterController.isGrounded;
        anim.SetBool("isGrounded", grounded);

        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        moveDirection.Normalize();
        moveDirection = moveDirection * moveSpeed;
        moveDirection.y = yStore;

        //animate when were moving
        if (anim.GetFloat("speed") != 0f && anim.GetBool("isGrounded") == true) 
        {
            anim.Play("running");
        }

        if (characterController.isGrounded) 
        {
            moveDirection.y = 0f;

            if (Input.GetButtonDown("Jump")) 
            {
                anim.Play("Jump");
                moveDirection.y = jumpForce;
            }
        }

        moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;
        characterController.Move(moveDirection * Time.deltaTime);

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            transform.rotation = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, 0);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }

        anim.SetFloat("speed", Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z));
       
    }
}
