using System;
using UnityEngine;

namespace PlanetStructureTypes
{
    public struct VertexData
    {
        public Vector3 position;
        public Vector3 normal;
        public Vector4 tangent;
        public Vector4 color;
        public Vector2 UV0;

        public bool Equals (VertexData other)
        {
            bool equalPosition = Mathf.Approximately(Vector3.Distance(position, other.position), 0);
            bool equalNormal = Mathf.Approximately(Vector3.Distance(normal, other.normal), 0);
            bool equalTangent = Mathf.Approximately(Vector4.Distance(tangent, other.tangent), 0);
            bool equalColor = (color == other.color);
            bool equalUV0 = Mathf.Approximately(Vector2.Distance(UV0, other.UV0), 0);

            return equalPosition
                && equalNormal
                // && equalTangent
                && equalColor
                // && equalUV0
                ;
        }
    }
}