using System;
using System.Dynamic;
using Katarai.Utils;
using Katarai.Wpf.PackagedKata;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Katarai.Wpf
{
    public interface IKataAttempt
    {
        string Name { get; set; }
        string Location { get; set; }
        IKataAttemptConfiguration Config { get; }
    }

    public class KataAttempt : IKataAttempt
    {
        private readonly IFileSystem _adapter;
        public string Name { get; set; }
        public string Location { get; set; }

        public KataAttempt(IFileSystem adapter)
        {
            _adapter = adapter;
        }

        public IKataAttemptConfiguration Config
        {
            get
            {
                var configLocation = _adapter.Combine(Location,"config.json");
                var masterDll = string.Empty;
                var solutionName = string.Empty;
                var playerDll = string.Empty;
                var kataName = string.Empty;
                var instructions = new Instructions();
                if (_adapter.FileExists(configLocation))
                {
                    var readAllText = _adapter.ReadAllText(configLocation);
                    dynamic deserializeObject = JsonConvert.DeserializeObject<ExpandoObject>(readAllText, new ExpandoObjectConverter());
                    if (deserializeObject != null)
                    {
                        masterDll = deserializeObject.masterSolutionAssembly;
                        solutionName = deserializeObject.playerSolutionName;
                        playerDll = deserializeObject.playerSolutionAssembly;
                        kataName = deserializeObject.kataName;
                        SetInstructions(instructions, deserializeObject);
                    }

                }

                return new KataAttemptConfiguration(masterDll, solutionName, playerDll, Location,instructions,kataName);
            }
        }

        private void SetInstructions(Instructions instructions, dynamic deserializeObject)
        {
            instructions.Header = deserializeObject.instructions.header + Environment.NewLine;

            foreach (var instruction in deserializeObject.instructions.content)
            {
                instructions.Content.Add(instruction.ToString());
                instructions.Content.Add(Environment.NewLine);
            }
        }
    }

}