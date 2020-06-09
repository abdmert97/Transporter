using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PanelController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI winGame;

    [SerializeField] TextMeshProUGUI loseGame;
    
    // Start is called before the first frame update
    private void Start()
    {
        GameManager.LevelRestarted += StartLevel;
        GameManager.NextLevelStarted += StartLevel;
        GameManager.LevelFailed += LoseGame;
        GameManager.LevelSuccessfull += WinGame;

    }
    private void WinGame()
    {
    
            winGame.enabled = true;

    }
    private void LoseGame()
    {

            loseGame.enabled = true;
    
    }

    private void StartLevel()
    {
        winGame.enabled = false;
        loseGame.enabled = false;
    }
}
