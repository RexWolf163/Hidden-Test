using AppScripts.LevelSystem;
using Vortex.Core.UIProviderSystem.Model;
using Vortex.Unity.UIProviderSystem.Model;

namespace AppScripts.UIConditions
{
    public class OnLevelStarted : UnityUserInterfaceCondition
    {
        protected override void Run()
        {
            LevelController.OnLevelStarted += RunCallback;
            RunCallback();
        }

        public override void DeInit()
        {
            LevelController.OnLevelStarted -= RunCallback;
        }

        public override ConditionAnswer Check() =>
            LevelController.GetCurrentLevel() != null ? ConditionAnswer.Open : ConditionAnswer.Close;
    }
}