using UnityEngine;

public class Keep_Rotation_And_Position : MonoBehaviour
{
    private Quaternion startRotation;
    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }
    
    private void LateUpdate()
    {
        transform.rotation = startRotation;
        transform.position = startPosition;
    }
}
