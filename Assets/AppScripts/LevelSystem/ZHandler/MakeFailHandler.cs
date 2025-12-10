namespace AppScripts.LevelSystem.ZHandler
{
    public class MakeFailHandler : IMakeFail
    {
        public void Fail()
        {
            LevelController.Fail();
        }
    }
}