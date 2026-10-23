using System.Collections.Generic;
using Godot;

namespace GA.Ships
{
    public partial class EnemyBrain : Node
    {
        [Export] private AIStateSet _states = null;
        [Export] private AIStateID _initialStateID = null;

        private EnemyShip _owner = null;
        private Dictionary<AIStateID, AIState> _stateLookup = new Dictionary<AIStateID, AIState>();

        public AIState CurrentState { get; private set; } = null;

        public void Initialize(EnemyShip owner)
        {
            _owner = owner;

            if (_states != null)
            {
                _stateLookup = _states.BuildLookup();
                // TODO: Activate initial state.
            }
            else
            {
                GD.PushError("EnemyBrain: State lookup table is null.");
            }
        }

        public bool ChangeState(AIStateID targetStateID)
        {
            if (targetStateID == null
                || !_stateLookup.TryGetValue(targetStateID, out AIState targetState))
            {
                // State is not found.
                return false;
            }

            if (CurrentState == targetState)
            {
                // Already in target state.
                return false;
            }

            if (CurrentState != null)
            {
                // Exit CurrentState
                CurrentState.ExitState(_owner);
            }

            CurrentState = targetState;

            if (CurrentState != null)
            {
                CurrentState.EnterState(_owner);
            }

            return true;
        }
    }
}
