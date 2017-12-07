using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MysterytagTest
{
    public class AsteroidGenerator : MonoBehaviour
    {

        #region Injects

        [Inject]
        private LevelController _levelController;

        [Inject]
        private Asteroid.Pool _asteroidPool;

        [Inject]
        private GameSettings _gameSettings;

        #endregion

        #region Private fields

        private List<Asteroid> _asteroids;

        private Coroutine _generatorRoutine;

        #endregion

        #region Interface

        public void StartGenerate()
        {
            _generatorRoutine = StartCoroutine(GeneratorRoutine());
        }

        public void StopGenerate()
        {
            if (_generatorRoutine != null)
                StopCoroutine(_generatorRoutine);
        }

        public void Remove(Asteroid asteroid)
        {
            _asteroids.Remove(asteroid);
        }

        public void DespawnAll()
        {
            while (_asteroids.Count > 0)
            {
                _asteroids[_asteroids.Count - 1].Despawn();
            }
        }

        #endregion

        #region Utils

        private void Awake()
        {
            _asteroids = new List<Asteroid>();
        }

        private IEnumerator GeneratorRoutine()
        {
            var levelData = _levelController.LevelData;

            while (true)
            {
                yield return new WaitForSeconds(levelData.AsteroidSpawnDelay);

                if (_asteroids.Count < levelData.AsteroidCount)
                {
                    var asteroid = _asteroidPool.Spawn( Random.Range(levelData.MinAsteroidSpeed, levelData.MaxAsteroidSpeed), levelData.AsteroidHealth);
                    asteroid.transform.position = GetRandomEdgePoint();
                    _asteroids.Add(asteroid);
                }
            }
        }

        private Vector3 GetRandomEdgePoint()
        {
            var sideNum = Random.Range(1, 5);

            switch (sideNum)
            {
                case 1:
                    return GetRandomPointRight();
                case 2:
                    return GetRandomPointLeft();
                case 3:
                    return GetRandomPointTop();
                case 4:
                    return GetRandomPointBottom();
                default:
                    return GetRandomPointTop();
            }
        }

        private Vector3 GetRandomPointTop()
        {
            var pos = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0f, Screen.width), Screen.height, 0f));
            pos.z = 0f;
            return pos;
        }

        private Vector3 GetRandomPointBottom()
        {
            var pos = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0f, Screen.width), 0f, 0f));
            pos.z = 0f;
            return pos;
        }

        private Vector3 GetRandomPointLeft()
        {
            var pos = Camera.main.ScreenToWorldPoint(new Vector3(0f, Random.Range(0f, Screen.height), 0f));
            pos.z = 0f;
            return pos;
        }

        private Vector3 GetRandomPointRight()
        {
            var pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Random.Range(0f, Screen.height), 0f));
            pos.z = 0f;
            return pos;
        }

        #endregion

    }
}

