namespace VRShooting.Scripts.Gun
{
    /// <summary>
    /// 弾の状態
    /// </summary>
    public enum BulletState
    {
        /// <summary>
        /// 銃から発射されて何も起きていない状態
        /// </summary>
        Normal,
        /// <summary>
        /// 反射した後の状態
        /// </summary>
        Reflection,
    }
}