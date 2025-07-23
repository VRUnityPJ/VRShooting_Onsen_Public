using Onogawa.Scripts.Point;
using Onogawa.Scripts.Point.interfaces;
using Onogawa.Scripts.UI.InGame;
using Onogawa.Scripts.UI.InGame.TutorialUI;
using VContainer;
using VContainer.Unity;

namespace Onogawa.Scripts.Utility
{
    /// <summary>
    /// ステージ1専用の依存関係を解決する
    /// </summary>
    public class Stage1LifeTimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IPointHolder, PointHolder>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<Ready>();
            builder.RegisterComponentInHierarchy<Stage1ResultUIViewer>().As<IResultUIViewer>();

        }
    }
}