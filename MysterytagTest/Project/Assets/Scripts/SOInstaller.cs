using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MysterytagTest
{
    [CreateAssetMenu(fileName = "SOInstaller", menuName = "Create SOInstaller")]
    public class SOInstaller : ScriptableObjectInstaller
    {
        [SerializeField]
        private GameSettings _gameSettings;

        [SerializeField]
        private InputSettings _inputSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(_gameSettings);
            Container.BindInstance(_inputSettings);
        }

    }
}
