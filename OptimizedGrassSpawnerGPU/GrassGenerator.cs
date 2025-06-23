using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Project.GrassSystem
{
    public class GrassGenerator : IGrassGenerator // Генератор позиций, масштабов и вращений травинок по площади.
    {
        private readonly Terrain _terrain;
        private const float _densityScalingFactor = 0.1f; 
         
        public GrassGenerator(Terrain terrain) => _terrain = terrain;

        public async UniTask<List<Matrix4x4>> GenerateAsync(Vector2 start, Vector2 size, int density, Vector2 scaleRange, bool randomRotation)
        {
            List<Matrix4x4> result = new List<Matrix4x4>();
            if (_terrain == null || density <= 0) return result;

            int actualDensity = Mathf.FloorToInt(density * _densityScalingFactor);
            int totalItems = actualDensity * actualDensity;
            int itemsPerFrame = Mathf.Max(totalItems / 10, 100); // Обрабатываем часть данных каждый кадр

            for (int x = 0; x < actualDensity; x++)
            {
                for (int z = 0; z < actualDensity; z++)
                {
                    float posX = start.x + (x / (float)actualDensity) * size.x;
                    float posZ = start.y + (z / (float)actualDensity) * size.y;
                    float y = _terrain.SampleHeight(new Vector3(posX, 0, posZ)) + _terrain.GetPosition().y;

                    Vector3 position = new Vector3(posX, y, posZ);
                    Quaternion rotation = randomRotation ? Quaternion.Euler(0, Random.Range(0f, 360f), 0) : Quaternion.identity;
                    float scale = Random.Range(scaleRange.x, scaleRange.y);
                    Vector3 scaleVec = new Vector3(scale, scale, scale);

                    result.Add(Matrix4x4.TRS(position, rotation, scaleVec));

                    // Периодически ждем следующий кадр для распределения нагрузки
                    if ((x * actualDensity + z) % itemsPerFrame == 0)
                    {
                        await UniTask.Yield(PlayerLoopTiming.Update);
                    }
                }
            }

            return result;
        }
    }
}