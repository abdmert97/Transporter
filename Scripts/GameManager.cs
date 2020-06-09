using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameUI;

public enum GameType {OneBaloon, ManyBalloon }
public class GameManager : MonoBehaviour
{
    public PanelController panelController;
    public static Action LevelRestarted;
    public static Action NextLevelStarted;
    public static Action StageCompleted;
    public static Action LevelFailed;
    public static Action LevelSuccessfull;
    public static Action StageEnd;
    public GameObject balloonPrefab;
    public List<Material> colors;
    public int ballCountleft = 0;
    public int ballCountRight = 0;
    public GameObject plane;
    public List<GameObject> levels;
    public int currentLevel = 0;
    public bool isGameEnded = false;
    public bool isGameActive = true;
    public static GameManager Instance { get; private set; }
    public GameObject placedObjects;
    public FinishLine finishline;
    public float torkeffectBallons;
    public float torkEffectObject;
    public GameObject activeLevel;
    public Transform carriersParent;
    public GoldParticleSystem goldEffect;
    public bool goldOption = true;
    #region GameData

    public float maxUpSpeed = 15;
    public float defaultUpForce = 0.5f;
    public float ballUpwardForce = 1;
    public float maxAngularSpeed = 0.05f;

    #endregion
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Application.targetFrameRate = 60;

    }

    private void SetGoldOption()
    {
        if (goldOption)
        {
            goldEffect.gameObject.SetActive(true);
           
        }
        else
        {
            goldEffect.gameObject.SetActive(false);
           
    
        }
    }
    private void Start()
    {
      
        panelController = FindObjectOfType<PanelController>();
        LevelRestarted += SetGoldOption;
        NextLevelStarted += SetGoldOption;
        GameObject level = Instantiate(levels[0]);
        activeLevel = level;
        placedObjects = new GameObject("placedObject");
        carriersParent = (new GameObject("Carriers")).transform;
    }

    private void ClearCarriers()
    {
        int carrierCount = carriersParent.childCount;
        List<GameObject> carriers = new List<GameObject>();
        for (int i = 0; i < carrierCount; i++)
        {
            carriers.Add(carriersParent.GetChild(i).gameObject);
         
        }

        for (int i = 0; i < carrierCount; i++)
        {
            Destroy(carriers[i]);        }
    }
    void ResetBallCounts()
    {
        ballCountleft = 0;
        ballCountRight = 0;

    }
    private void NextLevel()
    {
       
        currentLevel++;
        currentLevel = currentLevel % levels.Count;
        Invoke(nameof(DestroyLevel), 1f);
        Invoke(nameof(LoadLevel), 1.5f);
        Invoke(nameof(OnNextLevelStarted),1.51f);
       

    }

    private void OnNextLevelStarted()
    {
        NextLevelStarted?.Invoke();
    }

    private void OnLevelRestarted()
    {
        LevelRestarted?.Invoke();
    }
    private void ReloadLevel()
    {  
       
        Invoke(nameof(DestroyLevel), 1f);
        Invoke(nameof(LoadLevel), 1.5f);
        Invoke(nameof(OnLevelRestarted),1.51f);

    }
    
    private void DestroyLevel()
    {
        ClearCarriers();
        Destroy(GameObject.FindGameObjectWithTag("Level"));
      
    }
    private void LoadLevel()
    {
          
        Destroy(placedObjects); 
        placedObjects = new GameObject("placedObject");
        GameObject level = Instantiate(levels[currentLevel]);
        activeLevel = level;
        isGameActive = true;
        isGameEnded = false;
        ResetBallCounts();

    }

    public void StageFailed()
    {
        isGameActive = false;
        isGameEnded = true;
        LevelFailed?.Invoke();
        ReloadLevel();
        
      
    }
    public void StageSuccessfull()
    {
        StageCompleted.Invoke();
        isGameActive = true;
        
    }

    public void OnStageEnd()
    {
        isGameActive = false;
        StageEnd?.Invoke();
        
    }
    public void LevelCompleted()
    {
        isGameEnded = true;
        isGameActive = false;
        LevelSuccessfull?.Invoke();
        NextLevel();
    }
}
