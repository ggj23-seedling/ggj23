using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace PlanetStructureTypes
{
    public class StructureNode
    {
        public int index { get; }

        // planet origin in world space
        Transform planetTransform;

        // position in local space coordinates
        public Vector3 localPosition { get; }

        public Quaternion localRotation { get; }

        // position in world space coordinates
        NodeModel model { get; }
        public List<StructureNode> neighbours;

        public StructureNode(int index, Transform planetTransform, VertexData vertexData)
        {
            this.index = index;
            this.planetTransform = planetTransform;

            localPosition = vertexData.position;
            localRotation = Quaternion.FromToRotation(Vector3.up, vertexData.normal);

            model = CreateModel(vertexData);
            neighbours = new List<StructureNode>();
        }

        static NodeModel CreateModel(VertexData vertexData)
        {
            // TODO: parse vertexData to fill the model
            return new NodeModel();
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