using System.Collections.Generic;
using UnityEngine;

namespace Project.GrassSystem
{
    public class GrassData : IGrassData // Хранит и предоставляет доступ к списку матриц трансформаций травы.
    {
        private List<Matrix4x4> _matrices = new List<Matrix4x4>();

        public List<Matrix4x4> Matrices => _matrices;

        public void SetMatrices(List<Matrix4x4> newMatrices) => _matrices = newMatrices ?? new List<Matrix4x4>();
    }
} 