using UnityEngine;
using Zenject;

namespace Project.GrassSystem
{
    public class GrassInstaller : MonoInstaller
    {
        [SerializeField] private Terrain _terrain;
        [SerializeField] private Camera _renderingCamera;

        [SerializeField] private Mesh _meshGrass;
        [SerializeField] private Material _materialGrass;


        public override void InstallBindings()
        {
            BindTerrain();
            BindRenderingCamera();
            BindGrassData();
            BindGrassRenderer();
            BindGrassGenerator();
        }

        private void BindTerrain() => Container.Bind<Terrain>().FromInstance(_terrain).AsSingle();
        private void BindRenderingCamera() => Container.Bind<Camera>().FromInstance(_renderingCamera).AsSingle();
        private void BindGrassData() => Container.Bind<IGrassData>().To<GrassData>().AsSingle();
        private void BindGrassRenderer() => Container.Bind<IGrassRenderer>().To<GrassRenderer>().AsSingle().WithArguments(_meshGrass, _materialGrass);
        private void BindGrassGenerator() => Container.Bind<IGrassGenerator>().To<GrassGenerator>().AsSingle().WithArguments(_terrain);
    }
}