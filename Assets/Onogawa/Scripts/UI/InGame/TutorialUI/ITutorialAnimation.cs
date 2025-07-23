using System;
using UniRx;

namespace Onogawa.Scripts.UI.InGame.TutorialUI
{
    public interface ITutorialAnimation
    {
        public IObservable<Unit> OnStartAnimationProcess { get; }
        public IObservable<Unit> OnEndAnimationProcess { get; }
    }
}