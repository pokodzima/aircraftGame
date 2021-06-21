using UnityEngine;
using Zenject;

public class EnemyFactory: IEnemyFactory
{
    private readonly DiContainer _diContainer;
    private readonly Transform canvasTransform;
 
    private const string EnemyPath = "Enemy";
    private const string MarkPrefab = "Mark";

    private Object _enemyPrefab;
    private Object _markPrefab;

    public EnemyFactory(DiContainer diContainer, Canvas canvas)
    {
        _diContainer = diContainer;
        canvasTransform = canvas.transform;
    }
    public void Load()
    {
        _enemyPrefab = Resources.Load(EnemyPath);
        _markPrefab = Resources.Load(MarkPrefab);
    }

    public void Create(Vector3 at)
    {
        var enemy = _diContainer.InstantiatePrefab(_enemyPrefab, at, Quaternion.identity,null);
        var mark = _diContainer.InstantiatePrefab(_markPrefab, canvasTransform);
        mark.GetComponent<Mark>().relatedEnemyTransform = enemy.transform;
        mark.GetComponent<Mark>().relatedEnemy = enemy.GetComponent<Enemy.Enemy>();
    }
}