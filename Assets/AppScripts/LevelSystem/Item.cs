using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Unity.Extensions.Attributes;

namespace AppScripts.LevelSystem
{
    [Serializable]
    [FoldoutClass("@Name"), HideReferenceObjectPicker]
    public class Item
    {
        [SerializeField] private string name;

        /// <summary>
        /// Иконка предмета для панели поиска и прочих UI
        /// (Спрайт для карты не устанавливается. Что на карте назначили - то и остается)
        /// </summary>
        [SerializeField] private Sprite icon;

        /// <summary>
        /// Флаг включения/отключения предмета в комнате (глобальная настройка)
        /// </summary>
        [SerializeField] private bool active = true;

        public Item(string name, Sprite icon)
        {
            this.name = name;
            this.icon = icon;
        }

        public string Name => name;

        public Sprite Icon => icon;

        public bool Active => active;

        [ShowInInspector, HideInEditorMode] public bool Picked { get; internal set; }
    }
}