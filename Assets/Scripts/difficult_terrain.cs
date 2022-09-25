using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class difficult_terrain : MonoBehaviour
{
    public float maxPower = 0;
    public float minPower = 40; 
    public float minAngle = 10;
    public float maxAngle = 80;

    public float temp1;
    public float temp2;

    public CannonControlScript cannon;

    void Start(){
        temp1 = cannon.maxPower;
        temp2 = cannon.minPower; 
    }
    void OnCollisionStay(Collision collision){
        
        cannon.maxPower = maxPower;
        cannon.minPower = minPower;
        

    }
    void OnCollisionExit(Collision collision){
        cannon.maxPower = temp1;
        cannon.minPower = temp2;
        Debug.Log("left special area");
    }
}
