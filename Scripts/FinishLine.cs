using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
public class FinishLine : MonoBehaviour
{
    
    [SerializeField] private TextMeshPro _finish;
    [SerializeField] private Transform platform;
    // Start is called before the first frame update
    [SerializeField] private GameObject carrierPrefab;
    [SerializeField] private Transform spawnPoint;
    private Collider _collider;
    public int stageCounter = 0;
    public bool finishActive = false;
    private  Stack<GameObject> carriers = new Stack<GameObject>();
    public bool carryMissionEnd = false;
    [SerializeField] Vector3 platformDefaultPosition;
    [SerializeField] private Collider platformLeft;
    [SerializeField] private Collider platformRight;

    private void Start()
    {
        
        _collider = GetComponent<Collider>();
        GameManager.LevelRestarted += DecreaseLevelHeight;
        GameManager.NextLevelStarted += DecreaseLevelHeight;
        GameManager.LevelRestarted += EnableFinishString;
        GameManager.NextLevelStarted += EnableFinishString;
        
        GameManager.LevelRestarted += ResetCounter;
        GameManager.NextLevelStarted += ResetCounter;
        
        GameManager.LevelFailed += EndFinish;
        
        GameManager.LevelRestarted += InvokedCarriers;
        GameManager.NextLevelStarted += InvokedCarriers;
        InvokedCarriers();
        GameManager.StageCompleted += InvokedCarriers;
        GameManager.LevelRestarted += ClearCarriers;
        GameManager.NextLevelStarted += ClearCarriers;
        GameManager.LevelFailed += ClearCarriers;
     
     
    }

    private void InvokedCarriers()
    {
        Invoke(nameof(CreateCarriers),0.1f);
    }
    private void CreateCarriers()
    {
        Transform level = GameManager.Instance.activeLevel.transform;
        
        while (level.parent != null)
            level = level.parent;
        
        Transform objects = level.GetChild(0);
        
        int objectCount = objects.childCount;
        if (stageCounter != 3)
        {
            objectCount  /= (3 - stageCounter);
        }

        float random = Random.Range(-2f,2f);
        
        for (int i = 0; i < objectCount; i++)
        {
            Vector3 spawnPosition = spawnPoint.position;
            spawnPosition += Vector3.right * (i - 1) * 2.5f + Vector3.left * random;
            GameObject carrier = Instantiate(carrierPrefab, spawnPosition, spawnPoint.rotation);
            carriers.Push(carrier);
        }

    }


    private void ClearCarriers()
    {
        carriers.Clear();
    }
    private void EnableFinishString()
    {
        _finish.enabled = true;
    }
    
    private void EndFinish()
    {
        finishActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Plane")&& !GameManager.Instance.isGameEnded)
        {
         
            finishActive = true;
            GameManager.Instance.OnStageEnd();
            StartFinishRoutine(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Plane")&&!GameManager.Instance.isGameActive)
        {
            GameManager.Instance.StageFailed();
        }
    }

    private void StartFinishRoutine(Collider other)
    {

        _finish.enabled = false;
        StartCoroutine(TransportObjects());
    }


    private void IncreaseLevelHeight()
    {
        if(stageCounter ==1)
            transform.position += Vector3.up * 8.943f*(5);
        
        else if (stageCounter == 2)
        {
            transform.position += Vector3.up * 8.943f*(6);
        }
           

    }
    private void DecreaseLevelHeight()
    {
        if(stageCounter ==1)
            transform.position -= Vector3.up * 8.943f*(5);
        
        else if (stageCounter == 2)
        {
            transform.position -= Vector3.up * 8.943f*(11);
        }
           
        else if (stageCounter == 3)
        {
            transform.position -= Vector3.up * 8.943f*(11);
        }
    }
   

    private IEnumerator TransportObjects()
    {
        yield return  new WaitForSeconds(1f);
        
        if (finishActive)
        {
            Transform level = GameManager.Instance.activeLevel.transform;
            while (level.parent != null)
                level = level.parent;
            //Objects on plane
            Transform objects = level.GetChild(0);
            
            while (carriers.Count >0)
            {
                var carrier = carriers.Pop();
                var carrierController = carrier.GetComponent<CarrierController>();
                carrierController.TakeJob(carrier, objects);
                
            }
            yield return new WaitUntil(() =>carryMissionEnd);
            
            yield return new WaitForSeconds(0.1f);
            if(finishActive)
            {
                finishActive = false;
                EndLevel();
            }
            
        }

    }

    void ResetCounter()
    {
        stageCounter = 0;
    }
  
    private void EndLevel()
    {
        if (GameManager.Instance.goldOption)
        {
            GameManager.Instance.goldEffect.PlayGoldEffect();
        }
       
        StartCoroutine(MoveBlocks(2,true));
   
    }

    private void FinishStage()
    {
        carryMissionEnd = false;
        stageCounter++;
        if (stageCounter == 3)
        {
            DecreaseLevelHeight();
            stageCounter = 0;
            GameManager.Instance.LevelCompleted();
        }

        GameManager.Instance.StageSuccessfull();
        IncreaseLevelHeight();
        ResetFinishLine();
    }

    private IEnumerator MoveBlocks(float time,bool direction)
    {
        _collider.enabled = false;
            int iteration = (int) (time / Time.fixedDeltaTime);
            Vector3 distance = (platformDefaultPosition)/iteration;
             distance.y = 0;
            distance.z = direction ? distance.z : -1 * distance.z;
        
            for (int i = 0; i < iteration ; i++)
            {
                
                platform.localPosition += distance;
                yield return new WaitForFixedUpdate();
            }
            platformLeft.enabled = false;
            platformRight.enabled =false;
         
            if (direction) // 
            {
                FinishStage();
            }
            else
            {
                platformLeft.enabled = true;
                platformRight.enabled =true;
            }
          
    }

    private void OpenCollider()
    {
        _collider.enabled = true;
    }
    private void ResetFinishLine()
    {
     
        Invoke(nameof(OpenCollider),.75f);

      
        _finish.enabled = true;
        StartCoroutine(MoveBlocks(1,false));
    }


}
