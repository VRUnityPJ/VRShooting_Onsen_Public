using System.Collections.Generic;
using Onogawa.Scripts.Enemy.Firefly.interfaces;
using UnityEngine;

namespace Onogawa.Scripts.Enemy.Firefly
{
    public class FireflyMovement : MonoBehaviour, IFireflyMovement
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
        private IFireflyStateController _fireflyStateController;

        /// <summary>
        /// 自身のRigidBody
        /// </summary>
        private Rigidbody _rigidbody;

        /// <summary>
        /// 移動方向
        /// </summary>
        private Vector3 _moveDirection;

        /// <summary>
        /// プレイヤーのTransformを取得しているかどうか
        /// </summary>
        private bool _isGetPlayerTransform;

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
        private const float MAX_RANDOM_DIRECTION = 1f;

        private void Start()
        {
            // コンポーネントの取得
            // EnemyStateControllerを取得
            if (!TryGetComponent(out _fireflyStateController))
                Debug.Log("EnemyStateControllerがアタッチされていません");

            // RigidBodyを取得
            if (!TryGetComponent(out _rigidbody))
                Debug.Log("RigidBodyが取得できませんでした。");

            // ランダムな移動方向設定
            SetRandomMoveDirection(MIN_RANDOM_DIRECTION, MAX_RANDOM_DIRECTION);

            // イベントのサブスクライブ
            _fireflyStateController.OnEnterMove += SetPlayerTransform;
            _fireflyStateController.OnUpdateMove += Move;
            _fireflyStateController.OnUpdateMove += LookAtPlayer;
            _fireflyStateController.OnExitMove += Stop;
        }

        /// <summary>
        /// プレイヤーのTransformを取得して、_playerTransformに格納する
        /// </summary>
        private void SetPlayerTransform()
        {
            // _playerTransform = PlayerStorage.instance.GetPlayerEyeTransform();
            _isGetPlayerTransform = true;
        }

        private void Move(float deltaTime)
        {
            if (_rigidbody.velocity.magnitude >= _speed)
                return;

            _rigidbody.AddForce(_speed * deltaTime * _moveDirection, ForceMode.Impulse);
        }

        private void Stop()
        {
            _rigidbody.velocity = Vector3.zero;
        }

        /// <summary>
        /// プレイヤーの方向を向き続ける
        /// </summary>
        private void LookAtPlayer(float deltaTime)
        {
            transform.LookAt(Camera.main.transform);
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
            _moveDirection = Vector3.Reflect(_moveDirection, normal).normalized;

            // 反射方向へ力を加える
            var lastVelocity = _rigidbody.velocity;
            _rigidbody.velocity = _moveDirection * lastVelocity.magnitude * _elasticModulus;
        }
        /// <summary>
        /// ハードモード実装のための関数
        /// 倍率を受け取りスピードを上げる
        /// </summary>
        /// <param name="ratio"></param>
        public void SpeedUp(float ratio)
        {
            if (ratio < 1)
                return;

            _speed = _speed * ratio;
        }
    }
}
