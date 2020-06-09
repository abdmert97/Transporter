using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using ComplexGame;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarrierController : MonoBehaviour
{
    private Animator _animator;
    private Vector3 _startPoint;
    [SerializeField] private Transform carryPoint;
    // Start is called before the first frame update
    private bool _canCarry = false;
    public Transform targetObject;
    
    void Start()
    {
      //  GameManager.StageEnd += DestroyItself;
        GameManager.LevelFailed += DestroyItself;
        _animator = GetComponent<Animator>();
        RaycastHit hit;
        transform.position-=Vector3.back*Random.Range(3f,5f);
        if (Physics.Raycast(transform.position+Vector3.up, Vector3.down, out hit,LayerMask.GetMask("Plane")))
        {
            transform.position = hit.point;
       
        }
        _startPoint = transform.position;
    }

    private void DestroyItself()
    {
        GameManager.StageEnd -= DestroyItself; 
        GameManager.LevelFailed -= DestroyItself;
        if(gameObject != null)
             Destroy(gameObject);
    }


    public void TakeJob(GameObject carrier, Transform target)
    {
     
        targetObject = GetClosestObject(target);
        targetObject.SetParent(null);
        transform.LookAt(targetObject);
        transform.Rotate(-1*Vector3.right*transform.localRotation.eulerAngles.x);
        StartCoroutine(Move(carrier.transform, targetObject, 1));
    }

    private Transform GetClosestObject(Transform target)
    {
        Transform closest = target.GetChild(0);
        Vector3 carrierPosition = transform.position;
        float distance = Vector3.Distance(closest.position, carrierPosition);
        for (int i = 1; i < target.childCount; i++)
        {
            Transform child = target.GetChild(i);
            if (Vector3.Distance(carrierPosition, child.position) < distance )
            {
                distance = Vector3.Distance(carrierPosition, child.position);
                closest = child;
            }
        }

        return closest;
    }

    IEnumerator Move(Transform player, Transform target, float time)
    {
        int iteration = (int) (time / Time.fixedDeltaTime);
        Vector3 position = target.position-Vector3.back*0.2f;
        position.z -= .5f;
        Vector3 distance = (position - player.position)/iteration;
        distance.y = 0;
        
        _animator.SetBool("Idle",false);
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < iteration&& player!=null; i++)
        {
            if(!Physics.Raycast(carryPoint.position,Vector3.back,0.2f))
            {
                player.position += distance;
            }
            else
            {
                break;
            }
            
            yield return new WaitForFixedUpdate();
        }
            
            
        if (player == null)
        {
            Debug.Log("Player Null");
            EndMission();
        }
        if (target != null)
        {
            target.GetComponent<Rigidbody>().isKinematic = true;
            targetObject.GetComponent<Collider>().enabled = false;
            targetObject.transform.SetParent(transform);
            Bounds bound = BoundCalculator.CalculateCumulativeBounds(target.gameObject);
            float offset = bound.extents.z;

            target.GetComponent<GameCondition>().isActive = false;
            targetObject.transform.position = carryPoint.position+Vector3.back*offset;
            _animator.SetBool("StartCarry",true);
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(Rotate(Vector3.up * 180, 0.4f));  
            float finishTime =2;
            StartCoroutine( Move(transform,_startPoint,finishTime ));
            Invoke(nameof(EndMission),finishTime);
        }
        else
        {
            Invoke(nameof(EndMission),0.1f);
        }
   

    }
    IEnumerator Move(Transform player, Vector3 position, float time)
    {
        int iteration = (int) (time / Time.fixedDeltaTime);
        Vector3 distance = (position - player.position)/iteration;
        
        distance.y = 0;
        for (int i = 0; i < iteration && player!=null; i++)
        {
            player.position += distance;
            yield return new WaitForFixedUpdate();
        }

        if (player == null)
        {
            Debug.Log("Player Null");
            // buga sebeb oluyor olabilir
            EndMission();
        }
        _canCarry = true;

    }
    IEnumerator Rotate(Vector3 angle,float time)
    {
        int iteration = (int) (time / Time.fixedDeltaTime);
        Vector3 rotate = angle/iteration;
     
        for (int i = 0; i < iteration; i++)
        {
            transform.Rotate(rotate);
            yield return new WaitForFixedUpdate();
        }

     
    }


    private void EndMission()
    {
        GameManager.StageEnd -= DestroyItself; 
        GameManager.LevelFailed -= DestroyItself;
        if(GameManager.Instance.finishline.finishActive)
        {
            GameManager.Instance.finishline.carryMissionEnd = true;
            RaycastHit hit;
          
            Destroy(targetObject.gameObject.GetComponent<GameCondition>());
            var collider = targetObject.GetComponent<Collider>();
            collider.enabled = true;
            var rigidbody = targetObject.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
            
              
            transform.SetParent(GameManager.Instance.carriersParent);
            targetObject.SetParent(GameManager.Instance.placedObjects.transform);
            _animator.SetBool("Idle",true);
            _animator.SetBool("StartCarry",false);
         
            StartCoroutine(Rotate(Vector3.up * 180,1));
            //   Destroy(gameObject);
        }
        else
        {
            transform.SetParent(GameManager.Instance.carriersParent);
            targetObject.SetParent(GameManager.Instance.placedObjects.transform);
            _animator.SetBool("Idle",true);
            _animator.SetBool("StartCarry",false);
         
            StartCoroutine(Rotate(Vector3.up * 180,1));
        }
      
    }
}
