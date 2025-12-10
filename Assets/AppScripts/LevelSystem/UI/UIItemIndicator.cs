using UnityEngine;
using UnityEngine.UI;
using Vortex.Core.AppSystem.Bus;
using Vortex.Core.System.Abstractions;
using Vortex.Core.System.Enums;
using Vortex.Unity.AppSystem.System.TimeSystem;
using Vortex.Unity.UI.Attributes;
using Vortex.Unity.UI.StateSwitcher;
using Vortex.Unity.UI.Tweeners;
using Vortex.Unity.UI.UIComponents;

namespace AppScripts.LevelSystem.UI
{
    public class UIItemIndicator : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour dataStorage;

        [SerializeField] private Image icon;
        [SerializeField] private Image shadow;

        [SerializeField] private UIComponent texts;

        [StateSwitcher(typeof(ItemIndicateMode))] [SerializeField]
        private UIStateSwitcher uiStateSwitcher;

        /// <summary>
        /// Твинеры управления 
        /// </summary>
        [SerializeField] private TweenerBase[] tweeners;

        private IDataStorage _dataStorage;

        private IDataStorage DataStorage => _dataStorage ??= dataStorage as IDataStorage;

        private UIItemSlot _slot;

        private Item Item => _slot?.Item;

        private Item _oldItem;

        private void OnEnable()
        {
#if UNITY_EDITOR
            //Защита от ошибок если при старте в редакторе сцена загружена
            if (App.GetState() != AppStates.Running)
                return;
#endif
            uiStateSwitcher.Reset();

            _slot = DataStorage.GetData<UIItemSlot>();
            _slot.OnSetItem += OnSetNewItem;
            Refresh();
        }

        private void OnDisable()
        {
            _slot.OnSetItem -= OnSetNewItem;
            TimeController.RemoveCall(this);
        }


        private void OnSetNewItem()
        {
            foreach (var tweener in tweeners)
                tweener.Back();
            TimeController.Call(Refresh, 0, this);
        }

        private void Refresh()
        {
            uiStateSwitcher.Set(_slot.Mode);

            if (_oldItem != null)
            {
                texts.SetText(_oldItem.Name, 1);
                shadow.sprite = _oldItem.Icon;
            }

            if (Item == null)
            {
                foreach (var tweener in tweeners)
                    tweener.Back(true);
                return;
            }

            _oldItem = Item;
            texts.SetText(Item.Name);
            icon.sprite = Item.Icon;

            foreach (var tweener in tweeners)
                tweener.Forward();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (DataStorage == null)
                _dataStorage = null;
        }
#endif
    }
}