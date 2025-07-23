using System;

namespace VRShooting.Scripts.Gun
{
    /// <summary>
    /// 弾のデータにアクセスするインターフェース
    /// 弾のオブジェクトはこのインターフェースを必ず実装すること。
    /// </summary>
    public interface IBulletData
    {
        /// <summary>
        /// 弾の攻撃力
        /// </summary>
        public float attackPower { get; }
        
        /// <summary>
        /// 弾の最大速度
        /// </summary>
        public float maxSpeed { get; }
        
        /// <summary>
        /// 弾の生存時間
        /// </summary>
        public float lifeTime { get; }
        
        /// <summary>
        /// Int型の値を取得する
        /// </summary>
        /// <param name="key">値を取り出すためのキー</param>
        /// <typeparam name="TKey">任意のEnum型</typeparam>
        /// <returns></returns>
        public int GetIntData<TKey>(TKey key)
            where TKey : Enum;
        /// <summary>
        /// Float型の値を取得する
        /// </summary>
        /// <param name="key">値を取り出すためのキー</param>
        /// <typeparam name="TKey">任意のEnum型</typeparam>
        /// <returns></returns>
        public float GetFloatData<TKey>(TKey key)
            where TKey : Enum;
        /// <summary>
        /// bool型の値を取得する
        /// </summary>
        /// <param name="key">値を取り出すためのキー</param>
        /// <typeparam name="TKey">任意のEnum型</typeparam>
        /// <returns></returns>
        public bool GetBoolData<TKey>(TKey key)
            where TKey : Enum;
    }
}
