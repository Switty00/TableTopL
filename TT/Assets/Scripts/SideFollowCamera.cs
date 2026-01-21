using UnityEngine;

public class SideFollowCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Camera Settings")]
    public Vector3 sideOffset = new Vector3(-5f, 0f, 2.6f);
    public float followSpeed = 5f;
    public float rotateSpeed = 5f;

    private void LateUpdate()
    {
        if (target == null)
            return;
        Vector3 offset =
            target.right * sideOffset.x +
            target.up * sideOffset.y +
            target.forward * sideOffset.z;
        Vector3 desiredPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);
        Quaternion lookRot = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, rotateSpeed * Time.deltaTime);
    }
}
