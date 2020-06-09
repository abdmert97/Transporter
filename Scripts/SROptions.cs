using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

public partial class SROptions
{
    
    
    [Category("Game Settings")]
    public float DefaultUpForce
    {
        get => GameManager.Instance.defaultUpForce;
        set
        {
            GameManager.Instance.defaultUpForce = value;
      
        }
    }
    [Category("Game Settings")]
    public float ObjectTorkEffect
    {
        get => GameManager.Instance.torkEffectObject;
        set
        {
            GameManager.Instance.torkEffectObject = value;
         
        }
    }
    [Category("Game Settings")]
    public float BallTorkEffect
    {
        get => GameManager.Instance.torkeffectBallons;
        set
        {
            GameManager.Instance.torkeffectBallons = value;
          
        }
    }
    [Category("Game Settings")]
    public float BallUpwardForce
    {
        get => GameManager.Instance.ballUpwardForce;
        set
        {
            GameManager.Instance.ballUpwardForce = value;
        
        } 
    }

    [Category("Game Settings")]
    public float MaxAngularSpeed
    {
        get => GameManager.Instance.maxAngularSpeed;
        set {
             GameManager.Instance.maxAngularSpeed = value;
        }
    }
    [Category("Game Settings")]
    public int currentLevel
    {
        get => GameManager.Instance.currentLevel;
        set => GameManager.Instance.currentLevel = value;
    }
    [Category("Game Settings")]
    public bool GoldEffect
    {
        get => GameManager.Instance.goldOption;
        set
        {
            GameManager.Instance.goldOption = value;
        }
       
    }
    [Category("Game Settings")]
    public float MaxUpSpeed
    {
        get =>  GameManager.Instance.maxUpSpeed;
        set
        {
            GameManager.Instance.maxUpSpeed= value;
        }
      
    }
}