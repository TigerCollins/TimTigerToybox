using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] EnemySpawner[] enemySpawners;
    [SerializeField] List<Enemy> enemies = new List<Enemy>();

    public List<Enemy> EnemyList { get => enemies; set => enemies = value; }

    internal GameManager game;

    internal void Init()
    {
        InitEnemySpawners();
    }

    void InitEnemySpawners()
    {
        foreach (EnemySpawner enemySpawner in enemySpawners)
        {
            enemySpawner.player = game.Player.transform;
            enemySpawner.enemyManager = this;
            enemySpawner.init = true;
        }
    }

    internal void InitEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
        enemy.manager = this;
        enemy.player = game.Player.transform;
    }

    internal void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }
}
