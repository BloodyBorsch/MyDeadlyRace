using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace MaksK_Race
{
    public sealed class BotController : BaseController, IExecute, IInitialization
    {
        private int _countBot;
        private int _index = 0;

        [SerializeField] private int _teamsCount; //не менее 2, не более 4

        [SerializeField] private float _startSpawnTimer;
        [SerializeField] private float _timebtwSpawn = 0.0f;

        [SerializeField] private bool _spawnRepeat;

        private readonly HashSet<MainBot> _botList = new HashSet<MainBot>();
        
        private BaseCar _car;
        private Transform _gameControllerPos;
        private PoolObjects _objectPooler;

        public MainBot FriendlyBot = null;

        public BotController(BaseCar car, Transform gameControllerPos, PoolObjects pool = null)
        {
            _car = car;
            _gameControllerPos = gameControllerPos;

            if (pool != null) _objectPooler = pool;
        }

        public void Initialization()
        {           
            _countBot = ServiceLocatorMonoBehaviour.GetService<GameController>().AmountOfBots;
            _teamsCount = ServiceLocatorMonoBehaviour.GetService<GameController>().TeamsCount;
            _spawnRepeat = ServiceLocatorMonoBehaviour.GetService<GameController>().SpawnRepeat;
            _startSpawnTimer = ServiceLocatorMonoBehaviour.GetService<GameController>().SpawnTimer; 

            if (_spawnRepeat) _timebtwSpawn = _startSpawnTimer;

            if (_countBot > 0 && _objectPooler != null)
            {
                _objectPooler.FirstStarting();

                if (!_objectPooler.PoolDictionary.ContainsKey(UnitSwitcher.FlyingBot)) return;

                for (_index = 0; _index < _countBot;)
                {
                    if ((_index % 2) == 0)
                    {
                        DropBot(_gameControllerPos, BotTeams.TeamOne);
                    }
                    else
                    {
                        DropBot(_gameControllerPos, BotTeams.TeamTwo);
                    }
                }
            }            
        }

        public MainBot DropBot(Transform obj, BotTeams type, bool closeDistance = false)
        {            
            //MainBot newBot = _objectPooler.SpawnBotFromPool(UnitSwitcher.FlyingBot, Patrol.GenericPoint(obj, closeDistance), Quaternion.identity);
            MainBot newBot = (MainBot)_objectPooler.SpawnFromPool(UnitSwitcher.FlyingBot, Patrol.GenericPoint(obj, closeDistance), Quaternion.identity);
            newBot.TypeOfBot = type;
            newBot.Agent.avoidancePriority = _index;            
            AddBotToList(newBot);
            _index++;
            return newBot;
        }

        private void AddBotToList(MainBot bot)
        {
            if (!_botList.Contains(bot))
            {
                _botList.Add(bot);
                bot.OnDieChange += RemoveBotFromList;
            }
        }

        private void RemoveBotFromList(MainBot bot)
        {
            if (!_botList.Contains(bot))
            {
                return;
            }
            
            bot.OnDieChange -= RemoveBotFromList;
            _botList.Remove(bot);
        }

        public void Execute()
        {
            if (!IsActive)
            {
                return;
            }

            for (var i = 0; i < _botList.Count; i++)
            {
                var bot = _botList.ElementAt(i);
                bot.Tick();
            }

            if (_spawnRepeat)
            {
                if (_timebtwSpawn <= 0)
                {
                    int random = Random.Range(0, _teamsCount + 1);

                    switch (random)
                    {
                        case 1:
                            DropBot(_gameControllerPos, BotTeams.TeamOne);
                            break;
                        case 2:
                            DropBot(_gameControllerPos, BotTeams.TeamTwo);
                            break;
                        case 3:
                            DropBot(_gameControllerPos, BotTeams.TeamThree);
                            break;
                        case 4:
                            DropBot(_gameControllerPos, BotTeams.TeamFour);
                            break;
                        default:
                            Debug.LogWarning("Неправильный подбор команды бота");
                            break;
                    }

                    _timebtwSpawn = _startSpawnTimer;
                }
                else
                {
                    _timebtwSpawn -= Time.deltaTime;
                }
            }
        }

        public void GetFriendlyBot(MainBot bot)
        {
            FriendlyBot = bot;
        }
    }
}
