using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private MeshCollider _meshCollider;
    private float upForce = 0;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _meshCollider = GetComponent<MeshCollider>();
        GameManager.Instance.plane = gameObject;
        _rigidbody.useGravity = true;
        _rigidbody.maxAngularVelocity = GameManager.Instance.maxAngularSpeed;
    }

    private void FixedUpdate()
    {
        int forceDirection = GameManager.Instance.ballCountRight - GameManager.Instance.ballCountleft;
        if(GameManager.Instance.isGameActive)
        {
            _rigidbody.AddTorque(forceDirection *GameManager.Instance.torkeffectBallons* GameManager.Instance.ballUpwardForce * Vector3.forward/50);
//            Debug.Log("Balls add torque " +forceDirection*GameManager.Instance.torkeffectBallons * GameManager.Instance.ballUpwardForce* Vector3.forward/50 );
      
        if (_rigidbody.velocity.y > GameManager.Instance.maxUpSpeed)
            _rigidbody.velocity -= (_rigidbody.velocity.y -GameManager.Instance.maxUpSpeed) * Vector3.up;
        }
    }
}
