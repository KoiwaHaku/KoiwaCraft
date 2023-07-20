using System.IO;
using System.Text.RegularExpressions;
using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;

    public float jumpPower;

    public float moveSpeed;
    
    public bool fly = true;

    public float sensitivityX = 1.4f; //マウス感度
    public float sensitivityY = 1.4f; 


    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody>();

        //カーソル設定
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate(){
        //回転！！
        moveRotate();

        //移動！！
        move();

        //ジャンプ！！
        if(Input.GetKey(KeyCode.Space)) jump();
    }

    //回転
    void moveRotate(){
        float mouse_move_x = Input.GetAxis("Mouse X") * sensitivityX;
        float mouse_move_y = Input.GetAxis("Mouse Y") * sensitivityY;

        //横方向の回転 (自分自信のy回転)
        Transform myTransform = this.transform;
        UnityEngine.Vector3 worldAngle = myTransform.eulerAngles;
        worldAngle.y += mouse_move_x;
        myTransform.eulerAngles = worldAngle;

        //縦方向の回転 (子オブジェクトであるカメラのx回転)
        Transform childTransform = this.gameObject.transform.GetChild(0);
        UnityEngine.Vector3 childAngle = childTransform.eulerAngles;
        childAngle.x -= mouse_move_y;
        childTransform.eulerAngles = childAngle;
    }

    //十字移動ベクトル演算
    private (float sinNum, float cosNum) moveWASD(float angle, float speed){
        return(
            (float)(Math.Sin(angle * (Math.PI / 180)) * speed),
            (float)(Math.Cos(angle * (Math.PI / 180)) * speed)
        );
    }

    //十字移動
    void move(){

        Transform myTransform = this.transform;
        float yAngle = myTransform.eulerAngles.y;

        UnityEngine.Vector3 movePosi = myTransform.position;

        if(Input.GetKey(KeyCode.W)){
            var get = moveWASD(yAngle, moveSpeed);

            movePosi.x += get.sinNum;
            movePosi.z += get.cosNum;
        }

        if(Input.GetKey(KeyCode.A)){
            var get = moveWASD(yAngle - 90, moveSpeed * 0.5f);

            movePosi.x += get.sinNum;
            movePosi.z += get.cosNum;
        }

        if(Input.GetKey(KeyCode.S)){
            var get = moveWASD(yAngle + 180, moveSpeed * 0.5f);

            movePosi.x += get.sinNum;
            movePosi.z += get.cosNum;
        }

        if(Input.GetKey(KeyCode.D)){
            var get = moveWASD(yAngle + 90, moveSpeed * 0.3f);

            movePosi.x += get.sinNum;
            movePosi.z += get.cosNum;
        }

        myTransform.position = movePosi;
    }

    //ジャンプ
    void jump(){

        if(fly)return;

        rb.AddForce(0, jumpPower, 0, ForceMode.VelocityChange);
        // rb.AddForce(transform.up * jumpPower);
        fly = true;
    }
}
