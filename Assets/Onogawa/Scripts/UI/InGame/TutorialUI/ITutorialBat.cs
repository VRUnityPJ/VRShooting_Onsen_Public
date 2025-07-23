using System;
using UniRx;

namespace Onogawa.Scripts.UI.InGame.TutorialUI
{
    public interface ITutorialBat
    {
        public IObservable<Unit> OnDead { get; }
    }
}