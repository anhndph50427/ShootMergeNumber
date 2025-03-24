using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WallStep : MonoBehaviour
{
    public float stepDistance = 2f; // Khoảng cách di chuyển
    public float moveSpeed = 5f;    // Tốc độ di chuyển
    private bool isMoving = false;

    private Vector3 targetPosition;
    private Vector3 positionWall;

    void Start()
    {
        targetPosition = transform.position;
        positionWall = transform.position;
    }

    // Hàm được gọi từ script khác
    public void MoveWall(bool moveBack)
    {
        if (!isMoving)
        {
            {
                float distance = moveBack ? -stepDistance * 2 : stepDistance;
                targetPosition = transform.position + transform.forward * distance;
                StartCoroutine(MoveToTarget());
            }
        }
    }

    IEnumerator MoveToTarget()
    {
        isMoving = true;
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        isMoving = false;
    }
}