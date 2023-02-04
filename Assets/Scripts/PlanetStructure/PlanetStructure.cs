using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlanetStructureTypes
{
    public class PlanetStructure
    {
        public StructureNode[] nodes { get; }

        public PlanetStructure(StructureNode[] nodes)
        {
            this.nodes = nodes;
        }

        public PlanetStructure() : this(new StructureNode[0]) {}
    }
}