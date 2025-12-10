using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vortex.Core.AppSystem.Bus;
using Vortex.Core.DatabaseSystem.Bus;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.System.Abstractions;

namespace AppScripts.LevelSystem
{
    public class LevelController : Singleton<LevelController>
    {
        /// <summary>
        /// Событие открытия уровня
        /// </summary>
        public static event Action OnLevelStarted;

        /// <summary>
        /// Событие выигрыша (все предметы найдены)
        /// </summary>
        public static event Action OnWin;

        /// <summary>
        /// Событие проигрыша (таймер закончился)
        /// </summary>
        public static event Action OnFail;

        /// <summary>
        /// Предмет отмечен
        /// </summary>
        public static event Action OnItemPicked;

        /// <summary>
        /// Список всех уровней в БД
        /// </summary>
        private static List<Level> levels;

        /// <summary>
        /// Сортируемый индекс для быстрого поиска по ключу
        /// </summary>
        private static SortedDictionary<string, int> index = new();

        /// <summary>
        /// Номер текущего уровня в списке
        /// </summary>
        private static int currentLevel;

        /// <summary>
        /// Признак проигрыша
        /// </summary>
        private static bool fail;

        public static Item[] GetItems(string levelName)
        {
            if (!index.ContainsKey(levelName))
            {
                Debug.LogError($"Level «{levelName}» is not found");
                return null;
            }

            var level = index[levelName];

            if (levels.Count <= level)
            {
                Debug.LogError($"Error. Levels Array are broken.");
                return null;
            }

            return levels[level].Items.ToArray();
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Run()
        {
            App.OnStart += Init;
        }

        private static void Init()
        {
            App.OnStart -= Init;

            levels = Database.GetRecords<Level>();

            //Формируем индекс
            var c = levels.Count;
            for (var i = 0; i < c; i++)
            {
                var levelData = levels[i];
                index.AddNew(levelData.Name, i);
            }

            currentLevel = -1;

            //Загружаем уровень (по умолчанию первый в списке)
            _ = LoadLevel();
        }

        /// <summary>
        /// Загрузка уровня
        /// </summary>
        /// <param name="levelIndex">Номер в списке</param>
        private static async Task LoadLevel(int levelIndex = 0)
        {
            fail = false;
            await Task.Yield();
            await UnloadLevel();
            await Task.Yield();
            currentLevel = levelIndex;
            var level = GetCurrentLevel();
            if (level == null)
            {
                Debug.LogError($"No level data found (#{currentLevel})");
                return;
            }

            await SceneManager.LoadSceneAsync(level.Name, LoadSceneMode.Additive);
            OnLevelStarted?.Invoke();
        }

        /// <summary>
        /// Выгрузка текущего уровня
        /// </summary>
        private static async Task UnloadLevel()
        {
            var level = GetCurrentLevel();
            if (level == null)
                return;
            await SceneManager.UnloadSceneAsync(level.Name);
        }

        public static Level GetCurrentLevel()
        {
            if (currentLevel == -1)
                return null;
            if (levels == null || levels.Count == 0)
                return null;
            return levels[currentLevel];
        }

        public static Item GetItemData(string level, string itemKey)
        {
            var items = GetItems(level);
            return items.First(x => x.Name == itemKey);
        }

        /// <summary>
        /// Отметить найденный предмет
        /// </summary>
        /// <param name="item"></param>
        public static void PickItem(Item item)
        {
            if (!GetSearchableItems().Contains(item))
                return;
            item.Picked = true;
            OnItemPicked?.Invoke();

            if (IsWin())
                OnWin?.Invoke();
        }

        public static void Fail()
        {
            fail = true;
            OnFail?.Invoke();
        }

        public static Item[] GetSearchableItems()
        {
            var level = GetCurrentLevel();
            return level.Items.Where(x => x.Active && !x.Picked).Take(level.PoolSize).ToArray();
        }

        public static bool IsWin()
        {
            var level = GetCurrentLevel();
            if (level == null)
                return false;
            return level.Items.All(x => x.Picked || !x.Active);
        }

        public static bool IsFail() => fail;
    }
}