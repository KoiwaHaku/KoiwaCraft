using System.Threading;
using System.Text.RegularExpressions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    LineRenderer linerend;

    private BoxCollider collider;

    //ブロック破戒系変数
    public float durability = 2.0f;
    private float nowDurability = 2.0f;
    private bool nowBreaking = false;
    private float breakStartTime = 0.0f;

    //ブロック破戒後
    private bool breakedBrock = false;
    public float soulRotaSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        nowDurability = durability;
        linerend = gameObject.AddComponent<LineRenderer>();
        collider = gameObject.AddComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //ブロック破戒
        if(nowBreaking){
            if(Input.GetMouseButtonUp(0)) nowBreaking = false;

            if(Time.time - breakStartTime >= nowDurability){
                breaked();
            }
        }

        //ブロック破戒後　(浮遊)
        if(breakedBrock){
            soul();
        }
    }

    public void forcused(){
        makeLine();
    }

    public void reForcused(){
        linerend.positionCount = 0;
        nowBreaking = false;
    }

    public void clicked(int mode, UnityEngine.Vector3 clickPosi){
        if(mode == 1){
            layBlock(clickPosi);
        }else if(mode == 0){
            breakBlockStart();
        }
    }

    void layBlock(UnityEngine.Vector3 clickPosi){
        if(breakedBrock) return;

        Transform myTransform = this.transform;
        UnityEngine.Vector3 myPosi = myTransform.position;
        
        //クリックされたローカル座標
        UnityEngine.Vector3 localPosi = new Vector3(
            clickPosi.x - myPosi.x,
            clickPosi.y - myPosi.y,
            clickPosi.z - myPosi.z
        );

        //新たにブロックを作成する座標
        UnityEngine.Vector3 newPosi = new Vector3(
            myPosi.x,
            myPosi.y,
            myPosi.z
        );

        Func<float, float> judgeFlont = (float get) => {
            return ( get > 0 ? 1.0f : -1.0f );
        };

        //どの面がクリックされたか
        if(Math.Abs(localPosi.x) == 0.5f){
            newPosi.x += (1.0f * judgeFlont(localPosi.x));
        }else if(Math.Abs(localPosi.y) == 0.5f){
            newPosi.y += (1.0f * judgeFlont(localPosi.y));
        }else if(Math.Abs(localPosi.z) == 0.5f){
            newPosi.z += (1.0f * judgeFlont(localPosi.z));
        }

        //playerにゲームオブジェクト作成申請
        GameObject playerObj = GameObject.Find("Player");
        Hand script = playerObj.GetComponent<Hand>();
        script.layNewBlock(newPosi);
    }

    void breakBlockStart(){
        nowBreaking = true;
        breakStartTime = Time.time;
    }

    //破戒された
    void breaked(){
        Transform myTransform = this.transform;

        //サイズ変更
        UnityEngine.Vector3 myScale = myTransform.localScale;
        myScale.x = 0.2f;
        myScale.y = 0.2f;
        myScale.z = 0.2f;
        myTransform.localScale = myScale;

        //コライダーのサイズ変更
        collider.size = new Vector3(0.2f, 1.0f, 0.2f);

        //物理演算の作成
        this.gameObject.AddComponent<Rigidbody>();
        Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation; // 移動も回転もさせない

        nowBreaking = false;
        breakedBrock = true;
    }

    void soul() {
        Transform myTransform = this.transform;
        UnityEngine.Vector3 myAngle = myTransform.eulerAngles;

        myAngle.y += soulRotaSpeed;

        myTransform.eulerAngles = myAngle;
    }

    //ユーザーに触れたら
    void OnCollisionEnter(Collision collision){
        if( !breakedBrock ) return;

        GameObject hitObj = collision.gameObject;

        const int pleyerLayer = 3;
        
        if(hitObj.layer == pleyerLayer){
            Debug.Log("good... bye...");
            Destroy(this.gameObject);
        }
    }

    void makeLine(){
        //正方形を囲むように線を一本づつ描く　色々試したがゴリ押し

        if(breakedBrock) return;

        Transform myTransform = this.transform;
        UnityEngine.Vector3 myPosi = myTransform.position;

        float[] p = new float[6]{
            myPosi.x - 0.51f,
            myPosi.y - 0.51f,
            myPosi.z - 0.51f,
            myPosi.x + 0.51f,
            myPosi.y + 0.51f,
            myPosi.z + 0.51f
        };

        UnityEngine.Vector3[] positions = new Vector3[]{
            new Vector3(p[0], p[1], p[2]),
            new Vector3(p[0], p[1], p[5]),
            new Vector3(p[0], p[4], p[2]),
            new Vector3(p[0], p[4], p[5]),
            new Vector3(p[3], p[1], p[2]),
            new Vector3(p[3], p[1], p[5]),
            new Vector3(p[3], p[4], p[2]),
            new Vector3(p[3], p[4], p[5])
        };

        UnityEngine.Vector3[] polygons = new Vector3[]{
            positions[0],
            positions[1],
            positions[3],
            positions[2],
            positions[0],
            positions[4],
            positions[6],
            positions[2],
            positions[3],
            positions[7],
            positions[6],
            positions[4],
            positions[5],
            positions[7],
            positions[5],
            positions[1],
        };

        linerend.positionCount = polygons.Length;

        linerend.widthMultiplier = 0.02f;

        linerend.material = new Material(Shader.Find("Sprites/Default"));
        linerend.startColor = Color.black;
        linerend.endColor = Color.black;

        linerend.SetPositions(polygons);
    }
}
