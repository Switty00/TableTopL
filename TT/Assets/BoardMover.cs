using UnityEngine;
using System.Collections.Generic;

public class BoardMover : MonoBehaviour
{
    public float moveSpeed = 6f;
    public List<Transform> boardSpaces;
    public int currentIndex = 0;

    private int stepsRemaining = 0;
    private bool isMoving = false;
    private Transform targetSpace;

    [HideInInspector] public bool isCurrentPlayer = false;
    public System.Action onMoveComplete;

    void Update()
    {
        if (isMoving)
            MoveToNextSpace();
    }

    public void MoveSpaces(int spaces)
    {
        if (!isCurrentPlayer)
        {
            Debug.LogWarning(name + " tried to move but is not current player.");
            return;
        }

        if (isMoving || boardSpaces.Count == 0) return;

        Debug.Log(name + " is starting to move " + spaces + " spaces.");
        stepsRemaining = spaces;
        SetNextTarget();
    }


    private void SetNextTarget()
    {
        currentIndex = (currentIndex + 1) % boardSpaces.Count;
        targetSpace = boardSpaces[currentIndex];
        isMoving = true;

        Vector3 dir = (targetSpace.position - transform.position).normalized;
        if (dir.sqrMagnitude > 0.0001f)
        {
            Quaternion look = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Euler(-90f, look.eulerAngles.y, 0f);
        }
    }

    private void MoveToNextSpace()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetSpace.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetSpace.position) < 0.01f)
        {
            transform.position = targetSpace.position;
            stepsRemaining--;

            if (stepsRemaining > 0)
            {
                SetNextTarget();
            }
            else
            {
                isMoving = false;
                onMoveComplete?.Invoke();
            }
        }
    }
}
