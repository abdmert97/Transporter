using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public ForceController forceControllerLeft;
    public ForceController forceControllerRight;
    // Start is called before the first frame update



    private Camera _camera;
    
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)&& GameManager.Instance.isGameActive)
        {
            Vector2 position = _camera.ScreenToViewportPoint(Input.mousePosition);
            if (position.x > 0.5f)
            {
                forceControllerRight.ButtonEvent();
            }
            else
            {
                forceControllerLeft.ButtonEvent();
            }
        
        }
    }
}
