namespace Onogawa.Scripts.Point
{
    /// <summary>
    /// 総ポイント数など管理するクラス
    /// </summary>
    public sealed class TotalPointHolder
    {
        private int _totalPoints;

        /// <summary>
        /// 総合ポイント
        /// </summary>
        public int TotalPoints => _totalPoints;

        /// <summary>
        /// 加算
        /// </summary>
        /// <param name="value"></param>
        public void AddTotalPoint(int value) => _totalPoints += value;
    }
}