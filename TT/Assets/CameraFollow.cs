using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameManager gameManager;
    public Vector3 offset = new Vector3(0f, 15f, -10f);
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (gameManager == null) return;

        Transform target = gameManager.players[gameManager.currentPlayer].transform;

        Vector3 desiredPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);

        transform.LookAt(target);
    }
}
