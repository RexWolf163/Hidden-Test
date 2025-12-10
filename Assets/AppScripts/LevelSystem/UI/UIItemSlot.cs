using System;

namespace AppScripts.LevelSystem.UI
{
    public class UIItemSlot
    {
        public event Action OnSetItem;

        public Item Item { get; private set; }

        public ItemIndicateMode Mode { get; private set; } = ItemIndicateMode.Sprite;

        public void SetItem(Item item, ItemIndicateMode mode)
        {
            Item = item;
            Mode = mode;
            OnSetItem?.Invoke();
        }

        public void SetItem(Item item)
        {
            Item = item;
            OnSetItem?.Invoke();
        }
    }
}