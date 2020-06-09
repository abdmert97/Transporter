using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
   

    // Update is called once per frame
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Plane") && (!GameManager.Instance.isGameActive&& GameManager.Instance.finishline.finishActive))
        {
            if (other.transform.GetComponent<GameCondition>() != null)
            {
                Debug.Log("hit base");
                GameManager.Instance.StageFailed();
            }
        }
    }
}
