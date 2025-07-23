namespace VRShooting.Scripts.Gun
{
    /// <summary>
    /// 新しく銃を作るときの参考にするスクリプト
    /// 射撃するときの処理をオーバライドして定義するぐらいしか書き換える部分はない。
    /// </summary>
    public class GunBaseObject : GunBehaviour
    {
        /// <summary>
        /// 弾を発射する
        /// </summary>
        public override void Shoot()
        {
            bulletPool.Get();
        }
    }
}


