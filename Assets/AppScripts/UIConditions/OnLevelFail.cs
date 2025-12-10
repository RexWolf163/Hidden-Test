using AppScripts.LevelSystem;
using Vortex.Core.UIProviderSystem.Model;
using Vortex.Unity.UIProviderSystem.Model;

namespace AppScripts.UIConditions
{
    public class OnLevelFail : UnityUserInterfaceCondition
    {
        protected override void Run()
        {
            LevelController.OnFail += RunCallback;
            RunCallback();
        }

        public override void DeInit()
        {
            LevelController.OnFail -= RunCallback;
        }

        public override ConditionAnswer Check() =>
            LevelController.IsFail() ? ConditionAnswer.Open : ConditionAnswer.Close;
    }
}