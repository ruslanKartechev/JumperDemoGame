﻿using Ecs;
using Ecs.Components;
using Ecs.Systems;
using Ecs.Systems.Init;
using Leopotam.EcsLite;
using Zenject;

namespace Installers
{
    public class EcsInstaller : MonoInstaller
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            var world = new EcsWorld();
            Pool.World = world;
            
            var runner = new SystemRunner();
            Container.Inject(runner);
            Container.Bind<SystemRunner>().FromInstance(runner);
            
            var initSystems = new EcsSystems(world);
            var updateSystems = new EcsSystems(world);
            var fixedSystems = new EcsSystems(world);
            
            // init
            MakeInitSystem<InitGameSystem>(initSystems);
            // fixed update
            // lateUpdate
            // update
            MakeUpdateAndInitSystem<InputCheckSystem>(updateSystems, initSystems);
            MakeUpdateAndInitSystem<LoadLevelSystem>(updateSystems, initSystems);
            MakeUpdateAndInitSystem<FillCellSystem>(updateSystems, initSystems);
            MakeUpdateAndInitSystem<SpawnPlayerSystem>(updateSystems, initSystems);
            MakeUpdateAndInitSystem<TransformMoveSystem>(updateSystems, initSystems);
            MakeUpdateAndInitSystem<SetPlayerMoveDestinationSystem>(updateSystems, initSystems);
            MakeUpdateAndInitSystem<LerpJumpMoveSystem>(updateSystems, initSystems);
            MakeUpdateAndInitSystem<SpawnBlockCountdownSystem>(updateSystems, initSystems);
            MakeUpdateAndInitSystem<StartLevelSystem>(updateSystems, initSystems);
            MakeUpdateAndInitSystem<SpawnRandomBlockSystem>(updateSystems, initSystems);
            MakeUpdateAndInitSystem<SetCameraLookPositionSystem>(updateSystems, initSystems);
            MakeUpdateAndInitSystem<MoveCameraSystem>(updateSystems, initSystems);
            MakeUpdateAndInitSystem<DropMoveSystem>(updateSystems, initSystems);
            MakeUpdateAndInitSystem<JumpPoofSystem>(updateSystems, initSystems);

            runner.AddInit(initSystems);
            runner.AddUpdate(updateSystems);
            runner.AddFixedUpdate(fixedSystems);
        }


        
        
        private T MakeUpdateAndInitSystem<T>(EcsSystems updateSystems, EcsSystems iniSystems) where T : class, IEcsRunSystem, IEcsInitSystem, new()
        {
            var instance = new T();
            Container.Inject(instance);
            updateSystems.Add(instance);
            iniSystems.Add(instance);
            return instance;
        }
        

        private T MakeUpdateSystem<T>(EcsSystems updateSystems) where T : class, IEcsRunSystem, new()
        {
            var instance = new T();
            Container.Inject(instance);
            updateSystems.Add(instance);
            return instance;
        }

        private T MakeInitSystem<T>(EcsSystems initSystems) where T : class, IEcsInitSystem, new()
        {
            var instance = new T();
            Container.Inject(instance);
            initSystems.Add(instance);
            return instance;
        }
        
        

        private T CreateSystem<T>() where T : class, new()
        {
            var instance = new T();
            Container.Inject(instance);
            return instance;
        }
    }
}