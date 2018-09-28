using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Runners;

namespace Engine
{
    public enum KataActivityLoopStates
    {
        Unknown,
        Red
    }

    public class KataStateDeterminer<TKata> 
        where TKata : class
    {
        protected List<Type> GoldenImplementationTypes { get; set; }
        protected Type PlayerTestType { get; set; }
        protected Type PlayerImplementationType { get; set; }
        protected Type GoldenTestType { get; set; }

        public KataStateDeterminer(Type playerTestType, Type playerImplementationType, Type goldenTestType, List<Type> goldenImplementationTypes)
        {
            if (playerTestType == null) throw new ArgumentNullException("playerTestType");
            if (playerImplementationType == null) throw new ArgumentNullException("playerImplementationType");
            if (goldenTestType == null) throw new ArgumentNullException("goldenTestType");
            if (goldenImplementationTypes == null) throw new ArgumentNullException("goldenImplementationTypes");
            if (!goldenImplementationTypes.Any()) throw new ArgumentException("No golden implementation types provided.", "goldenImplementationTypes");
            GoldenTestType = goldenTestType;
            PlayerTestType = playerTestType;
            PlayerImplementationType = playerImplementationType;
            GoldenImplementationTypes = goldenImplementationTypes;
        }

        public KataActivityLoopStates State
        {
            get
            {
                return KataActivityLoopStates.Unknown;
            }

        }

        public void Run()
        {
            var playerImplementationLevelDeterminer = new PlayerImplementationRunner<TKata>(PlayerImplementationType, GoldenTestType);
            //playerImplementationLevelDeterminer.Run();
        }
    }
}