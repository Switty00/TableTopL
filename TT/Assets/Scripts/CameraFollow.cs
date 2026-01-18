using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;         
    public float followSpeed = 5f;   
    public float rotateSpeed = 5f;   

    [Header("Camera Offset")]
    public Vector3 offset = new Vector3(0, 25, -25);

    void LateUpdate()
    {
        if (target == null) return;
        Vector3 desiredPos = target.position + offset;
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            followSpeed * Time.deltaTime
        );
        Quaternion desiredRot = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRot,
            rotateSpeed * Time.deltaTime
        );
    }
}
