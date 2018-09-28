using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Annotations;
using Katarai.StringCalculator.Interfaces;

namespace Katarai.StringCalculator.Golden.Levels
{
    [TestStep(0)]
    public class Player_StringCalculator_AtLevel0 : IStringCalculator
    {
        public Player_StringCalculator_AtLevel0()
        {
            throw new NotImplementedException();
        }

        public int Add(string input)
        {
            throw new NotImplementedException();
        }
    }
    [TestStep(1)]
    public class Player_StringCalculator_AtLevel1 : IStringCalculator
    {
        public int Add(string input)
        {
            throw new NotImplementedException();
        }
    }

    [TestStep(2)]
    public class Player_StringCalculator_AtLevel2 : IStringCalculator
    {
        public int Add(string input)
        {
            if (input == "")
                return 0;
            throw new NotImplementedException("DIE");
        }
    }

    [TestStep(3)]
    public class Player_StringCalculator_AtLevel3 : IStringCalculator
    {
        public int Add(string input)
        {
            if (input == "") return 0;
            return int.Parse(input);
        }
    }

    [TestStep(4)]
    public class Player_StringCalculator_AtLevel4 : IStringCalculator
    {
        public int Add(string input)
        {
            if (input == "") return 0;
            var parts = input.Split(new [] {','}, 2);
            return parts.Select(int.Parse).Sum();
            var joinPos = input.IndexOf(',');
            if (joinPos == -1) return int.Parse(input);
            return int.Parse(input.Substring(0, joinPos)) + int.Parse(input.Substring(joinPos + 1));
        }
    }

    [TestStep(5)]
    public class Player_StringCalculator_AtLevel5 : IStringCalculator
    {
        public int Add(string input)
        {
            if (input == "") return 0;
            var parts = input.Split(new [] {','});
            return parts.Select(int.Parse).Sum();
        }
    }

    [TestStep(6)]
    public class Player_StringCalculator_AtLevel6 : IStringCalculator
    {
        public int Add(string input)
        {
            if (input == "") return 0;
            var parts = input.Split(new[] { ',', '\n' });
            return parts.Select(int.Parse).Sum();
        }
    }
}
