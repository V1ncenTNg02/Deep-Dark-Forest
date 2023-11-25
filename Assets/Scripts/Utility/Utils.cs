using System;
using UnityEngine;

namespace ORZ.Utility
{
    public static class Utils
    {
        /// <summary>
        /// The platform 2D direction (without y axis) from Vector3 to Vector2
        /// </summary>
        /// <param itemName="direction"></param>
        /// <returns></returns>
        public static Vector2 Direction3To2(Vector3 direction)
        {
            direction.y = 0;
            direction.Normalize();
            return new Vector2(direction.x, direction.z);
        }

        /// <summary>
        /// The platform 2D direction (without y axis) from one object to another
        /// </summary>
        /// <param itemName="from"></param>
        /// <param itemName="to"></param>
        /// <returns></returns>
        public static Vector2 Direction3To2(Vector3 from, Vector3 to)
        {
            Vector3 direction3D = (to - from).normalized;
            return Direction3To2(direction3D);
        }

        /// <summary>
        /// The angle between front direction and given direction
        /// </summary>
        /// <param itemName="direction"></param>
        /// <returns></returns>
        public static float AngleOfDirection(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            return 90 - angle;
        }

        public static Comparison<GameObject> CompareByName 
            = (obj1, obj2) => String.Compare(obj1.name, obj2.name, StringComparison.Ordinal);
    }

}
