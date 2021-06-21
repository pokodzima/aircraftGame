using Enemy;
using Player;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller, IInitializable
{
    public GameObject player;
    public GameObject cameraGameObject;
    public Canvas canvas;
    public EnemyMarker[] enemyMarkers;

    public override void InstallBindings()
    {
        BindInstallerInterfaces();
        BindCanvas();
        BindPlaneController();
        BindCamera();
        BindEnemyFactory();
    }

    private void BindEnemyFactory()
    {
        Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();
    }

    private void BindInstallerInterfaces()
    {
        Container.BindInterfacesTo<GameInstaller>().FromInstance(this).AsSingle();
    }

    private void BindCanvas()
    {
        Container.Bind<Canvas>().FromInstance(canvas);
    }

    private void BindPlaneController()
    {
        PlaneController planeController = Container.InstantiatePrefabForComponent<PlaneController>(player);

        Container.Bind<PlaneController>().FromInstance(planeController).AsSingle();
    }

    private void BindCamera()
    {
        Camera cameraComponent = Container.InstantiatePrefabForComponent<Camera>(cameraGameObject);

        Container.Bind<Camera>().FromInstance(cameraComponent).AsSingle();
    }

    public void Initialize()
    {
        var enemyFactory = Container.Resolve<IEnemyFactory>();
        enemyFactory.Load();
        foreach (EnemyMarker marker in enemyMarkers)
        {
            enemyFactory.Create(marker.transform.position);
        }
    }
}