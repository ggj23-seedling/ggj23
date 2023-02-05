using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlanetStructureTypes
{
    public class PlanetStructure
    {
        public List<StructureNode> nodes { get; }

        public StructureNode root { get; }

        public PlanetStructure(List<StructureNode> nodes, StructureNode root)
        {
            this.nodes = nodes;
            this.root = root;
        }

        public PlanetStructure() : this(new List<StructureNode>(), null) {}
    }
}