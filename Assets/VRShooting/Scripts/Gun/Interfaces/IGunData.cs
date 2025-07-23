using System;

namespace VRShooting.Scripts.Gun
{
    public interface IGunData
    {
        /// <summary>
        /// この銃に割り当てられている弾のオブジェクト
        /// </summary>
        public IBullet bulletObj { get; }
        
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