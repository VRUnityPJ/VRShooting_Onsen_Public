using Onogawa.Scripts.Enemy.Firefly;
using Onogawa.Scripts.Point;
using Onogawa.Scripts.Stage;
using Onogawa.Scripts.UI.InGame;
using Onsen;
using VContainer;
using VContainer.Unity;
using VRShooting.Scripts.Stage.Interfaces;
using VRShooting.Scripts.UI;

namespace Onogawa.Scripts.Utility
{
    /// <summary>
    /// 依存関係を解決するクラス
    /// </summary>
    public class GameLifeTimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<EnemySpawner>(Lifetime.Singleton);
            builder.Register<FireflySearcher>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();;

            //MonoBehaviorクラス
            builder.RegisterComponentInHierarchy<StageChanger>();
            builder.RegisterComponentInHierarchy<DebugPoint>();
            builder.RegisterComponentInHierarchy<ScoreUIPresenter>();
            builder.RegisterComponentInHierarchy<OnsenSpawnerLevelScale>();
            builder.RegisterComponentInHierarchy<ResultUIPresenter>();
            builder.RegisterComponentInHierarchy<OnsenLevelStateController>().As<ILevelStateController>();

        }
    }
}