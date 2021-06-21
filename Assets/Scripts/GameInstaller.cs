using Enemy;
using Player;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller, IInitializable
{
    public GameObject Player;
    public GameObject Camera;
    public Canvas canvas;
    public EnemyMarker[] EnemyMarkers;
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
        PlaneController planeController = Container.InstantiatePrefabForComponent<PlaneController>(Player);
        
        Container.Bind<PlaneController>().FromInstance(planeController).AsSingle();
    }

    private void BindCamera()
    {
        Camera camera = Container.InstantiatePrefabForComponent<Camera>(Camera);

        Container.Bind<Camera>().FromInstance(camera).AsSingle();
    }

    public void Initialize()
    {
        var enemyFactory = Container.Resolve<IEnemyFactory>();
        enemyFactory.Load();
        foreach (EnemyMarker marker in EnemyMarkers)
        {
            enemyFactory.Create(marker.transform.position);
        }
    }
}