using System;
using Gameplay.Item.Types;
using UnityEngine;

namespace Gameplay.Container.Config
{
    [Serializable]
    public struct ContainerDefinition
    {
        [Tooltip("Bottom-to-top colors (visually top-to-bottom). Use fewer than capacity for partially filled containers.")]
        public SortItemColor[] colorsBottomToTop;
    }
}