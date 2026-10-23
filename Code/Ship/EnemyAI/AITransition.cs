using Godot;

namespace GA.Ships
{
    /// <summary>
    /// One-way transition to a target state. The transition to that state
    /// should happen when Condition evaluates to true.
    /// </summary>
    [GlobalClass]
    public partial class AITransition : Resource
    {
        [Export] public AIConditionBase Condition { get; private set; }
        [Export] public AIStateID TargetStateID { get; private set; }
    }
}
