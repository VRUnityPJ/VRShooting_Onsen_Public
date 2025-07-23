using Onogawa.Scripts.Point;
using Onogawa.Scripts.Point.interfaces;
using VContainer;
using VContainer.Unity;

namespace Onogawa.Scripts.Utility
{
    /// <summary>
    /// Pointの依存関係を解決する
    /// </summary>
    public class PointLifeTimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder) => builder.Register<TotalPointHolder>(Lifetime.Singleton);
    }
}
