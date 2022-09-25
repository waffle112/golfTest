using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballScript : MonoBehaviour
{
    private Rigidbody rb;
    public CannonControlScript cannon;
    public float airSpeed = 10;
    public float explosionPower = 10;
    public float explosionRadius = 10;

    private void Awake() 
    {
        // ADD CODE HERE
        this.rb = this.GetComponent<Rigidbody>();
        // END OF CODE    
    }

    public void Launch(CannonControlScript cannon, float power, float angle)
    {
        //this.cannon = cannon;

        // ADD CODE HERE
        this.rb.AddRelativeForce(this.transform.up * power, ForceMode.Impulse);
        // END OF CODE
    }

    // Update is called once per frame
    void Update()
    {
        // ADD CODE HERE
        
        // END OF CODE
    }

    private void OnCollisionEnter(Collision other) 
    {
        //hasFired = false;
        StartCoroutine(cannon.ReturnCamera());
        //Destroy(this.gameObject, 1);
    }

    
}