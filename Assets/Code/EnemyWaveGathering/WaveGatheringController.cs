using CombatSystem;
using Interfaces;
using System.Collections.Generic;
using UnityEngine;
using Wave.Interfaces;


namespace Wave
{
    public class WaveGatheringController : IWaveGathering
    {
        private const int STARTING_CAPACITY = 5;
        private const int BOSS_STARTING_CAPACITY = 2;

        private readonly IWaveCalculator _calculator;
        private readonly Dictionary<EnemyType, IPoolController<IEnemyController>> _pools = 
            new Dictionary<EnemyType, IPoolController<IEnemyController>>();
        private readonly List<EnemySettings> _enemyTypesToBuy = new List<EnemySettings>();

        private readonly IEnemySorter _enemySorter;


        public WaveGatheringController(GeneratorLevelController generatorLevelController, IEnemyAIController enemyAIController, 
            IBulletsController bulletsController)
        {
            _calculator = new WaveCalculator();
            var enemySet = Resources.Load<EnemySet>(nameof(EnemySet));
            EquipEnemyPool(enemySet, generatorLevelController, enemyAIController, bulletsController);

            _enemySorter = new EnemySorterTanksPriority();
        }

        private void EquipEnemyPool(EnemySet enemySet, GeneratorLevelController generatorLevelController, IEnemyAIController enemyAIController,
            IBulletsController bulletsController)
        {
            var overallPoolHolder = new GameObject("EnemyPools").transform;
            foreach (var enemy in enemySet.Enemys)
            {
                var capacity = enemy.Type == EnemyType.Boss ? BOSS_STARTING_CAPACITY : STARTING_CAPACITY;
                _pools[enemy.Type] = new EnemyControllerPool(capacity, enemy, overallPoolHolder, generatorLevelController, 
                    enemyAIController, bulletsController);
                if (enemy.Type != EnemyType.Boss) _enemyTypesToBuy.Add(enemy);
            }
        }

        public List<IPoolController<IEnemyController>> GetEnemysList(int waveNumber, bool isDowntime)
        {
            var waveCost = _calculator.GetWaveCost(waveNumber, isDowntime);
            List<IPoolController<IEnemyController>> enemies = new List<IPoolController<IEnemyController>>();

            while (TryBuyRandomEnemy(ref waveCost, out EnemySettings enemy))
            {
                enemies.Add(_pools[enemy.Type]);
            }
            enemies.Add(_pools[EnemyType.Boss]);

            return _enemySorter.SortEnemyList(enemies);
        }

        private bool TryBuyRandomEnemy(ref float totalScore, out EnemySettings enemy)
        {
            var typeEnemyToBuy = _enemyTypesToBuy[UnityEngine.Random.Range(0, _enemyTypesToBuy.Count)];
            Debug.Log($"���� ������ {typeEnemyToBuy.Type} ���� {totalScore}");
            if(typeEnemyToBuy.Stats.Cost < totalScore)
            {
                totalScore -= typeEnemyToBuy.Stats.Cost;
                enemy = typeEnemyToBuy;
                Debug.Log($"����� {typeEnemyToBuy.Type}. ����� �������� {totalScore}");
                return true;
            }
            Debug.Log($"�� ���� ������ {typeEnemyToBuy.Type}");
            enemy = null;
            return false;
        }

        public void Dispose()
        {
            foreach (var enemyPool in _pools.Values) enemyPool.Dispose();
            _pools.Clear();
        }
    }
}