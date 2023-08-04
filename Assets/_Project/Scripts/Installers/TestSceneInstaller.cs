using UnityEngine;
using Zenject;

public class TestSceneInstaller : MonoInstaller
{
    [SerializeField] private ResetSceneButton resetSceneButton;
    
    public override void InstallBindings()
    {
        Container.Bind<ResetSceneButton>().FromInstance(resetSceneButton).AsSingle();
    }
}