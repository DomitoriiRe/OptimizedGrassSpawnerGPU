using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Project.GrassSystem
{
    public class OptimizedGrassSpawnerGPU : MonoBehaviour // ”правл€ет генерацией и отрисовкой травы в рантайме.
    {
        [SerializeField] Vector2 _scaleRange = new Vector2(0.8f, 1.2f);
        [SerializeField] bool _randomRotation = true;
        [SerializeField] bool _useFrustumCulling = true; 
        [Header("Area Settings")]
        [SerializeField] Vector2 _areaStart = Vector2.zero;
        [SerializeField] Vector2 _areaSize = new Vector2(50, 50);
        [SerializeField, Range(1, 1500)] int _grassDensity = 100;

        private Camera _renderingCamera;
        private Terrain _terrain;
        private IGrassData _grassData;
        private IGrassGenerator _generator;
        private IGrassRenderer _renderer;

        private bool _needsUpdate = true;

        [Inject] public void Construct(Camera renderingCamera, Terrain terrain, IGrassData grassData, IGrassGenerator generator, IGrassRenderer renderer)
        {
            _renderingCamera = renderingCamera;
            _terrain = terrain;
            _grassData = grassData;
            _generator = generator;
            _renderer = renderer;
        }

        private async void OnEnable() => await UpdateGrassAsync();

        private void OnValidate() => _needsUpdate = true;

        private async void Update()
        {
            if (_needsUpdate)
            {
                await UpdateGrassAsync();
                _needsUpdate = false;
            }

            _renderer.Render(_grassData.Matrices, _renderingCamera ?? Camera.main, _useFrustumCulling, _scaleRange);
        }

        private async UniTask UpdateGrassAsync()
        { 
                var matrices = await _generator.GenerateAsync(
                    _areaStart,
                    _areaSize,
                    _grassDensity,
                    _scaleRange,
                    _randomRotation);

                _grassData.SetMatrices(matrices); 
        }

        private void OnDrawGizmosSelected()
        {
            if (_terrain == null) return;

            Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
            Vector3 corner = new Vector3(_areaStart.x, 0, _areaStart.y);
            Vector3 size = new Vector3(_areaSize.x, 0.1f, _areaSize.y);
            float y = _terrain.SampleHeight(corner) + _terrain.GetPosition().y;
            corner.y = y;
            Gizmos.DrawCube(corner + size * 0.5f, size);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(corner + size * 0.5f, size);
        }
    }
}