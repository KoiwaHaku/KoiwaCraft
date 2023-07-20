using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Init : MonoBehaviour
{
    public GameObject Bedrock;
    public GameObject Cobble;
    public GameObject Soil;

    public int worldSizeX = 30;
    public int worldSizeZ = 30;

    public int soilStartZone = 5;
    public int soilEndZone = 9;

    // Start is called before the first frame update
    void Start()
    {
        //岩盤をワールドの高さ0に敷き詰める
        createBedblock();
    }

    //岩盤をワールドの高さ0に敷き詰める
    void createBedblock(){
        for(int ix = 0; ix < worldSizeX; ix++){
            for(int iz = 0; iz < worldSizeZ; iz++){
                packBlock(ix, iz);
            }
        }
    }

    void packBlock(int x , int z){
        Vector3 bornPos = new Vector3((float)x, 0.0f, (float)z);
        Instantiate(Bedrock, bornPos, Quaternion.identity);

        
        for(int i = 1; i < soilStartZone; i++){
            bornPos = new Vector3((float)x, (float)i, (float)z);
            Instantiate(Cobble, bornPos, Quaternion.identity);
        }

        for(int i = soilStartZone; i < soilEndZone; i++){
            bornPos = new Vector3((float)x, (float)i, (float)z);
            Instantiate(Soil, bornPos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
