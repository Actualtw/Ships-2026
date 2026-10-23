using Godot;

namespace GA.Ships
{
    public abstract partial class AIConditionBase : Resource
    {
        public abstract bool Decide(EnemyShip ship);
    }
}
