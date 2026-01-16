using Unity.VisualScripting;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] private bool move1=false;
    [SerializeField] private bool move2=false;
    [SerializeField] private bool move3=false;
    [SerializeField] private bool move4=false;
    [SerializeField] private bool move5=false;
    [SerializeField] private bool move6=false;
    public float moveSpeed = 5f;
    private Vector3 targetPosition;
    private bool isMoving = false;


    void Update()
    {
        if (move1)
        {
            targetPosition = transform.position + transform.up * 2f;
            isMoving = true;
            move1 = false;
        }
        if (move2)
        {
            targetPosition = transform.position + transform.up * 4f;
            isMoving = true;
            move2 = false;
        }
        if (move3)
        {
            targetPosition = transform.position + transform.up * 6f;
            isMoving = true;
            move3 = false;
        }
        if (move4)
        {
            targetPosition = transform.position + transform.up * 8f;
            isMoving = true;
            move4 = false;
        }
        if (move5)
        {
            targetPosition = transform.position + transform.up * 10f;
            isMoving = true;
            move5 = false;
        }
        if (move6)
        {
            targetPosition = transform.position + transform.up * 12f;
            isMoving = true;
            move6 = false;
        }

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
            }
        }

        Vector3 p1 = new Vector3(10f, 1.3f, -10f);
        Vector3 p2 = new Vector3(-10f, 1.3f, -10f);
        Vector3 p3 = new Vector3(-10f, 1.3f, 10f);
        Vector3 p4 = new Vector3(10f, 1.3f, 10f);

        if (Vector3.Distance(transform.position, p1) < 0.1f)
            transform.rotation = Quaternion.Euler(-90f, 0f, 90f);

        if (Vector3.Distance(transform.position, p2) < 0.1f)
            transform.rotation = Quaternion.Euler(-90f, 0f, 180f);

        if (Vector3.Distance(transform.position, p3) < 0.1f)
            transform.rotation = Quaternion.Euler(-90f, 0f, 270f);

        if (Vector3.Distance(transform.position, p4) < 0.1f)
            transform.rotation = Quaternion.Euler(-90f, 0f, 360f);
    }

}
