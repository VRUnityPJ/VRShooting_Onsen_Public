using NaughtyAttributes;
using Onogawa.Scripts.Point.interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace Onogawa.Scripts.Point
{
    public class DebugPoint : MonoBehaviour
    {
        [SerializeReference]
        [Inject]
        private IPointHolder _pointHolder;

        [ContextMenu("シーン遷移"),Button]
        public void ChangeScene()
        {
            SceneManager.LoadScene("PointNext");
        }

        [Button]
        public void Add()
        {
            Debug.Log("Add");
            _pointHolder.AddPoint(10);
        }

        [Button]
        public void Sub()
        {
            Debug.Log("Sub");
            _pointHolder.SubPoint(10);
        }

        [Button]
        public void Reset()
        {
            Debug.Log("Reset");
            _pointHolder.InitializePoint();
        }

        [Button]
        public void Rank()
        {
            Debug.Log(_pointHolder.GetRank());
        }

        [Button]
        public void Show()
        {
            Debug.Log($"point : {_pointHolder.Point}");
        }
    }
}