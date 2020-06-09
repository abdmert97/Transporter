using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Vector3 distance = new Vector3(0,8,-10);
    public Vector3 angle = new Vector3(0,0,0);
    [SerializeField] Transform obj;
    [SerializeField] float yOffset;
    [SerializeField] float followSpeed;

    Vector3 vel = new Vector3();

    private Vector3 currentDistance;

    private void Awake()
    {
        currentDistance = distance;
    }
    private void Start()
    {
       
    }
    private void LateUpdate()
    {

       
        if (GameManager.Instance.plane == null) return;
        obj = GameManager.Instance.plane.transform;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(angle), 2 * Time.deltaTime);


        float time = Time.deltaTime * followSpeed;

        currentDistance = Vector3.Lerp(currentDistance, distance, followSpeed * Time.deltaTime);
        if (currentDistance.y < yOffset) return;
        var position = transform.position;
        transform.localPosition = new Vector3(position.x,

                                         Mathf.Lerp(position.y, currentDistance.y + obj.position.y,
                                        followSpeed / 3 / Mathf.Abs(position.y - currentDistance.y - obj.position.y)),
                                        Mathf.Lerp(position.z, currentDistance.z + obj.position.z,
                                        followSpeed / Mathf.Abs(position.z - currentDistance.z - obj.position.z)));
  
    }
}
