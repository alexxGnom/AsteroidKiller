using System;
using UnityEngine;

namespace MysterytagTest
{
    [Serializable]
    public struct LevelData
    {
        public int WinScorePerLevel;

        public int AsteroidCount;

        public float AsteroidSpawnDelay;

        public float MaxAsteroidSpeed;
        
        public float MinAsteroidSpeed;

        public int AsteroidHealth;
    }

    [Serializable]
    public struct WeaponData
    {
        public int Damage;

        public float Speed;

        public float ChargeTime;

        public int ScoreToUpdate;

        public Color Color;
    }

    [CreateAssetMenu(fileName = "GameSettings", menuName = "Create GameSettings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Level settings:")]

        public LevelData[] LevelsData;

        [Header("Weapon settings:")]
        public WeaponData[] WeaponsData;

        [Header("Player settings:")]
        public GameObject PlayerPrefab;
        public float PlayerSpeed;

        [Header("Missile Settings:")]
        public GameObject MissilePrefab;
        public float MissileLifeTime;

        [Header("Asteroid settings:")]
        public GameObject AsteroidPrefab;

        [Header("Screen border settings:")]
        public float EdgeReflectForce;
        public int EdgeDelta;

    }
}
