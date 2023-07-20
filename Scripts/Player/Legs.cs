using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legs : MonoBehaviour
{
    private GameObject parentObj;
    private Player script;
    // Start is called before the first frame update
    void Start()
    {
        parentObj = transform.parent.gameObject;
        script = parentObj.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //着地判定
    void OnTriggerEnter(){
        script.fly = false;
        Debug.Log("着地");
    }
    //離陸判定
    void OnTriggerExit(){
        script.fly = true;
    }
}
