using System;
using Onogawa.Scripts.Point.interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VRShooting.Scripts.Stage.Interfaces;
using UniRx;

namespace Onogawa.Scripts.Enemy.Firefly
{
    public class FireflySearcher : IStartable, IDisposable
    {
        [Inject] private IPointHolder _pointHolder;
        [Inject] private ILevelStateController _lsc;

        private const string FireflyTag = "Firefly";
        private int _sumFireflies;
        private int _currentFireflies;
        private IDisposable _disposable;

        /// <summary>
        /// 最初のホタル数
        /// </summary>
        public int SumFireflies => _sumFireflies;

        /// <summary>
        /// 現在のホタル数
        /// </summary>
        public int CurrentFireflies => _currentFireflies;

        /// <summary>
        /// 残りホタル数をカウントし、ポイントに加算する
        /// </summary>
        private void OnSearch()
        {
            _currentFireflies = GameObject.FindGameObjectsWithTag(FireflyTag).Length;
            _pointHolder.AddRestFireflyPoint(_currentFireflies);
        }

        public void Start()
        {
            _sumFireflies = GameObject.FindGameObjectsWithTag(FireflyTag).Length;
            _disposable = _lsc.OnEndGame
                .Subscribe(_ =>
                {
                    OnSearch();
                });
        }

        public void Dispose() => _disposable?.Dispose();
    }
}