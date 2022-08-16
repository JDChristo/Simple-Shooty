using System.Collections;
using System.Collections.Generic;
using SimpleShooty.Common;
using SimpleShooty.Game;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static List<Enemy> enemies;
    public PlayerController player { get => GameManager.Instance.Player; }

    private void Awake()
    {
        enemies = new List<Enemy>();
    }
    private void Start()
    {
        GameManager.Instance.OnStateUpdate += OnStateUpdate;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnStateUpdate -= OnStateUpdate;
    }
    private void OnStateUpdate(GameState status)
    {
        switch (status)
        {

            case GameState.Start:
                ActivateAllEnemy(true);
                break;

            case GameState.MainMenu:
            case GameState.Won:
            case GameState.Lost:
            default:
                ActivateAllEnemy(false);
                break;
        }
    }
    private void ActivateAllEnemy(bool enable)
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.Stop(!enable);
        }
    }
    public static void AddEnemy(Enemy enemy)
    {
        if (enemies == null)
        {
            enemies = new List<Enemy>();
        }
        enemies.Add(enemy);
    }
    public static void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    public Enemy GetClosetEnemy(float shootingRange)
    {
        float minimumDistance = Mathf.Infinity;
        Enemy closestEnemy = null;

        foreach (Enemy enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(enemy.transform.position, this.player.transform.position);

            if (distanceToEnemy <= shootingRange)
            {
                if (distanceToEnemy < minimumDistance)
                {
                    minimumDistance = distanceToEnemy;
                    closestEnemy = enemy;
                }
            }
        }
        return closestEnemy;
    }
}
