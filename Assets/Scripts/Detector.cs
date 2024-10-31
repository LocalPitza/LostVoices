using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Detector : MonoBehaviour
{
    public float detectionRadius = 10f;
    public LayerMask enemyLayer;
    public Image alertIndicator;
    public Color safeColor = Color.green; 
    public Color alertColor = Color.red;

    private Transform nearestEnemy;

    private void Start()
    {
        SetIndicatorSafe();
    }

    private void Update()
    {
        DetectEnemies();
    }

    private void DetectEnemies()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);
        
        if (enemiesInRange.Length > 0)
        {
            nearestEnemy = FindClosestEnemy(enemiesInRange);
            SetIndicatorAlert();
        }
        else
        {
            nearestEnemy = null;
            SetIndicatorSafe();
        }
    }

    private Transform FindClosestEnemy(Collider[] enemies)
    {
        Transform closestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        return closestEnemy;
    }

    private void SetIndicatorSafe()
    {
        alertIndicator.color = safeColor;
        alertIndicator.transform.localScale = Vector3.one;  // Reset size if using pulse effect
        alertIndicator.transform.rotation = Quaternion.identity;  // Reset rotation when no enemy is detected
    }

    private void SetIndicatorAlert()
    {
        alertIndicator.color = alertColor;
        alertIndicator.transform.localScale = Vector3.one * 1.2f;  // Optional: slight scaling for alert

        if (nearestEnemy != null)
        {
            // Calculate direction and rotate the UI indicator
            Vector3 directionToEnemy = nearestEnemy.position - transform.position;
            float angle = Vector3.SignedAngle(transform.forward, directionToEnemy, Vector3.up);
            alertIndicator.transform.rotation = Quaternion.Euler(0, 0, -angle);  // Rotate in UI (z-axis rotation)
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
