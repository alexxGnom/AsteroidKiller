using UnityEngine;
using Zenject;

namespace MysterytagTest
{
    public class Player : MonoBehaviour
    {

        #region Unity properties

        [SerializeField]
        private Transform _gun;

        #endregion

        #region Injects

        [Inject]
        private float _speed = 0f;

        [Inject]
        private InputController _inputController;

        [Inject]
        private Missile.Pool _missilePool;

        [Inject]
        private GameController _gameController;

        [Inject]
        private GameSettings _gameSettings;

        [Inject]
        private WeaponController _weaponController;

        #endregion

        #region Private fields

        private Rigidbody2D _rigidbody;

        private float _lastShootTime;

        #endregion

        #region Interface

        public void Shoot()
        {
            var weapon = _weaponController.Weapon;

            var missile = _missilePool.Spawn(weapon.Damage, weapon.Speed, _gameSettings.MissileLifeTime, weapon.Color);

            missile.transform.position = _gun.transform.position;

            missile.transform.rotation = transform.rotation;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            transform.position = Vector3.zero;
        }

        #endregion

        #region Utils

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            _lastShootTime = Time.realtimeSinceStartup;
        }

        private void Update()
        {
            LookAtCursor();

            if (_inputController.IsMovingLeft)
            {
                _rigidbody.AddForce(
                    Vector3.left * _speed);
            }

            if (_inputController.IsMovingRight)
            {
                _rigidbody.AddForce(
                    Vector3.right * _speed);
            }

            if (_inputController.IsMovingUp)
            {
                _rigidbody.AddForce(
                    Vector3.up * _speed);
            }

            if (_inputController.IsMovingDown)
            {
                _rigidbody.AddForce(
                    Vector3.down * _speed);
            }

            KeepOnScreen();

            if (_inputController.IsFiring && (Time.realtimeSinceStartup - _lastShootTime > _weaponController.Weapon.ChargeTime))
            {
                _lastShootTime = Time.realtimeSinceStartup;
                Shoot();
            }

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _gameController.SetEndLevel();
        }

        private void KeepOnScreen()
        {
            var scrH = Screen.height;
            var scrW = Screen.width;

            var extentLeft = Camera.main.WorldToScreenPoint(transform.position).x - _gameSettings.EdgeDelta;
            var extentRight = (scrW - _gameSettings.EdgeDelta) - Camera.main.WorldToScreenPoint(transform.position).x;

            var extentTop = (scrH - _gameSettings.EdgeDelta) - Camera.main.WorldToScreenPoint(transform.position).y;
            var extentBottom = Camera.main.WorldToScreenPoint(transform.position).y - _gameSettings.EdgeDelta;

            if (extentLeft < 0)
            {
                _rigidbody.AddForce(
                    Vector3.right * _gameSettings.EdgeReflectForce * (-extentLeft));
            }
            else if (extentRight < 0)
            {
                _rigidbody.AddForce(
                    Vector3.left * _gameSettings.EdgeReflectForce * (-extentRight));
            }

            if (extentTop < 0)
            {
                _rigidbody.AddForce(
                    Vector3.down * _gameSettings.EdgeReflectForce * (-extentTop));
            }
            else if (extentBottom < 0)
            {
                _rigidbody.AddForce(
                    Vector3.up * _gameSettings.EdgeReflectForce * (-extentBottom));
            }
        }

        private void LookAtCursor()
        {
            var direction = _inputController.GetMousePosition() - transform.position;
            var angle = -Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = rotation;
        }

        #endregion

        public class PlayerFactory : Factory<float, Player> { }
    }

    

}
