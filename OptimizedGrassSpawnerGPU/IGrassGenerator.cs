using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Project.GrassSystem
{
    public interface IGrassGenerator
    { 
        UniTask<List<Matrix4x4>> GenerateAsync(Vector2 start, Vector2 size, int density, Vector2 scaleRange, bool randomRotation);
    }
} 