using Godot;

namespace GA.Ships
{
    public abstract partial class AIActionBase : Resource
    {
        public abstract void Act(EnemyShip ship, float deltaTime = 0);
    }
}
