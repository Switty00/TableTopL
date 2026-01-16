using Unity.VisualScripting;
using UnityEngine;

public class BoardMover : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float tileSize = 2f;

    private Vector3 targetPosition;
    private bool isMoving = false;
    private int moveSteps = 0;

    public GameManager gameManager;

    // Board corners
    private Vector3 p1 = new Vector3(10f, 1.3f, -10f);
    private Vector3 p2 = new Vector3(-10f, 1.3f, -10f);
    private Vector3 p3 = new Vector3(-10f, 1.3f, 10f);
    private Vector3 p4 = new Vector3(10f, 1.3f, 10f);

    void Update()
    {
        HandleCornerRotation();

        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    // Call this from DiceUI

    public void MoveSpaces(int spaces)
    {
        if (isMoving) return;

        moveSteps = spaces;
        SetNextTarget();
    }

    private void SetNextTarget()
    {
        targetPosition = transform.position + transform.up * tileSize;
        isMoving = true;
    }

    private void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            moveSteps--;

            if (moveSteps > 0)
            {
                SetNextTarget();
            }
            else
            {
                isMoving = false;
                gameManager.NextTurn();
            }
        }
    }

    private void HandleCornerRotation()
    {
        if (Vector3.Distance(transform.position, p1) < 0.1f)
            transform.rotation = Quaternion.Euler(-90f, 0f, 90f);

        if (Vector3.Distance(transform.position, p2) < 0.1f)
            transform.rotation = Quaternion.Euler(-90f, 0f, 180f);

        if (Vector3.Distance(transform.position, p3) < 0.1f)
            transform.rotation = Quaternion.Euler(-90f, 0f, 270f);

        if (Vector3.Distance(transform.position, p4) < 0.1f)
            transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
    }
}

