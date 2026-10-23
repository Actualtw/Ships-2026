using GA.Common;
using Godot;

namespace GA.Ships
{
    public partial class EnemyShip : ShipBase
    {
        // EnemyAI system should be stateless. This means, all enemy data
        // should be stored here. States and actions modify that data.

        public EnemyBrain Brain { get; private set; } = null;

        public override void _Ready()
        {
            base._Ready();

            Brain = this.GetNode<EnemyBrain>();

            if (Brain != null)
            {
                Brain.Initialize(this);
            }
            else
            {
                GD.PushError("Brain for the enemy ship can't be found!");
            }
        }
    }
}
