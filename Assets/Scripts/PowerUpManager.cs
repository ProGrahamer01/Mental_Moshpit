using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PowerUpManager : MonoBehaviour
{
    [Header("Power-Up Settings")]
    public float freezeDuration = 5f;       // How long enemies stay frozen
    public float enragedSpeedBoost = 2f;    // How much faster enemies move when enraged
    //               public float enrageDuration = 10f;      // How long they are enraged for 

    private List<EnemyMovementAnimated> enemyList = new List<EnemyMovementAnimated>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // Cache all animated enemiess in the scene at start
        EnemyMovementAnimated[] enemies = FindObjectsByType<EnemyMovementAnimated>(FindObjectsSortMode.None);
        enemyList.AddRange(enemies);
    }

    public void TriggerPowerUp()
    {
        StartCoroutine(FreezeEnemies());
        StartCoroutine(EnrageEnemiesTemporarily());
        // EnrageEnemies();
    }

    // ------------------------------------------------------------------------------------------
    private IEnumerator FreezeEnemies()
    {
        //  1) Freeze
        foreach (var enemy in enemyList)
        {
            if (enemy != null && !enemy.immuneToFreeze)
                enemy.GetComponent<NavMeshAgent>().isStopped = true;
        }

        //  2) Wait freezeDuration
        yield return new WaitForSeconds(freezeDuration);

        //  3) Unfreeze
        foreach (var enemy in enemyList)
        {
            if (enemy != null)
            {
                enemy.GetComponent<NavMeshAgent>().isStopped = false;

                //enemy.isEnraged = true;// <-- we'll add this  bool in EnemyMovementAnimated
                //enemy.GetComponent<NavMeshAgent>().speed += enragedSpeedBoost;

            }
        }

        // 4) Chain enrage for twice the freeze time
        float enrageTime = freezeDuration * 2f;
        Debug.Log($"[PowerUpManager] Now enraging for {enrageTime:F1}s");
        StartCoroutine(EnrageEnemiesTemporarily());

    }
    private IEnumerator EnrageEnemiesTemporarily()
    {
        foreach (var enemy in enemyList)
        {
            enemy.isEnraged = true;
            enemy.GetComponent<NavMeshAgent>().speed += enragedSpeedBoost;
        }

        yield return new WaitForSeconds(enrageDuration);

        foreach (var enemy in enemyList)
        {
            if (enemy != null)
            {
                {
                    enemy.isEnraged = false;
                    enemy.GetComponent<NavMeshAgent>().speed -= enragedSpeedBoost;
                }
            }
        }
    }





}