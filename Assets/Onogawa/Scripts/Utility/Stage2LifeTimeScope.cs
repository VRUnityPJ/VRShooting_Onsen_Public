using Onogawa.Scripts.Point;
using Onogawa.Scripts.Point.interfaces;
using Onogawa.Scripts.Stage;
using Onogawa.Scripts.UI.InGame;
using VContainer;
using VContainer.Unity;

namespace Onogawa.Scripts.Utility
{
    /// <summary>
    /// ステージ2専用の依存関係を解決する
    /// </summary>
    public class Stage2LifeTimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IPointHolder, PointHolder>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<GameStarter>();
            builder.RegisterComponentInHierarchy<Stage2ResultUIViewer>().As<IResultUIViewer>();
        }
    }
}