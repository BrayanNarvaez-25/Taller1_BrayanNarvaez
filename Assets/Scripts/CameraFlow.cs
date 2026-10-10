using UnityEngine;

public class CameraFlow : MonoBehaviour
{

    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0, 1, -10);

    void LateUpdate()
    {
        if(target != null)
        {
            Vector3 desideredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desideredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }
}
