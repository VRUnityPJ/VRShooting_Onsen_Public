using System;
using Onogawa.Scripts.Point.interfaces;
using Onogawa.Scripts.Stage;
using UnityEngine;
using VContainer;

namespace Onogawa.Scripts.Point
{
    [Serializable]
    public sealed class PointHolder : IPointHolder
    {
        [SerializeField] private int _point = 0;
        [SerializeField] private int _restFireflyPoint = 500;

        [Space]
        [SerializeField] private int _sRankBorder;
        [SerializeField] private int _aRankBorder;
        [SerializeField] private int _bRankBorder;

        [Inject] private readonly TotalPointHolder _total;

        public int Point => _point;
        public int RestFireflyPoint => _restFireflyPoint;

        /// <summary>
        /// ポイントを初期化　
        /// </summary>
        public void InitializePoint() => _point = 0;

        /// <summary>
        /// 残りホタル数に応じてポイントを加算
        /// </summary>
        /// <param name="fireflyCount">残ったホタルの数</param>
        public void AddRestFireflyPoint(int fireflyCount) => AddPoint(fireflyCount * _restFireflyPoint);

        /// <summary>
        /// 加算
        /// </summary>
        /// <param name="value"></param>
        public void AddPoint(int value)
        {
            _point += value;
            _total.AddTotalPoint(value);
        }

        /// <summary>
        /// 減算
        /// </summary>
        /// <param name="value"></param>
        public void SubPoint(int value)
        {
            if (_point < value)
            {
                _point = 0;
                return;
            }

            _point -= value;
        }

        /// <summary>
        /// 総合ポイントを返す
        /// </summary>
        /// <returns></returns>
        public int GetTotalPoint() => _total.TotalPoints;

        /// <summary>
        /// スコアに応じたランクを取得する
        /// </summary>
        /// <returns></returns>
        public Rank GetRank()
        {
            if (_point >= _sRankBorder) return Rank.S;
            if (_point >= _aRankBorder) return Rank.A;
            if (_point >= _bRankBorder) return Rank.B;

            return Rank.C;
        }
    }
}