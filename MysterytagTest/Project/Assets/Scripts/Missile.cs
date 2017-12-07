using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MysterytagTest
{
    public class Missile : MonoBehaviour
    {
        #region Injects

        [Inject]
        private Pool _missilePool;

        [Inject]
        private GameController _gameController;

        #endregion

        #region Private Fields

        private int _damage;

        private float _speed;

        private float _lifeTime;

        private float _startTime;

        private SpriteRenderer _spriteRenderer;

        #endregion

        #region Utils

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

        }

        private void Update()
        {
            transform.position += transform.up * _speed * Time.deltaTime;

            if (Time.realtimeSinceStartup - _startTime > _lifeTime || !_gameController.IsStarted)
            {
                Despawn();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_gameController.IsStarted) return;

            var asteroid = collision.GetComponent<Asteroid>();

            if (asteroid != null)
            {
                Despawn();
                asteroid.SetDamage(_damage);
            }

        }

        #endregion

        #region Pool

        public void Despawn()
        {
            _missilePool.Despawn(this);
        }

        public class Pool : MonoMemoryPool<int, float, float, Color, Missile>
        {
            protected override void Reinitialize(int damage, float speed, float lifeTime, Color color, Missile missile)
            {
                missile._damage = damage;
                missile._speed = speed;
                missile._lifeTime = lifeTime;

                if (missile._spriteRenderer != null)
                    missile._spriteRenderer.color = color;

                missile._startTime = Time.realtimeSinceStartup;
            }
        }

        #endregion
    }
}
