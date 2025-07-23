using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using VContainer;
using VRShooting.Scripts.Enemy.Interfaces;
using VRShooting.Scripts.Stage;

namespace VRShooting.Scripts.Enemy
{
    public class EnemyMovement : MonoBehaviour, IEnemyMovement
    {
        /// <summary>
        /// 移動スピード
        /// </summary>
        [SerializeField] private float _speed = 1f;

        [SerializeField] private float _elasticModulus = 0.8f;

        /// <summary>
        /// 見た目のオブジェクトのTransform
        /// </summary>
        [SerializeField] private Transform _viewTransform;

        /// <summary>
        /// 敵の状態によってイベントを発行する
        /// </summary>
        private IEnemyStateController _enemyStateController;

        /// <summary>
        /// 自身のRigidBody
        /// </summary>
        private Rigidbody _rigidbody;

        /// <summary>
        /// プレイヤーの位置
        /// </summary>
        private Transform _playerTransform;

        /// <summary>
        /// 移動方向
        /// </summary>
        private Vector3 _moveDirection;

        /// <summary>
        /// プレイヤーのTransformを取得しているかどうか
        /// </summary>
        private bool _isGetPlayerTransform = false;

        private readonly List<Vector3> _directionList = new()
        {
            Vector3.up,
            Vector3.down,
            Vector3.forward,
            Vector3.back,
            Vector3.left,
            Vector3.right
        };

        private const float MIN_RANDOM_DIRECTION = -1f;
        private const float MAX_RANDOM_DIRECTION =  1f;

        private void Start()
        {
            // コンポーネントの取得
            // EnemyStateControllerを取得
            if(!TryGetComponent(out _enemyStateController))
                Debug.Log("EnemyStateControllerがアタッチされていません");
            // RigidBodyを取得
            if(!TryGetComponent(out _rigidbody))
                Debug.Log("RigidBodyが取得できませんでした。");

            // ランダムな移動方向設定
            SetRandomMoveDirection(MIN_RANDOM_DIRECTION, MAX_RANDOM_DIRECTION);

            // イベントのサブスクライブ
            _enemyStateController.OnEnterMove += SetPlayerTransform;
            _enemyStateController.OnUpdateMove += Move;
            _enemyStateController.OnUpdateMove += LookAtPlayer;
            _enemyStateController.OnExitMove += Stop;

            //ハードモードの設定

            _speed *= 1.3f;
        }

        /// <summary>
        /// プレイヤーのTransformを取得して、_playerTransformに格納する
        /// </summary>
        private void SetPlayerTransform()
        {
            _playerTransform = PlayerStorage.instance.GetPlayerEyeTransform();
            _isGetPlayerTransform = true;
        }

        public void Move(float deltaTime)
        {
            if (_rigidbody.velocity.magnitude >= _speed) return;

            _rigidbody.AddForce(_speed * deltaTime * _moveDirection, ForceMode.Impulse);
        }

        private void Stop()
        {
            _rigidbody.velocity = Vector3.zero;
        }

        /// <summary>
        /// プレイヤーの方向を向き続ける
        /// </summary>
        /// <param name="deltaTime"></param>
        private void LookAtPlayer(float deltaTime)
        {
            // プレイヤーの位置を取得していなかったら取得する
            if(!_isGetPlayerTransform)
                SetPlayerTransform();

            // プレイヤーへの方向ベクトルを計算
            var playerDirection = (_playerTransform.position - transform.position).normalized;

            // プレイヤーの方向を向く
            _viewTransform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);

            // var deltaRotationX = Mathf.Lerp(transform.forward.x, playerDirection.x, deltaTime);
        }

        /// <summary>
        /// ランダムな移動方向を設定する。
        /// _moveDirectionにランダムな方向のベクトルをセットしている。
        /// </summary>
        /// <param name="minValue">ランダムに取りえる最小値</param>
        /// <param name="maxValue">ランダムに取りえる最大値</param>
        private void SetRandomMoveDirection(float minValue, float maxValue)
        {
            // シード値を設定
            Random.InitState(System.DateTime.Now.Millisecond);

            // ランダムな値を取得
            var randomX = Random.Range(minValue, maxValue);
            var randomY = Random.Range(minValue, maxValue);
            var randomZ = Random.Range(minValue, maxValue);

            // 移動方向を変更
            _moveDirection = new Vector3(randomX, randomY, randomZ).normalized;
        }

        private void OnCollisionEnter(Collision collision)
        {
            // 反射方向のベクトルを取得する。
            var normal = collision.contacts[0].normal;
            _moveDirection = Vector3.Reflect(this._moveDirection, normal).normalized;

            // 反射方向へ力を加える
            var lastVelocity = _rigidbody.velocity;
            _rigidbody.velocity = _moveDirection * lastVelocity.magnitude * _elasticModulus;
        }
        /// <summary>
        /// ハードモード実装のための関数
        /// 倍率を受け取りスピードを上げる
        /// </summary>
        /// <param name="_ratio"></param>
        public void SpeedUp(float _ratio)
        {
            if(_ratio < 1)
                return;

            _speed = _speed*_ratio;
        }
    }
}
