using AppScripts.LevelSystem.ZHandler;
using Zenject;

namespace AppScripts.Zenject
{
    public class LevelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IPickItem>().To<PickItemHandler>().AsTransient();
        }
    }
}