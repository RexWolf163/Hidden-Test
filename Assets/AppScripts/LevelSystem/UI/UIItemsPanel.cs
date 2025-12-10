using System.Linq;
using UnityEngine;
using Vortex.Unity.UI.PoolSystem;

namespace AppScripts.LevelSystem.UI
{
    public class UIItemsPanel : MonoBehaviour
    {
        [SerializeField] private Pool itemsPool;

        private UIItemSlot[] slots;

        private void OnEnable()
        {
            LevelController.OnLevelStarted += Refresh;
            LevelController.OnItemPicked += RecalcItems;
            Refresh();
        }

        private void OnDisable()
        {
            LevelController.OnLevelStarted -= Refresh;
            LevelController.OnItemPicked -= RecalcItems;
        }

        private void Refresh()
        {
            var level = LevelController.GetCurrentLevel();
            itemsPool.Clear();
            if (level == null)
                return;

            var items = LevelController.GetSearchableItems();
            var c = items.Length;
            slots = new UIItemSlot[c];
            for (var i = 0; i < c; i++)
            {
                slots[i] = new UIItemSlot();
                slots[i].SetItem(items[i], level.IndicateMode);
                itemsPool.AddItem(slots[i]);
            }
        }

        private void RecalcItems()
        {
            var slot = slots.First(x => x.Item?.Picked ?? false);
            var items = LevelController.GetSearchableItems();
            var newItem = items.FirstOrDefault(x => slots.All(y => y.Item != x));
            slot.SetItem(newItem);
        }
    }
}