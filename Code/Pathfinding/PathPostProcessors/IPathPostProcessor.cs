using System.Collections;
using System.Collections.Generic;
using Godot;

namespace GA.Ships.Pathfinding
{
    public interface IPathPostProcessor
    {
        IList<Vector3> PostProcess(IList<Vector3> path);
    }
}
