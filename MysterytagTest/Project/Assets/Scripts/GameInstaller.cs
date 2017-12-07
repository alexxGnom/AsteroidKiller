using Zenject;

namespace MysterytagTest
{
    public class GameInstaller : MonoInstaller
    {
        [Inject]
        private GameSettings _gameSettings;

        public override void InstallBindings()
        {
            Container.Bind<LevelController>().AsSingle();
            Container.Bind<WeaponController>().AsSingle();

            Container.DeclareSignal<AsteroidDamageSignal>();

            Container.BindFactory<float, Player, Player.PlayerFactory>().FromComponentInNewPrefab(_gameSettings.PlayerPrefab).WithGameObjectName("Player");

            Container.BindMemoryPool<Missile, Missile.Pool>()
                .WithInitialSize(5)
                .FromComponentInNewPrefab(_gameSettings.MissilePrefab)
                .UnderTransformGroup("Missiles");

            Container.BindMemoryPool<Asteroid, Asteroid.Pool>()
                .WithInitialSize(5)
                .FromComponentInNewPrefab(_gameSettings.AsteroidPrefab)
                .UnderTransformGroup("Asteroids");
        }
    }
}
