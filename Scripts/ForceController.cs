using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ForceController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private GameObject _balloon;
    private bool _ballEnabled = true;
    private List<Material> _colors;
    private int _ballCount;
    public bool isLeft;
    private float _totalUpwardForce = 0;
    
    [SerializeField] private Rigidbody planeRigidbody;
    [SerializeField] private Transform spawnPointFront;
    [SerializeField] private Transform spawnPointBack;
    private void Start()
    {
        _ballCount = 0;
        _colors = GameManager.Instance.colors;
        _rigidbody = GetComponent<Rigidbody>();
        GameManager.StageCompleted += DecreaseUpwardForce;
        GameManager.StageCompleted += AddTorque;
        GameObject ball1 = Instantiate(GameManager.Instance.balloonPrefab, spawnPointFront);
        GameObject ball2 = Instantiate(GameManager.Instance.balloonPrefab, spawnPointBack);
       // _totalUpwardForce += GameManager.Instance.ballUpwardForce ;
        StartCoroutine(MoveUpward(ball1.transform.GetChild(0).transform, 6f));
        _balloon = ball1.transform.GetChild(0).gameObject;
        StartCoroutine(MoveUpward(ball2.transform.GetChild(0).transform, 6f));
        _balloon = ball2.transform.GetChild(0).gameObject;
      

    }

    private void OnDisable()
    {
        GameManager.StageCompleted -= DecreaseUpwardForce;
        GameManager.StageCompleted -= AddTorque;
    }

    IEnumerator MoveUpward(Transform transform,float amount)
    {
        int iteration = 60;
        float amountPerFrame = amount / iteration;
        for (int i = 0; i < iteration; i++)
        {
            transform.position += Vector3.up *amountPerFrame;
            yield return new WaitForEndOfFrame();
        }
    }

    void DecreaseUpwardForce()
    {
        _totalUpwardForce /=1.15f;
    }

    private void FixedUpdate()
    {
        planeRigidbody.AddForce(_totalUpwardForce * Vector3.up, ForceMode.Force);
    }

    public void AddForce(float force)
    {
        _totalUpwardForce += force;
        if(!isLeft)  
            planeRigidbody.AddTorque(GameManager.Instance.ballUpwardForce*Vector3.forward*50);
        else
        {
            planeRigidbody.AddTorque(GameManager.Instance.ballUpwardForce*Vector3.back*50);
        }
        //planeRigidbody.AddForceAtPosition(force*Vector3.up,transform.position*100,ForceMode.Force);
    }

    private void AddTorque()
    {
        var ınstanceBallUpwardForce = GameManager.Instance.ballUpwardForce;
        if(!isLeft)  
            planeRigidbody.AddTorque(_ballCount*ınstanceBallUpwardForce*Vector3.forward*90);
        else
        {
            planeRigidbody.AddTorque(_ballCount*ınstanceBallUpwardForce*Vector3.back*90);
        }
    }



    public void ButtonEvent()
    {
        if (_ballEnabled)
        {
            ClickEvent();
         
        }
    }

    private void ClickEvent()
    {
       if(_ballEnabled)
       {
           CreataBall(spawnPointBack);
           CreataBall(spawnPointFront);
           AddForce(GameManager.Instance.ballUpwardForce);
           _ballEnabled = false;
           Invoke("EnableBalls", 0.4f);
           _ballCount++;
           if (isLeft)
               GameManager.Instance.ballCountleft =  _ballCount;
           else
           {
               GameManager.Instance.ballCountRight =  _ballCount;
           }
       }
    }

    private void CreataBall(Transform spawnPoint)
    {
        GameObject ball = Instantiate(GameManager.Instance.balloonPrefab, spawnPoint, false);
        StartCoroutine(MoveUpward(ball.transform.GetChild(0), 6 + Random.Range(-1f, 2f)));
        ball.GetComponentInChildren<Renderer>().sharedMaterial = _colors[Random.Range(0, _colors.Count)];
        float xOffset = isLeft ? Random.Range(0f, 1.5f) : Random.Range(-1.5f, 0f);
        ball.transform.localPosition += Vector3.right * xOffset; 
        ball.transform.localPosition += 0.1f * _ballCount * Vector3.forward;
   
    }

    private void EnableBalls()
    {
        _ballEnabled = true;
    }
}
