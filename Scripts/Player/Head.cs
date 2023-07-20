using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{    
    public float rayDistance = 5.0f;

    private GameObject beforeForcus = null;
    private UnityEngine.Vector3 hitPosition = new Vector3(0.0f, 0.0f, 0.0f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) clickBlock(0);
        if(Input.GetMouseButtonDown(1)) clickBlock(1);

        forcus();
    }

    void forcus(){
        Transform myTransform = this.transform;
        UnityEngine.Vector3 posi = myTransform.position;
        float angleX = myTransform.eulerAngles.x;

        Transform perTransform = transform.parent.gameObject.transform;
        float angleY = perTransform.eulerAngles.y;

        UnityEngine.Vector3 angle = new Vector3(angleX, angleY, 0.0f);

        UnityEngine.Vector3 direct= Quaternion.Euler(angle) * Vector3.forward;

        Ray ray = new Ray(posi, direct);

        RaycastHit hit;

        if(Physics.Raycast(ray ,out hit ,rayDistance)){
            GameObject forcusObj = hit.collider.gameObject;
            hitPosition = hit.point;

            if(beforeForcus != forcusObj){
                Block script = forcusObj.GetComponent<Block>();
                script.forcused();

                if(beforeForcus != null){
                    Block bscript = beforeForcus.GetComponent<Block>();
                    bscript.reForcused();
                }

                beforeForcus = forcusObj;
            }
        }else{
            if(beforeForcus != null){
                Block bscript = beforeForcus.GetComponent<Block>();
                bscript.reForcused();
            }

            beforeForcus = null;
        }
    }

    void clickBlock(int mode){
        if(beforeForcus == null) return;

        Block script = beforeForcus.GetComponent<Block>();
        script.clicked(mode, hitPosition);
    }
}
