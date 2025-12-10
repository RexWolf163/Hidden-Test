using AppScripts.LevelSystem.UI;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Vortex.Unity.DatabaseSystem.Presets;
using Vortex.Unity.Extensions.Attributes;

namespace AppScripts.LevelSystem
{
    /// <summary>
    /// пресет комнаты-уровня
    /// </summary>
    [CreateAssetMenu(fileName = "Level", menuName = "Database/New Level")]
    public class LevelPreset : RecordPreset<Level>
    {
        [SerializeField, PropertyRange("GetMin", "GetMax")]
        private int poolSize = 3;

        [SerializeField] private ItemIndicateMode indicateMode;

        /// <summary>
        /// Список предметов принадлежащих комнате
        /// </summary>
        [SerializeReference] private Item[] items = new Item[0];

        /// <summary>
        /// Таймер на выполнение (сек)
        /// </summary>
        [SerializeField, HorizontalGroup("h1")]
        private int timer = 2 * 60;

        [HorizontalGroup("h1"), TimerDraw, HideLabel, ShowInInspector]
        private long timerVal => timer;

        public Item[] Items
        {
            get
            {
                items.ForEach(x => x.Picked = false);
                return items;
            }
        }

        public int Timer => timer;

        public int PoolSize => poolSize;
        public ItemIndicateMode IndicateMode => indicateMode;
#if UNITY_EDITOR

        /// <summary>
        /// Линк на префаб камеры на настроенной сцене.
        /// Используется для быстрого заполнения ScriptableObject в БД данными Chidren-предметов для этой камеры 
        /// </summary>
        [ShowInInspector, BoxGroup("FastLoad")]
        private Camera prefab;

        [Button(ButtonSizes.Large), BoxGroup("FastLoad"), HideIf("@prefab == null")]
        private void LoadDataFromScene()
        {
            if (prefab == null)
                return;
            var uiItems = prefab.transform.GetComponentsInChildren<UIItem>();
            var c = uiItems.Length;
            items = new Item[c];
            for (var i = 0; i < c; i++)
            {
                var uiItem = uiItems[i];
                var itemName = uiItem.name.ToUpper();
                items[i] = new Item(itemName, null);
            }
        }

        private int GetMin() => items.Length > 0 ? 1 : 0;
        private int GetMax() => items.Length - 1;
#endif
    }
}