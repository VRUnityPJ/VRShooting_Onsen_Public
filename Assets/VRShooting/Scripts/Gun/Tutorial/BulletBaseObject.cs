using UnityEngine;

namespace VRShooting.Scripts.Gun
{
    /// <summary>
    /// 弾オブジェクトのクラス。BulletBehaviourを継承する。
    /// 新たに弾を作る際は、ここを参考にする。
    /// 反射や爆発といった機能は備えていないので、それらの処理を記述することになると思う。
    /// </summary>
    public class BulletBaseObject : BulletBehaviour
    {
        protected override void OnTriggerEnter(Collider other)
        {
            ReleaseSelf();
        }

        protected override void OnCollisionEnter(Collision other)
        {

        }

        protected override void MoveOnFixedUpdate()
        {
            // TODO: [CHANGE] RigidBodyで移動させる
            // 毎フレーム移動させる
            var speed = bulletData.GetFloatData(BulletBaseDataType.Speed);
            transform.position += moveDirection * (speed * Time.deltaTime);
        }
    }
}