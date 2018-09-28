using System;

namespace Engine
{
    public interface ITestPack<TKataInterface>
    {
        Func<TKataInterface> CreateSUT { get; set; }
    }
}