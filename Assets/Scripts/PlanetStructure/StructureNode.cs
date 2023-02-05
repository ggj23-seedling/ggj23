using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace PlanetStructureTypes
{
    public class StructureNode
    {
        // planet origin in world space
        Transform planetTransform;

        // position in local space coordinates
        public Vector3 localPosition { get; }

        public Quaternion localRotation { get; }

        public Vector3 normalDirection { get { return vertexData.normal; } }

        private VertexData vertexData;

        // position in world space coordinates
        public NodeModel model { get; }
        public List<StructureNode> neighbours;

        public StructureNode(Transform planetTransform,
            VertexData vertexData, System.Func<Vector4, NodeModel> getNodeModelFromColor)
        {
            this.planetTransform = planetTransform;
            this.vertexData = vertexData;
            
            localPosition = vertexData.position;
            localRotation = Quaternion.FromToRotation(Vector3.up, vertexData.normal);

            model = getNodeModelFromColor(vertexData.color);
            neighbours = new List<StructureNode>();
        }

        public bool HasSameData(StructureNode other)
        {
            if (other == null) return false;
            if (this == other) return true;
            return planetTransform.Equals(other.planetTransform) && vertexData.Equals(other.vertexData);
        }

        public void addNeighbour(StructureNode other)
        {
            if (this != other)
            {
                if (!neighbours.Contains(other))
                {
                    neighbours.Add(other);
                    this.model.AddNeighbour(other.model);
                    other.addNeighbour(this);
                }
            }
            else
            {
                Debug.LogWarning("Cannot add the node ad neighbour of hinmself. Something strange in the logic.");
            }
        }
    }
}