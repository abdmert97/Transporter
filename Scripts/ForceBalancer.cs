using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceBalancer : MonoBehaviour
{

    float totalMass;

    public Rigidbody plane;

    private bool _isClicked = false;
    // Start is called before the first frame update
    void Start()
    {
        totalMass = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            Rigidbody body = transform.GetChild(i).GetComponent<Rigidbody>();
            if (body && body.gameObject.activeSelf)
                totalMass += body.mass;
        }
        totalMass += plane.mass;

        
    }
    private void FixedUpdate()
    {
        if (_isClicked == false&& Input.GetMouseButtonDown(0))
        {
            _isClicked = true;
    


        }
        if(_isClicked)
            plane.AddForce(totalMass *GameManager.Instance.defaultUpForce*Physics.gravity*-1,ForceMode.Force);
    }

    void SetAngular()
    {
        plane.maxAngularVelocity = 0.2f;
    }
  

}
