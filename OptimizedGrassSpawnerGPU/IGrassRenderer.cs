using System.Collections.Generic;
using UnityEngine;

namespace Project.GrassSystem
{
    public interface IGrassRenderer
    {
        void Render(List<Matrix4x4> matrices, Camera camera, bool useFrustumCulling, Vector2 scaleRange);
    }
} 