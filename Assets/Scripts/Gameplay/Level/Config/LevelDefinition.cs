using Gameplay.Container.Config;
using UnityEngine;

namespace Gameplay.Level.Config
{
    [CreateAssetMenu(menuName = "Level/Definition")]
    public class LevelDefinition : ScriptableObject
    {
        public LevelController levelPrefab;

        [Tooltip("Definitions for each container, in left-to-right order.")]
        public ContainerDefinition[] containers;
    }
}