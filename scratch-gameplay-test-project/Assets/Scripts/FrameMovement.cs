using System;
using UnityEngine;

public class FrameMovement : MonoBehaviour
{
    public RandomPrize randomPrize;
    public Transform startPoint;

    public Vector2Int size;

    public float stayTime = 1f;

    private int currentX = 0;
    private int currentY = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = startPoint.position;
        InvokeRepeating(nameof(Move), 0, stayTime);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) PresStop();
    }

    private void Move()
    {
        if (currentX < size.x - 1)
        {
            transform.position += Vector3.right;
            currentX++;
        }
        else
        {
            if (currentY < size.y - 1)
            {
                transform.position += Vector3.down;
                currentY++;
            }
            else
            {
                if (currentY == size.y - 1 & currentX == size.x - 1)
                {
                    transform.position = startPoint.position;
                    currentX = 0;
                    currentY = 0;
                    return;
                }
            }

            transform.position = new Vector2(startPoint.position.x, transform.position.y);
            currentX = 0;
        }
    }

    private void PresStop()
    {
        if (IsInvoking()) CancelInvoke();
        else
        {
            randomPrize.RollSprite();
            InvokeRepeating(nameof(Move), 0, stayTime);
        }
    }
}
