using UnityEngine;
using Zenject;

namespace MysterytagTest
{
    public class Asteroid : MonoBehaviour
    {
        #region Unity properties

        [SerializeField]
        private Sprite[] _sprites;
        
        #endregion

        #region Public properties

        public int Health 
        {
            get 
            {
                return _health;
            } 

            private set
            {
                _health = value;

                if (_health == 0)
                    Despawn();
            } 
        }

        #endregion

        #region Injects

        [Inject]
        private Pool _asteroidPool;

        [Inject]
        private AsteroidGenerator _asteroidGenerator;

        [Inject]
        private AsteroidDamageSignal _damageSignal;

        [Inject]
        private GameSettings _gameSettings;

        #endregion

        #region Private fields

        private int _health;

        private Rigidbody2D _rigidbody;

        private float _speed;

        private GUIStyle _healthStyle;

        private SpriteRenderer _spriteRenderer;

        #endregion

        #region Interface

        public void SetDamage(int damage)
        {
            damage = Mathf.Min(Health, damage);

            Health -= damage;

            _damageSignal.Fire(damage);
        }

        #endregion

        #region Utils

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            _spriteRenderer = GetComponent<SpriteRenderer>();

            _healthStyle = new GUIStyle();
            _healthStyle.normal.textColor = Color.red;
            _healthStyle.fontSize = 40;

        }

        private  void Update()
        {
            var velocity = _rigidbody.velocity;
            velocity.Normalize();

            _rigidbody.velocity = velocity * _speed;

            KeepOnScreen();
        }

        private void Start()
        {
            SetStartSettings();
        }

        private void OnGUI()
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.localPosition);

            GUI.Label(new Rect(pos.x, Screen.height - pos.y, 100, 100), Health.ToString(), _healthStyle);

        }

        private void SetStartSettings()
        {
            if (_spriteRenderer != null && _sprites.Length > 0)
                _spriteRenderer.sprite = _sprites[Random.Range(0, _sprites.Length)];
            
            var moveDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

            moveDirection.Normalize();

            _rigidbody.velocity = moveDirection * _speed;
        }

        private void KeepOnScreen()
        {
            var scrH = Screen.height;
            var scrW = Screen.width;

            var extentLeft = Camera.main.WorldToScreenPoint(transform.position).x;
            var extentRight = scrW - Camera.main.WorldToScreenPoint(transform.position).x;

            var extentTop = scrH - Camera.main.WorldToScreenPoint(transform.position).y;
            var extentBottom = Camera.main.WorldToScreenPoint(transform.position).y;

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

        #endregion

        #region Pool

        public void Despawn()
        {
            _asteroidGenerator.Remove(this);
            _asteroidPool.Despawn(this);
        }

        public class Pool : MonoMemoryPool<float, int, Asteroid>
        {
            protected override void Reinitialize(float speed, int health, Asteroid asteroid)
            {
                asteroid._speed = speed;
                asteroid._health = health;
            }

            protected override void OnSpawned(Asteroid asteroid)
            {
                base.OnSpawned(asteroid);
                asteroid.SetStartSettings();
            }
        }

        #endregion

    }
}
