using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // your player
    public Vector3 offset = new Vector3(0f, 0f, -10f); // make sure Z is -10 for 2D
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }
    }
}
