using System;
using DG.Tweening;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Onogawa.Scripts.UI.InGame
{
    public class Stage2ResultUIViewer : MonoBehaviour, IResultUIViewer
    {
        /// <summary>
        /// スコア表示用のテキスト
        /// </summary>
        [SerializeField, Required]
        private Text _scoreText;

        /// <summary>
        /// 合計スコア表示用のテキスト
        /// </summary>
        [SerializeField, Required]
        private Text _sumScoreText;

        /// <summary>
        /// 総ホタル数など表示用
        /// </summary>
        [SerializeField, Required]
        private Text _firefliesText;

        /// <summary>
        /// 総ホタル数など表示用
        /// </summary>
        [SerializeField, Required]
        private Text _addPointsText;

        // TODO: デバッグ用に数値を入れてる
        /// <summary>
        /// ボーナスが入る前のポイント
        /// </summary>
        private int _normalPoint = 500;

        /// <summary>
        /// ボーナスポイント
        /// </summary>
        private int _bonus = 100;

        private readonly Subject<Unit> _onFinishedAnimation = new();

        public IObservable<Unit> OnFinishedResult => _onFinishedAnimation;

        public void View(ResultViewModel result)
        {
            _normalPoint = result.CurrentPoint - result.BonusPoint;
            _bonus = result.BonusPoint;

            // テキストの書き換え
            _scoreText.text = $"現在のスコア ： {_normalPoint}";
            _firefliesText.text = $"生き残ったホタル数 : {result.CurrentFirefly}/{result.SumFirefly}";
            _addPointsText.text = $"ホタルボーナス!!! +{_bonus}";
            _sumScoreText.text = $"合計スコア : {result.TotalPoint}";
            // UIを表示
            gameObject.SetActive(true);

            OnAnimation();
        }

        [Button]
        private void OnAnimation()
        {
            _scoreText.rectTransform.localScale = Vector3.zero;
            _firefliesText.rectTransform.localScale = Vector3.zero;
            _addPointsText.rectTransform.localScale = Vector3.zero;
            _sumScoreText.rectTransform.localScale = Vector3.zero;

            var sequence = DOTween.Sequence();

            sequence
                .Append(_scoreText.transform.DOScale(Vector3.one, 0.5f))
                .Append(_firefliesText.transform.DOScale(Vector3.one, 0.5f))
                .Append(_addPointsText.transform.DOScale(Vector3.one, 0.5f))
                .AppendInterval(0.25f)
                .Append(
                    DOTween.To(() => _normalPoint, v =>
                    {
                        _scoreText.text = $"現在のスコア ：{v:N0}";
                    }, _normalPoint + _bonus, 1.0f)
                )
                .AppendInterval(1f)
                .Append(_scoreText.transform.DOScale(Vector3.zero, 0.1f))
                .Append(_firefliesText.transform.DOScale(Vector3.zero, 0.1f))
                .Append(_addPointsText.transform.DOScale(Vector3.zero, 0.1f))
                .Append(_sumScoreText.transform.DOScale(Vector3.one, 0.5f))
                .AppendInterval(2f)
                .AppendCallback(() => _onFinishedAnimation.OnNext(Unit.Default));
        }

        private void OnDestroy() => _onFinishedAnimation.Dispose();
    }
}