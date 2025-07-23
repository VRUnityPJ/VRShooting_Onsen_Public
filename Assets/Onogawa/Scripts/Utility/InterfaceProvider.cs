using System;
using NaughtyAttributes;
using UnityEngine;

namespace UnitScript
{
    //参考：https://qiita.com/yutorisan/items/9e1c7cc29abd5e8f271b

    /// <summary>
    /// inspectorでInterfaceとしてコンポーネントを受け取るためのクラス
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    [Serializable]
    public class InterfaceProvider<TInterface>
    {
        [SerializeField,Required]
        private GameObject gameObject;

        private TInterface _interface;

        public TInterface Interface
        {
            get
            {
                if (_interface == null)
                {
                    if (!gameObject.TryGetComponent<TInterface>(out _interface))
                        throw new InvalidOperationException("適したInterfaceが見つかりません");
                }
                return _interface;
            }
        }
    }
}

