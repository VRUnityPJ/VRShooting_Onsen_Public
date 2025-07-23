
using System.Collections.Generic;
using UnityEngine;
using VRShooting.Scripts.Enemy.Interfaces;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using Onogawa.Scripts.Enemy.Firefly.interfaces;
using UnityEngine.Serialization;

namespace VRShooting.Scripts.Enemy
{
    public class BatMovement : MonoBehaviour, IBatMovement
    {
#region インスペクタ変数
        /// <summary>
        /// Idle時の移動スピード
        /// </summary>
        [SerializeField,Tooltip("Idleのスピード")]
        private float _idleSpeed = 1f;

        /// <summary>
        /// ターゲットサーチ範囲の初期値
        /// </summary>
        [SerializeField,Tooltip("ターゲットサーチ範囲の初期値")]
        private float _initialSearchRange = 1f;

        /// <summary>
        /// ターゲットサーチ範囲の拡大量
        /// </summary>
        [SerializeField,Tooltip("ターゲットサーチ範囲の拡大量")]
        private float _searchRangeScaleAmount = 5f;

        /// <summary>
        /// ターゲットサーチのインターバル(秒)
        /// </summary>
        [SerializeField,Tooltip("ターゲットサーチのインターバル(秒)")]
        private int _SerachIntervalSec = 3;

        /// <summary>
        /// ターゲットサーチ範囲の最大距離
        /// </summary>
        [SerializeField,Tooltip("ターゲットサーチ範囲の最大距離")]
        private int _maxSearchRange = 100;

        /// <summary>
        /// つかみ可能距離
        /// </summary>
        [SerializeField,Tooltip("つかみ可能距離")]
        private float _holdabledRange = 2f;

        /// <summary>
        /// 掴み状態の周りを回る長さ
        /// </summary>
        [SerializeField,Tooltip("Hold時の円形移動の半径")]
        private float _holdRadius = 2f;

        /// <summary>
        /// 掴み時間
        /// </summary>
        [SerializeField,Tooltip("つかみ時間")]
        private float _maxHoldTime = 1f;

        /// <summary>
        /// 掴み時の相対角速度
        /// </summary>
        [SerializeField,Tooltip("掴み時の相対角速度")]
        private float _holdSpeed = 1f;

        /// <summary>
        /// 壁との弾性
        /// </summary>
        [SerializeField,Tooltip("壁との弾性")]
        private float _elasticModulus = 0.8f;

        /// <summary>
        /// ViewオブジェクトのTransform
        /// </summary>
        [SerializeField] private Transform _viewTransform;
#endregion

        /// <summary>
        /// 敵の状態によってイベントを発行する
        /// </summary>
        private IBatStateController _batStateController;

        /// <summary>
        /// 自身のRigidBody
        /// </summary>
        private Rigidbody _rigidbody;

        /// <summary>
        /// ターゲット(蛍)の位置
        /// </summary>
        private Transform _targetTransform = null;

        /// <summary>
        /// 移動方向
        /// </summary>
        private Vector3 _moveDirection;

        /// <summary>
        /// 現在の捕獲時間
        /// </summary>
        private float _holdingTime = 0;

        /// <summary>
        /// ターゲットを見逃す距離
        /// </summary>
        private float _missRange = 1000;

        /// <summary>
        /// ターゲットを見つけているか
        /// </summary>
        private bool _isTargetFound = false;

        private CancellationToken _token;

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

        private void Awake()
        {
            // コンポーネントの取得
            // EnemyStateControllerを取得
            if (!TryGetComponent(out _batStateController))
                Debug.Log("EnemyStateControllerがアタッチされていません");
            // RigidBodyを取得
            if (!TryGetComponent(out _rigidbody))
                Debug.Log("RigidBodyが取得できませんでした。");

            _token = this.GetCancellationTokenOnDestroy();

            // イベントのサブスクライブ
            _batStateController.OnEnterIdle += IdleStart;
            _batStateController.OnUpdateIdle += Move;
            _batStateController.OnUpdateIdle += LookPlayer;

            _batStateController.OnEnterFound += () => Debug.Log("Find");
            _batStateController.OnUpdateFound += Chase;
            _batStateController.OnUpdateFound += LookTarget;

            _batStateController.OnEnterHold += StartHold;
            _batStateController.OnEnterHold += () => SetLayer("HoldingBat");
            _batStateController.OnUpdateHold += MoveAroundTarget;
            _batStateController.OnUpdateHold += CountHoldTime;
            _batStateController.OnUpdateHold += LookPlayer;
            _batStateController.OnExitHold += () => SetLayer("Enemy");
            _batStateController.OnExitHold += Reset;

        }

        private void IdleStart()
        {
            //ランダム方向に進行
            SetRandomMoveDirection(MIN_RANDOM_DIRECTION,MAX_RANDOM_DIRECTION);
            //ホタルを探す
            SearchTarget(_token).Forget();
        }

        /// <summary>
        /// 進行方向に動く
        /// </summary>
        private void Move(float deltaTime)
        {
            // LookTarget(_moveDirection);
            transform.Translate(_moveDirection * _idleSpeed * deltaTime);
        }

