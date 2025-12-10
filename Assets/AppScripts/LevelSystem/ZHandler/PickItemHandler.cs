namespace AppScripts.LevelSystem.ZHandler
{
    /// <summary>
    /// Контроллер сбора предметов
    /// </summary>
    public class PickItemHandler : IPickItem
    {
        public void Pick(Item item) => LevelController.PickItem(item);
    }
}