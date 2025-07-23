using System;
using UniRx;

namespace Onogawa.Scripts.UI.InGame
{
    public interface IResultUIViewer
    {
        /// <summary>
        /// ポイントとホタル数などを表示する
        /// </summary>
        public void View(ResultViewModel result);

        public IObservable<Unit> OnFinishedResult { get; }
    }
}