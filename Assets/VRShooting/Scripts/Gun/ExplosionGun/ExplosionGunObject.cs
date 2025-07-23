using UnityEngine;

namespace VRShooting.Scripts.Gun
{
    public class ExplosionGunObject : GunBehaviour
    {
        public override void Shoot()
        {
            // オブジェクトプールから弾を取り出す
            // 銃側のスクリプトはこれだけで十分機能を果たす
            bulletPool.Get();
        }
    }
}