using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//CharacterControllerを入れる
[RequireComponent(typeof(CharacterController))]

public class TransformObject : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;
    private Vector3 moveDirection;
    public float gravity = 10f;
    private float jumpSpeed = 5f;
    private bool jumpUpEnd = false;
    [SerializeField]
    private float jumpTime;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        moveDirection = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (characterController.isGrounded)
        {
            moveDirection = Vector3.zero;
            var input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            if (input.magnitude > 0f)
            {
                animator.SetFloat("Speed", input.magnitude);
                transform.LookAt(transform.position + input);
                moveDirection += input.normalized * 2;
            }
            else
            {
                animator.SetFloat("Speed", 0f);
            }

            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpSpeed;

                //ジャンプ中に関わる変数
                //地面についている時にリセット
                jumpUpEnd = false;
                jumpTime = 0;
            }

            
        }
        else
        {
            //ジャンプボタン押していると上昇
            //押している時間3秒まで、jumpUpEndがfalseの場合有効
            if (Input.GetButton("Jump") && jumpTime < 3f && !jumpUpEnd)
            {
                //ジャンプボタン押している秒数を加算
                jumpTime += Time.deltaTime;
                moveDirection.y = jumpSpeed;
            }

            //ジャンプ中にジャンプボタン離したことを記録
            //jumpUpEndがfalseの場合有効
            if (Input.GetButtonUp("Jump") && !jumpUpEnd)
            {
                //二回ジャンプできなくする
                jumpUpEnd = true;
            }


        }

        // 重力を設定しないと落下しない
        moveDirection.y -= gravity * Time.deltaTime;

        // Move関数に代入する
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
