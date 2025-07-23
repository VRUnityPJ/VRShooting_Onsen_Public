namespace VRShooting.Scripts.Gun
{   
    /// <summary>
    /// 銃のタイプを表す変数
    /// </summary>
    public enum GunVariant
    {
        /// <summary>
        /// プロトタイプで制作された銃。本番環境では使われない予定
        /// </summary>
        ProtoTypeGun = 0,
        /// <summary>
        /// ただ単に撃てる銃
        /// </summary>
        NormalGun = 1,
        /// <summary>
        /// 爆発する弾を撃てる銃
        /// </summary>
        ExplosionGun = 2,
        /// <summary>
        /// 反射する弾を撃てる銃
        /// </summary>
        ReflectGun = 3,
        /// <summary>
        /// バースト射撃できる銃
        /// </summary>
        BurstGun = 4,
        /// <summary>
        /// フルオート射撃できる銃
        /// </summary>
        FullAutoGun = 5,
        /// <summary>
        /// フルオート射撃かつ反射する銃
        /// </summary>
        FullAutoReflectGun = 6,
    }
}