using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;


public class GameCondition : MonoBehaviour
{
    private PanelController _panelController;
    private Rigidbody _rigidbody;
    private Mesh _mesh;

    private float downwardFoce = 0;
    private LayerMask _layerMask;
    public bool isActive = true;
    private void Start()
    {
        _layerMask = LayerMask.GetMask("Plane");
        _panelController = GameManager.Instance.panelController ;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = true;
      
       

    }

    private void Update()
    {
        if (transform.parent == null && GameManager.Instance.isGameEnded && GameManager.Instance.isGameActive == false)
        {
            Destroy(gameObject);
        }
    }




    private void AddRandomTorque()
    {
   /*     Rigidbody plane = GameManager.Instance.plane.GetComponent<Rigidbody>();
        var ballforce = GameManager.Instance.ballUpwardForce;

        var ınstanceBallUpwardForce = GameManager.Instance.ballUpwardForce;
        if(transform.position.x > plane.transform.position.x)  
            plane.AddTorque(_rigidbody.mass* ınstanceBallUpwardForce*Vector3.back*60);
        else
        {
            plane.AddTorque(_rigidbody.mass* ınstanceBallUpwardForce*Vector3.forward*60);
        }*/
    }

    private void FixedUpdate()
    {
        
        if (!GameManager.Instance.isGameActive && !GameManager.Instance.isGameEnded) 
        {
            _rigidbody.AddForce(Physics.gravity*_rigidbody.mass*20);
        }

        if (!GameManager.Instance.isGameActive ||isActive == false)
        {
            return;
        }

        if (GameManager.Instance.plane == null)
            return;
        Rigidbody plane = GameManager.Instance.plane.GetComponent<Rigidbody>();
       
        var ballforce = GameManager.Instance.ballUpwardForce;

        var ınstanceBallUpwardForce = GameManager.Instance.ballUpwardForce;
        float distance = transform.position.x - plane.transform.position.x;
        if (distance > 6.7f)
            distance = 6.7f;
        if (distance < -6.7f)
            distance = -6.7f;
       
//        Debug.Log(name+ " added foce "  + _rigidbody.mass* GameManager.Instance.torkEffectObject* ınstanceBallUpwardForce*Vector3.back*distance / 80);
            plane.AddTorque( GameManager.Instance.torkEffectObject*_rigidbody.mass* ınstanceBallUpwardForce*Vector3.back/80*distance);
     
        
        
    }


    private void DrawMassCenter()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position+Vector3.up*0.1f, Vector3.down, out hit, LayerMask.GetMask("Plane")))
        {
            Debug.Log("Draw");

            Renderer rend = hit.transform.GetComponent<Renderer>();
            MeshCollider meshCollider = hit.collider as MeshCollider;

            if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
                return;

            Texture2D tex = rend.material.mainTexture as Texture2D;
            Vector2 pixelUV = hit.textureCoord;
            pixelUV.x *= tex.width;
            pixelUV.y *= tex.height;

            Gradient gradient;
            GradientColorKey[] colorKey;
            GradientAlphaKey[] alphaKey;

            gradient = new Gradient();

            // Populate the color keys at the relative time 0 and 1 (0 and 100%)
            colorKey = new GradientColorKey[2];
            colorKey[0].color = Color.red;
            colorKey[0].time = 0.0f;
            colorKey[1].color = Color.yellow;
            colorKey[1].time = 1.0f;

            // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
            alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 0.0f;
            alphaKey[1].time = 1f;

            gradient.SetKeys(colorKey, alphaKey);

            
            DrawCircle((int)pixelUV.x, (int)pixelUV.y, (int)_rigidbody.mass*20 , gradient, tex);
            //StartCoroutine(ResetCircle((int)pixelUV.x, (int)pixelUV.y, (int)rigidbody.mass * 30, tex));
          
        }
        else
        {
            Debug.LogError("Couldn't draw mass fields");
        }
 
    }

    void DrawCircle(int x, int y, int r, Gradient gradient,Texture2D tex)
    {
       
        float i, angle, x1, y1;

        for (int j = 1; j <r ; j++)
        {
            for (i = 0; i < 360; i += 0.1f)
            {
                angle = i;
                x1 = j * Mathf.Cos(angle * Mathf.PI / 180);
                y1 = j * Mathf.Sin(angle * Mathf.PI / 180);
               
                tex.SetPixel(x + (int)x1, y + (int)y1, gradient.Evaluate((float)j/(r*1.4f)));
            }
        }
        tex.Apply();
     //   Instantiate(GameManager.Instance.circle, transform);
     //   GameManager.Instance.circleMaterial.SetTexture("_MainTex",tex);

    }


    private void OnCollisionExit(Collision collision)
    {
       
         if (collision.collider.CompareTag("Plane")&&GameManager.Instance.isGameActive)
         {
             if (!Physics.Raycast(transform.position + Vector3.up, Vector3.down, 5f, _layerMask))
             {
          
                 LoseGame();
             }
         }
             

    }


    private void LoseGame()
    {
        GameManager.Instance.StageFailed();
    }


   
}
