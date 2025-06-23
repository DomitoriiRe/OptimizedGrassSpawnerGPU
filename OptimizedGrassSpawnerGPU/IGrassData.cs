using System.Collections.Generic; 
using UnityEngine;

namespace Project.GrassSystem
{
    public interface IGrassData
    {
        List<Matrix4x4> Matrices { get; }
        void SetMatrices(List<Matrix4x4> newMatrices);
    }
} 