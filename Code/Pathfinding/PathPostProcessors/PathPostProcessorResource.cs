using System.Collections.Generic;
using Godot;

namespace GA.Ships.Pathfinding
{
    public abstract partial class PathPostProcessorResource : Resource, IPathPostProcessor
    {
        public abstract IList<Vector3> PostProcess(IList<Vector3> path);
    }
}
