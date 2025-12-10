using Vortex.Core.DatabaseSystem.Model;

namespace AppScripts.LevelSystem
{
    public class Level : Record
    {
        public override string GetDataForSave() => "";

        public override void LoadFromSaveData(string data)
        {
            //Ignore
        }

        public Item[] Items { get; private set; }
        public int Timer { get; private set; }

        public int PoolSize { get; private set; }
        public ItemIndicateMode IndicateMode { get; private set; }
    }
}