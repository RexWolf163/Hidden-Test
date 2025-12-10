using AppScripts.LevelSystem.ZHandler;
using Zenject;

public class FailHandlerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IMakeFail>().To<MakeFailHandler>().AsSingle();
    }
}