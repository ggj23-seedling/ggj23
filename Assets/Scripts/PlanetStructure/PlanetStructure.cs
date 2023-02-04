using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlanetStructureTypes
{
    public class PlanetStructure
    {
        public List<StructureNode> nodes { get; }

        public PlanetStructure(List<StructureNode> nodes)
        {
            this.nodes = nodes;
        }

        public PlanetStructure() : this(new List<StructureNode>()) {}
    }
}