        /// <summary>
        /// 最寄りの対象のTransformを取得して、_targetTransformに格納する
        /// </summary>
        private async UniTask SearchTarget(CancellationToken token)
        {
            var searchRange = _initialSearchRange;

            //最大サーチ距離までサーチを繰り返す
            for ( ;searchRange < _maxSearchRange; )
            {
                Debug.Log($"サーチ開始 距離{searchRange}");
                await UniTask.Delay(_SerachIntervalSec*1000,cancellationToken:token);

                //ホタルオブジェクトをサーチ
                var hits = Physics.SphereCastAll(
                    this.transform.position,
                    searchRange,
                    this.transform.forward)
                    .Select(h => h.transform.gameObject)
                    .Where(obj => obj.TryGetComponent<IFireflyMovement>(out var _))
                    .ToList();

                //Search範囲を拡大
                searchRange += _searchRangeScaleAmount;

                //ホタルオブジェクトがなければ
                if (!hits.Any())
                    continue;

                //最寄りのホタルを取得
                float minTargetDistance = float.MaxValue;
                foreach (var hit in hits)
                {
                    float targetDistance = Vector3.Distance(this.transform.position, hit.transform.position);
                    if (targetDistance < minTargetDistance)
                    {
                        minTargetDistance = targetDistance;
                        _targetTransform = hit.transform;
                    }
                }

                //ホタルを見つけたら終了
                if (_targetTransform != null)
                {
                    _batStateController.ExecuteTrigger(BatStateTrigger.ToFound);
                    return;
                }
            }

            Debug.Log("サーチ終了");
        }

        /// <summary>
        /// ホタルを追う関数
        /// </summary>
        private void Chase(float deltaTime)
        {
            //ホタルが消えたら
            if (_targetTransform == null)
            {
                _batStateController.ExecuteTrigger(BatStateTrigger.ToIdle);
                return;
            }

            _moveDirection = (_targetTransform.position - transform.position).normalized;
            Move(deltaTime);

            float targetDistance = Vector3.Distance(transform.position, _targetTransform.position);

            //ターゲットを捉える
            if (targetDistance < _holdabledRange)
            {
                _batStateController.ExecuteTrigger(BatStateTrigger.ToHold);
                return;
            }
            //ターゲットを見逃す
            if (targetDistance > _missRange)
            {
                _targetTransform = null;
                _batStateController.ExecuteTrigger(BatStateTrigger.ToIdle);
                return;
            }
        }

        private void StartHold()
        {
            // this.transform.position = _targetTransform.position + new Vector3(_holdRadius, 0, 0);
            transform.parent = _targetTransform.gameObject.transform;
            _holdingTime = 0;
        }

        /// <summary>
        ///
        /// </summary>
        private void MoveAroundTarget(float deltaTime)
        {
            if (_targetTransform == null)
            {
                _batStateController.ExecuteTrigger(BatStateTrigger.ToIdle);
                return;
            }

            // 時間ベースの角度（回転速度 × 経過時間）
            float angle = _holdingTime * _holdSpeed;
            float progressHoldTime = (_maxHoldTime - _holdingTime) / _maxHoldTime;

            float ratio = _maxHoldTime / (_maxHoldTime - _holdabledRange);

            // 円軌道を計算（XZ平面上）
            float offsetX = Mathf.Cos(angle) * _holdRadius*progressHoldTime;
            float offsetY = Mathf.Sin(angle) * _holdRadius*progressHoldTime;
            var localOffset = new Vector3(offsetX, offsetY, 0);


            // ターゲットの現在の位置にオフセットを加える
            Vector3 newPosition = _targetTransform.position + localOffset;

            // スムーズに移動させる
            transform.localPosition = Vector3.Lerp(transform.localPosition, localOffset, deltaTime * _holdSpeed * ratio);
            // transform.LookAt(_targetTransform.position);
        }

        private void CountHoldTime(float deltaTime)
        {
            _holdingTime += Time.deltaTime;

            if (!(_holdingTime > _maxHoldTime))
                return;
            if (_targetTransform == null)
                return;

            //ホタルを攻撃
            Attack();
        }

        private void Attack()
        {
            if (!_targetTransform.gameObject.TryGetComponent(out IFireflyHealth firefly))
                return;

            Debug.Log("attack");

            firefly.ReceiveAttack();

            _batStateController.ExecuteTrigger(BatStateTrigger.ToIdle);
        }

        private void Reset()
        {
            //Hold→Idleにあたっていろいろとリセット
            _targetTransform = null;
            _holdingTime = 0;
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
            Bounce(collision);
        }

        //反射して方向転換する関数
        private void Bounce(Collision collision)
        {
            // 反射方向のベクトルを取得する。
            var normal = collision.contacts[0].normal;
            _moveDirection = Vector3.Reflect(this._moveDirection, normal).normalized;

            // 反射方向へ力を加える
            var lastVelocity = _rigidbody.velocity;
            _rigidbody.velocity = _moveDirection * lastVelocity.magnitude * _elasticModulus;
        }

        private void LookPlayer(float _)
        {
            _viewTransform.LookAt(Camera.main.transform);
        }

        private void LookTarget(float _)
        {
            if(_targetTransform == null)
                return;
            _viewTransform.LookAt(_targetTransform);
        }

        private void SetLayer(string layerName)
        {
            if(LayerMask.NameToLayer(layerName) == -1)
                Debug.Log("Layerが存在しません");
            gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }
}