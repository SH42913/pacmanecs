using UnityEngine;

namespace Portals
{
    [CreateAssetMenu(menuName = "PacManEcs/PortalDefinition", fileName = "PortalDef")]
    public class PortalDefinition : ScriptableObject
    {
        public float portalReloadTime = 1f;
    }
}