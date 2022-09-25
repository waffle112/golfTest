using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonControlScript : MonoBehaviour
{
    public Camera cam;
    public GameObject cannonBarrel;
    
    public Transform firepoint;
    //public GameObject cannonBallPrefab;

    public float minAngle = 10;
    public float maxAngle = 80;

    public float minPower = 5;
    public float maxPower = 20;

    public float currentAngle = 75;
    public float currentPower = 10;


    private float powerChangerValue = 0.25f;
    public bool hasFired = false;
    private bool lineup = false;
    private int switchline = 1; 
    private bool firestage1 = false;

    //0 for exact shooting
    //1 for variable shooting 
    public int gamemode = 0; 



    public GameObject cannonball;
    void Start()
    {
        currentAngle = Mathf.Clamp(cannonball.transform.localEulerAngles.x, minAngle, maxAngle);
        hasFired = false;
        if(cam == null)
        {
            cam = Camera.main;
        }
        if(gamemode == 1){
            GetComponent<CannonPathLine>()?.ToggleLine();
            currentAngle = minAngle;
            lineup = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(gamemode == 0){
            gamemode0();
        }
        if(gamemode == 1){
            
             
            gamemode1(); 
        }
    }
    void gamemode0(){
        float moveBarrel = -Input.GetAxis("Vertical");
        float moveBase = Input.GetAxis("Horizontal");

        //Debug.Log(moveBarrel);

        currentAngle = Mathf.Clamp(cannonBarrel.transform.localEulerAngles.x + moveBarrel, minAngle, maxAngle);


        //cannonBarrel.transform.Rotate(0, moveBase/3, 0);

        cannonBarrel.transform.localEulerAngles =  new Vector3(
            currentAngle,
            cannonBarrel.transform.localEulerAngles.y + moveBase/3,
            cannonBarrel.transform.localEulerAngles.z 
            );


        if(Input.mouseScrollDelta.y > 0) //up
        {
            currentPower = Mathf.Clamp(currentPower + powerChangerValue, minPower, maxPower);
        }
        if(Input.mouseScrollDelta.y < 0) //down
        {
            currentPower = Mathf.Clamp(currentPower - powerChangerValue, minPower, maxPower);
        }

        if(Input.GetKeyDown(KeyCode.Mouse1) && !lineup && !hasFired) //toggle fire line 
        {
            GetComponent<CannonPathLine>()?.ToggleLine();
            lineup = true; 
        }

        if(Input.GetKeyDown(KeyCode.Space) && !hasFired)
        {
            FireCannonball();
        }
    }
    void gamemode1(){
        
        if(!firestage1){
            float moveBase = Input.GetAxis("Horizontal");

                    currentAngle = Mathf.Clamp(currentAngle + .05f * switchline, minAngle, maxAngle);
        

                    if (currentAngle == maxAngle || currentAngle == minAngle){
                        switchline *= -1; 
                    }

                    //cannonBarrel.transform.Rotate(0, moveBase/3, 0);

                    cannonBarrel.transform.localEulerAngles =  new Vector3(
                        currentAngle,
                        cannonBarrel.transform.localEulerAngles.y + moveBase/3,
                        cannonBarrel.transform.localEulerAngles.z 
                        );

        }else{
            currentPower = Mathf.Clamp(currentPower + .025f * switchline, minPower, maxPower);
            if (currentPower == maxPower || currentPower == minPower){
                switchline *= -1; 
            }
        }

        if(Input.GetKeyDown(KeyCode.Space) && !hasFired)
        {
            if(!firestage1){
                firestage1 = true;
            }else{
                firestage1 = false;
                FireCannonball();
            }
            
        }

        
    }
    public void FireCannonball()
    {
        if(lineup){
            
            GetComponent<CannonPathLine>()?.ToggleLine();
            lineup = false;
        }
        hasFired = true;
        //GameObject cannonball =  Instantiate(cannonBallPrefab);
        cam.GetComponent<ThirdPersonOrbitCamBasic>().player = cannonball.transform;
        cannonball.transform.position = firepoint.position;
        cannonball.transform.rotation = firepoint.rotation;
        cannonball.GetComponent<CannonballScript>()?.Launch(this, currentPower, currentAngle);


        
    }

    private void OnCollisionEnter(Collision collision){
        if (hasFired){
            StartCoroutine(ReturnCamera());
        }
        

    }
    public IEnumerator ReturnCamera()
    {
        cam.GetComponent<ThirdPersonOrbitCamBasic>().player = this.transform;
        while(false)
        {
            yield return new WaitForFixedUpdate();
        }
        hasFired = false;

       cannonball.transform.rotation = Quaternion.Euler(0, 0, 0); 
       Debug.Log("can shoot again");
       if (!lineup){
            GetComponent<CannonPathLine>()?.ToggleLine();
            lineup = true;
       }
    }

}