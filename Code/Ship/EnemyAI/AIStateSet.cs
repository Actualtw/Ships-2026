using Godot;
using System.Collections.Generic;

namespace GA.Ships
{
    [GlobalClass]
    public partial class AIStateSet : Resource
    {
        [Export] public AIState[] States { get; private set; } = new AIState[0];

        public Dictionary<AIStateID, AIState> BuildLookup()
        {
            Dictionary<AIStateID, AIState> lookup = new Dictionary<AIStateID, AIState>();

            foreach (AIState state in States)
            {
                if (state == null)
                {
                    continue;
                }

                if (state.ID == null)
                {
                    GD.PushWarning($"AIState '{state.ResourcePath}' has no Id and cannot be used in transitions.");
                    continue;
                }

                if (!lookup.TryAdd(state.ID, state))
                {
                    GD.PushWarning($"AIState '{state.ResourcePath}' uses and Id that is already used by '{lookup[state.ID].ResourcePath}'.");
                }
            }

            return lookup;
        }
    }
}
