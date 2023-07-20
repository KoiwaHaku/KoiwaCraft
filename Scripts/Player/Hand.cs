using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public struct Blocks
    {
        public String name;
        public GameObject body;
        public int haveNum;
    }

    [SerializeField] private GameObject Bedrock;

    //ゲームオブジェクト取得用
    public GameObject[] blocksGet = new GameObject[]{};

    public Blocks[] blocks = new Blocks[]{};

    // Start is called before the first frame update
    void Start()
    {
        Array.Resize(ref blocks, blocksGet.Length);
        foreach(var i in blocksGet){
            Blocks newPush = new Blocks(){
                name = i.name,
                body = i,
                haveNum = 0
            };

            blocks[blocks.Length] = newPush;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void layNewBlock(UnityEngine.Vector3 makePosi){
        Instantiate(Bedrock, makePosi, Quaternion.identity);
    }
}
