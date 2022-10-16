using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    CharacterController characterController;

    [SerializeField]
    Camera cam;

    [SerializeField]
    Animator anim;

    public GameObject playerModel;

    public float moveSpeed;
    public float rotateSpeed;
    public Vector3 moveDirection;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        cam = Camera.main;

    }

    
    void Update()
    {
        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        moveDirection = moveDirection * moveSpeed;

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) 
        {
            transform.rotation = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, 0);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }

        characterController.Move(moveDirection * Time.deltaTime);
        anim.SetFloat("speed", Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z));

        //animate when were moving
        if (anim.GetFloat("speed") != 0f) 
        {
            anim.Play("running");
        }
    }
}
