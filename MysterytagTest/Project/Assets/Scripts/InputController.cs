using UnityEngine;
using Zenject;

namespace MysterytagTest
{
    public class InputController : MonoBehaviour
    {
        #region Injects
        
        [Inject]
        private InputSettings _inputSettings;

        [Inject]
        private GameController _gameController;

        #endregion

        #region Public properties

        public bool IsMovingUp { get; private set; }

        public bool IsMovingDown { get; private set; }

        public bool IsMovingRight { get; private set; }
        
        public bool IsMovingLeft { get; private set; }
        
        public bool IsFiring { get; private set; }

        #endregion

        #region Interface()

        public Vector3 GetMousePosition()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        #endregion

        #region Utils

        private void Start()
        {
            ResetInput();
        }

        private void Update()
        {
            if (!_gameController.IsStarted) return;

            IsMovingUp = Input.GetKey(_inputSettings.UpMoveBttn);
            IsMovingDown = Input.GetKey(_inputSettings.DownMoveBttn);
            IsMovingRight = Input.GetKey(_inputSettings.RightMoveBttn);
            IsMovingLeft = Input.GetKey(_inputSettings.LeftMoveBttn);
            IsFiring = Input.GetKey(_inputSettings.FireBttn) || Input.GetMouseButton(0);
        }

        private void ResetInput()
        {
            IsMovingUp = IsMovingDown = IsMovingRight = IsMovingLeft = IsFiring = false;
        }

        #endregion
    }
}
