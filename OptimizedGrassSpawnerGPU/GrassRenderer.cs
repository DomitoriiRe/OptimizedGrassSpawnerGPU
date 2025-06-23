using System.Collections.Generic;
using UnityEngine;

namespace Project.GrassSystem
{
    public class GrassRenderer : IGrassRenderer // Отвечает за отрисовку травы с поддержкой фрустрационного отсечения.
    {
        private readonly Mesh _mesh;
        private readonly Material _material;
        private readonly MaterialPropertyBlock _block = new MaterialPropertyBlock();
        private const int _maxBatchSize = 1023; 

        public GrassRenderer(Mesh mesh, Material material)
        {
            _mesh = mesh;
            _material = material;
        }

        public void Render(List<Matrix4x4> matrices, Camera camera, bool useFrustumCulling, Vector2 scaleRange)
        {
            if (matrices == null || matrices.Count == 0 || _mesh == null || _material == null) return;

            List<Matrix4x4> toRender = useFrustumCulling ? PerformFrustumCulling(matrices, camera, scaleRange) : matrices;

            const int batchSize = _maxBatchSize;
            for (int i = 0; i < toRender.Count; i += batchSize)
            {
                int count = Mathf.Min(batchSize, toRender.Count - i);
                Graphics.DrawMeshInstanced(_mesh, 0, _material, toRender.GetRange(i, count), _block);
            }
        }

        private List<Matrix4x4> PerformFrustumCulling(List<Matrix4x4> matrices, Camera camera, Vector2 scaleRange)
        {
            var result = new List<Matrix4x4>();
            if (camera == null) return result;

            var planes = GeometryUtility.CalculateFrustumPlanes(camera);
            foreach (var m in matrices)
            {
                Vector3 position = m.GetColumn(3);
                var bounds = new Bounds(position, Vector3.one * scaleRange.y);
                if (GeometryUtility.TestPlanesAABB(planes, bounds))
                    result.Add(m);
            }
            return result;
        }
    }
} 