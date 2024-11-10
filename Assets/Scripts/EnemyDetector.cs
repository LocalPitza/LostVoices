using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyDetector : MonoBehaviour
{
    public Image northIndicator;
    public Image southIndicator;
    public Image eastIndicator;
    public Image westIndicator;

    public Transform player;

    public float detectionRange = 10f;
    public float fadeDurationAtMaxDistance = 2f; // Duration for fade at maximum distance (slow)
    public float fadeDurationAtMinDistance = 0.5f; // Duration for fade at minimum distance (fast)

    private SoundManager soundManager;

    private Image currentPrimaryIndicator; // Primary indicator for main direction
    private bool isFadingPrimary = false; // Track primary fading state

    private List<Transform> enemies = new List<Transform>();

    void Start()
    {
        // Find all enemies automatically by tag
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemyObj in enemyObjects)
        {
            enemies.Add(enemyObj.transform);
        }

        // Locate the SoundManager
        soundManager = FindObjectOfType<SoundManager>();

        // Start all indicators as transparent
        SetIndicatorTransparency(0f);
    }

    void Update()
    {
        if (enemies.Count == 0) return;

        // Find the closest enemy within detection range
        Transform closestEnemy = GetClosestEnemy();

        if (closestEnemy != null)
        {
            float distanceToEnemy = Vector3.Distance(player.position, closestEnemy.position);
            UpdateIndicatorDirection(closestEnemy, distanceToEnemy);
        }
        else
        {
            StopFading();
        }
    }

    Transform GetClosestEnemy()
    {
        Transform closestEnemy = null;
        float closestDistance = detectionRange + 1f; // Start with a distance greater than the detection range

        foreach (Transform enemy in enemies)
        {
            if (enemy == null) continue;

            float distance = Vector3.Distance(player.position, enemy.position);

            if (distance < closestDistance && distance <= detectionRange)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    void UpdateIndicatorDirection(Transform enemy, float distanceToEnemy)
    {
        if (enemy == null) return;

        Vector3 directionToEnemy = (enemy.position - player.position).normalized;

        // Determine primary indicator
        Image primaryIndicator = null;

        // Calculate angles relative to the player
        float angleToEnemy = Vector3.Angle(player.forward, directionToEnemy);
        float rightAngleToEnemy = Vector3.Angle(player.right, directionToEnemy);
        float backwardAngleToEnemy = Vector3.Angle(-player.forward, directionToEnemy);
        float leftAngleToEnemy = Vector3.Angle(-player.right, directionToEnemy);

        // Determine primary direction (N, S, E, W)
        if (angleToEnemy < 45f) // Forward (North)
        {
            primaryIndicator = northIndicator;
        }
        else if (backwardAngleToEnemy < 45f) // Backward (South)
        {
            primaryIndicator = southIndicator;
        }
        else if (rightAngleToEnemy < 45f) // Right (East)
        {
            primaryIndicator = eastIndicator;
        }
        else if (leftAngleToEnemy < 45f) // Left (West)
        {
            primaryIndicator = westIndicator;
        }

        // Handle primary indicator fading
        if (primaryIndicator != null)
        {
            if (currentPrimaryIndicator != primaryIndicator)
            {
                StopFadingPrimary(); // Stop any fading on the previous primary indicator
                currentPrimaryIndicator = primaryIndicator;
                StartFading(currentPrimaryIndicator, distanceToEnemy); // Start primary fade
            }
        }
        else
        {
            StopFadingPrimary(); // If no primary indicator, stop fading
        }
    }

    void StartFading(Image indicator, float distanceToEnemy)
    {
        // Calculate fade duration based on distance to the enemy
        float fadeDuration = Mathf.Lerp(fadeDurationAtMaxDistance, fadeDurationAtMinDistance, 1 - (distanceToEnemy / detectionRange));

        // If the indicator is already fading, stop its current sequence
        if (isFadingPrimary && currentPrimaryIndicator == indicator)
        {
            indicator.DOKill();
        }

        // Start the fade in and out sequence
        indicator.DOFade(1f, fadeDuration).OnComplete(() =>
        {
            indicator.DOFade(0f, fadeDuration).OnComplete(() =>
            {
                // Allow new fades after completing
                if (currentPrimaryIndicator == indicator)
                {
                    isFadingPrimary = false;
                    StartFading(indicator, distanceToEnemy); // Restart primary fading
                }
            });
        });

        // Update fading state
        if (currentPrimaryIndicator == indicator)
        {
            isFadingPrimary = true;
        }

        // Play sound whenever the indicator starts fading
        if (soundManager != null)
        {
            soundManager.Play("Detect");
        }
    }

    void StopFading()
    {
        StopFadingPrimary();
    }

    void StopFadingPrimary()
    {
        if (currentPrimaryIndicator != null)
        {
            currentPrimaryIndicator.DOKill();
            SetIndicatorTransparency(0f);
            currentPrimaryIndicator = null; // Reset primary indicator
            isFadingPrimary = false; // Reset fading state
        }
    }

    void SetIndicatorTransparency(float alpha)
    {
        SetTransparency(northIndicator, alpha);
        SetTransparency(southIndicator, alpha);
        SetTransparency(eastIndicator, alpha);
        SetTransparency(westIndicator, alpha);
    }

    void SetTransparency(Image indicator, float alpha)
    {
        if (indicator == null) return;
        Color color = indicator.color;
        color.a = alpha;
        indicator.color = color;
    }
}
