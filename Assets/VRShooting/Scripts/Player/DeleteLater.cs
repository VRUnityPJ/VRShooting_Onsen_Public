using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class DeleteLater : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 oldVelocity;
    private Vector3 jumpVelocity;
    //移動速度
    [SerializeField] private float moveSpeed = 80.0f;
    //重力加速度
    [SerializeField] private float gravity = -20f;
    //曲がる速さ
    [SerializeField] private float agility = 30f;
    //ジャンプ高度
    [SerializeField] private float jump = 5f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //z軸方向にジャンプするバグがあったので
        //ジャンプは別の関数で処理している
        if (controller.isGrounded)
        {
            jumpVelocity.y = 0f;
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                jumpVelocity.y += Mathf.Sqrt(-jump * gravity);
            }
        }
        jumpVelocity.y += gravity * Time.deltaTime;

        //移動の方向・速さのベクトル
        //斜め移動で速度がルート2倍にならないように補正している
        if (controller.isGrounded)
        {
            velocity = Vector3.zero;
        }
        if (Input.GetAxisRaw("Horizontal") != 0f && Input.GetAxisRaw("Vertical") != 0f)
        {
            velocity = new Vector3(Input.GetAxisRaw("Horizontal") / Mathf.Sqrt(2), 0f, Input.GetAxisRaw("Vertical") / Mathf.Sqrt(2));
        }
        else if (Input.GetAxisRaw("Horizontal") != 0f ^ Input.GetAxisRaw("Vertical") != 0f)
        {
            velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        }
        velocity = Vector3.MoveTowards(oldVelocity, velocity, Time.deltaTime * agility);
        oldVelocity = velocity;

        transform.LookAt(transform.position + velocity);
        controller.Move(new Vector3(velocity.x * moveSpeed *Time.deltaTime, jumpVelocity.y * Time.deltaTime, velocity.z * moveSpeed * Time.deltaTime)); 
    }
}