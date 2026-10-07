using UnityEngine;

public class CameraRoute : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0,1,-10);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    void LateUpdate()
    {
        if(target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPositon = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPositon;
        }
    }

}
