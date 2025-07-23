using System;
using UniRx;
using UnityEngine;
namespace VRShooting.Scripts.Player
{
    public class MessageDisplayer : MonoBehaviour
    {

        [SerializeField]
        private PlayerInputManager _someInpute;

        private void Start()
        {
            IObserver<Unit> observer = new ValueDisplayObserver();
            _someInpute.OnTriggerShoot
                .Subscribe(observer)
                .AddTo(this);
        }

        private class ValueDisplayObserver : IObserver<Unit>
        {

            void IObserver<Unit>.OnCompleted()
            {
                throw new NotImplementedException();
            }

            void IObserver<Unit>.OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            void IObserver<Unit>.OnNext(Unit X)//ここ意味わからんXパラメータなんて作ったっけ？そもそもUnit単体で書くとエラーなのはなぜ
            {
                //throw new NotImplementedException();
                Debug.Log("Shot");
            }
        }
    }
}
