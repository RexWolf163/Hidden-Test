using AppScripts.LevelSystem;
using Vortex.Core.UIProviderSystem.Model;
using Vortex.Unity.UIProviderSystem.Model;

namespace AppScripts.UIConditions
{
    public class OnLevelWin : UnityUserInterfaceCondition
    {
        protected override void Run()
        {
            LevelController.OnWin += RunCallback;
            RunCallback();
        }

        public override void DeInit()
        {
            LevelController.OnWin -= RunCallback;
        }

        public override ConditionAnswer Check() =>
            LevelController.IsWin() ? ConditionAnswer.Open : ConditionAnswer.Close;
    }
}