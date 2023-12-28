using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace _Game.Utils
{
    public static class Utilities
    {
        // random thu tu 1 list
        public static List<T> RandomList<T>(List<T> list, int amount)
        {
            return list.OrderBy(_ => System.Guid.NewGuid()).Take(amount).ToList();
        }
        
        // lay ket qua theo ty le xac suat
        public static bool Chance(int rand, int max = 100)
        {
            return UnityEngine.Random.Range(0, max) < rand;
        }
        
        // random 1 gia tri enum trong 1 kieu enum
        private static System.Random _random = new System.Random();
        public static T RandomEnumValue<T>()
        {
            var v = System.Enum.GetValues(typeof(T));
            return (T) v.GetValue(_random.Next(v.Length));
        }
        public static Vector3 GetRandomPoint(Vector3 center, float maxDistance)
        {
            // Get Random Point inside Sphere which position is center, radius is maxDistance
            Vector3 randomPos = Random.insideUnitSphere * maxDistance + center;

            NavMeshHit hit; // NavMesh Sampling Info Container

            // from randomPos find a nearest point on NavMesh surface in range of maxDistance
            NavMesh.SamplePosition(randomPos, out hit, maxDistance, NavMesh.AllAreas);

            return (hit.position);
        }
    }
}