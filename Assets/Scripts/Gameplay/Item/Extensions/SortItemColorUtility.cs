using System;
using Gameplay.Item.Types;
using UnityEngine;

namespace Gameplay.Item.Extensions
{
    public static class SortItemColorUtility
    {
        public static Color GetColor(SortItemColor color)
        {
            return color switch
            {
                SortItemColor.Red => Color.red,
                SortItemColor.Orange => new Color(1, 0.5f, 0),
                SortItemColor.Yellow => Color.yellow,
                SortItemColor.Green => Color.green,
                SortItemColor.Blue => Color.blue,
                SortItemColor.Purple => new Color(01f, 0, 1f),
                _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
            };
        }
    }
}