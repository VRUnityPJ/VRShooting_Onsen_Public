namespace VRShooting.Scripts.Enemy.Interfaces
{
    public interface IEnemyMovement
    {
        /// <summary>
        /// 移動させる関数
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Move(float deltaTime);
    }
}
