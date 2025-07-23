namespace Onogawa.Scripts.UI.InGame
{
    public struct ResultViewModel
    {
        /// <summary>
        /// 現時点でのポイント
        /// </summary>
        public readonly int CurrentPoint;

        /// <summary>
        /// 総合ポイント
        /// </summary>
        public readonly int TotalPoint;

        /// <summary>
        /// ボーナスポイント
        /// </summary>
        public readonly int BonusPoint;

        /// <summary>
        /// ステージ上に存在していたホタル数
        /// </summary>
        public readonly int SumFirefly;

        /// <summary>
        /// 生き残ったホタル数
        /// </summary>
        public readonly int CurrentFirefly;

        public ResultViewModel(int currentPoint, int totalPoint, int bonus, int sumFirefly, int currentFirefly)
        {
            CurrentFirefly = currentFirefly;
            TotalPoint = totalPoint;
            BonusPoint = bonus;
            CurrentPoint = currentPoint;
            SumFirefly = sumFirefly;
        }
    }
}