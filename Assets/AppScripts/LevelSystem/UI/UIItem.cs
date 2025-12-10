using AppScripts.LevelSystem.ZHandler;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vortex.Core.AppSystem.Bus;
using Vortex.Core.System.Enums;
using Vortex.Unity.LocalizationSystem;
using Vortex.Unity.UI.Tweeners;
using Zenject;

namespace AppScripts.LevelSystem.UI
{
    /// <summary>
    /// Представление предмета на карте уровня
    /// </summary>
    public class UIItem : MonoBehaviour, IPointerClickHandler
    {
        /// <summary>
        /// ключ-названиe предмета (берется из списка локали)
        /// </summary>
        [SerializeField, LocalizationKey, OnValueChanged("OnChangeName")]
        private string itemKey;

        /// <summary>
        /// Картинка предмета
        /// </summary>
        [SerializeField] private Image sprite;

        /// <summary>
        /// Твинеры схлопывания
        /// </summary>
        [SerializeField] private TweenerBase[] tweeners;

        /// <summary>
        /// Контроллер схлопывания предмета
        /// </summary>
        [Inject] private IPickItem pickItemHandler;

        /// <summary>
        /// Кэш названия уровня (название сцены)
        /// </summary>
        private string _levelName;

        /// <summary>
        /// Кэш указателя на данные предмета
        /// </summary>
        private Item _item;

        private void OnEnable()
        {
#if UNITY_EDITOR
            //Блокировка ошибок если запуск из редактора с загруженной сценой
            if (App.GetState() != AppStates.Running)
                return;
#endif
            foreach (var tweener in tweeners)
                tweener.Back(true);

            _levelName = gameObject.scene.name;
            _item = LevelController.GetItemData(_levelName, itemKey);
            Refresh(true);
        }

        private void OnDisable()
        {
        }

        /// <summary>
        /// обработка клика
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            pickItemHandler.Pick(_item);
            Refresh();
        }

        /// <summary>
        /// Обновление представления
        /// </summary>
        private void Refresh(bool firstRun = false)
        {
            if (_item == null || !_item.Active || firstRun && _item.Picked)
            {
                foreach (var tweener in tweeners)
                    tweener.Forward(true);
                return;
            }

            if (_item.Picked)
                foreach (var tweener in tweeners)
                    tweener.Forward();
            else
                foreach (var tweener in tweeners)
                    tweener.Back(true);
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (itemKey.IsNullOrWhitespace())
                itemKey = name;
            if (sprite == null)
                sprite = GetComponentInChildren<Image>();
        }

        private void OnChangeName() => transform.name = itemKey;

#endif
    }
}