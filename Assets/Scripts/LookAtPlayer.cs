using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform player;
    public Transform head;
    public float rotationSpeed = 5f;
    public float maxLookAngle = 60f;
    public float maxTurnAngle = 80f;
    public float lookDistance = 5f;
    public float forwardAngleLimit = 90f;

    private void Update()
    {
        LookAt();
    }

    private void LookAt()
    {
        if (player != null && head != null)
        {

            float distanceToPlayer = Vector3.Distance(player.position, head.position);

            if (distanceToPlayer <= lookDistance)
            {

                Vector3 directionToPlayer = player.position - head.position;

                float angleToPlayer = Vector3.Angle(head.forward, directionToPlayer);

                if (angleToPlayer <= forwardAngleLimit)
                {

                    Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

                    Vector3 currentRotation = head.eulerAngles;

                    float clampedX = Mathf.Clamp(targetRotation.eulerAngles.x,
                                                  currentRotation.x - maxLookAngle,
                                                  currentRotation.x + maxLookAngle);

                    float clampedY = Mathf.Clamp(targetRotation.eulerAngles.y,
                                                  currentRotation.y - maxTurnAngle,
                                                  currentRotation.y + maxTurnAngle);

                    Quaternion clampedRotation = Quaternion.Euler(clampedX, clampedY, 0);
                    head.rotation = Quaternion.Slerp(head.rotation, clampedRotation, rotationSpeed * Time.deltaTime);
                }
            }
        }
    }
}
