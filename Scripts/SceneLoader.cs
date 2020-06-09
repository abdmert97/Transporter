using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
