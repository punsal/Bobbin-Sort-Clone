using System;
using Gameplay.Item.Types;
using UnityEngine;

namespace Gameplay.Item.Extensions
{
    public static class SortItemColorExtensions
    {
        public static Color ToColor(this SortItemColor color)
        {
            try
            {
                var result = SortItemColorUtility.GetColor(color);
                return result;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return Color.white;
            }
        }
    }
}