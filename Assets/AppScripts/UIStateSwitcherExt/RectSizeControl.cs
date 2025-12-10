using UnityEngine;
using Vortex.Unity.UI.StateSwitcher;

namespace AppScripts.UIStateSwitcherExt
{
    public class RectSizeControl : StateItem
    {
        [SerializeField] private RectTransform rect;

        [SerializeField] private Vector2 size;

        public override void Set()
        {
            rect.sizeDelta = size;
        }

        public override void DefaultState()
        {
        }

#if UNITY_EDITOR

        public override string DropDownItemName => "RectSize Control";
        public override string DropDownGroupName => "Objects";

        public override StateItem Clone()
        {
            var clone = new RectSizeControl()
            {
                rect = rect,
                size = size
            };
            return clone;
        }
#endif
    }
}