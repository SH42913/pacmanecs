using Leopotam.Ecs;

namespace Items.Food.Systems
{
    [EcsInject]
    public class EnergizerSystem : IEcsRunSystem
    {
        private readonly EcsFilter<EnergizerComponent, TakenItemComponent> _takenEnergizers = null;

        public void Run()
        {
            for (int i = 0; i < _takenEnergizers.EntitiesCount; i++)
            {
                //ToDo Ghost fear mode enable
            }
        }
    }
}