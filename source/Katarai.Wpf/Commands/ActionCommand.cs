using System;

namespace Katarai.Wpf.Commands
{
    public class ActionCommand : ActionCommandBase
    {
        public ActionCommand(Action toRun, Func<bool> executeCheck = null)
        {
            Init(toRun, executeCheck);
        }
    }
}