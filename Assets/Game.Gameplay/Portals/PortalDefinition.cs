using UnityEngine;

namespace Game.Gameplay.Portals {
    [CreateAssetMenu(menuName = "PacManEcs/" + nameof(PortalDefinition))]
    public sealed class PortalDefinition : ScriptableObject {
        public float portalReloadTime = 1f;
    }
}