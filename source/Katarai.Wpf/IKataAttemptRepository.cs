using System.Collections.Generic;
using Katarai.Wpf.PackagedKata;

namespace Katarai.Wpf
{
    public interface IKataAttemptRepository
    {
        IList<IKataAttempt> GetKataInfos();
        string GetMasterSolutionAssemblyPath(IKataAttempt kataAttempt);
        string GetPlayerSolutionAssemblyPath(IKataAttempt kataAttempt);
        IKataAttempt CreateNewKataAttempt(KataName selectedKata);
        IKataAttempt LoadKataAttemptFrom(string unpackLocation);
    }
}