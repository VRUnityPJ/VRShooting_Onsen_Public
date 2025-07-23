namespace VRShooting.Scripts.Enemy.Interfaces
{
    public interface IDamageable
    {
        /// <summary>
        /// 体力の値が変更されたときのイベント
        /// </summary>
        void TakeDamage(float damage);
    }
}
