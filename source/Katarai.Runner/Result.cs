using System;
using Engine.Runners;

namespace Katarai.Runner
{
    [Serializable]
    public class Result
    {
        public PlayerImplementationRunResult PlayerImplementationRunResult { get; private set; }
        public PlayerTestsRunResult PlayerTestsRunResult { get; private set; }

        public enum GameStates
        {
            Unknown,
            Red,
            Green
        }

        public PlayerFeedback PlayerFeedback { get; private set; }
        public GameStates GameState { get; private set; }
        public int PlayerImplementationLevel { get; private set; }
        public int PlayerTestLevel { get; private set; }
        public DateTime Time { get; protected set; }

        public Result(int playerImplementationlevel, int playerTestLevel, PlayerFeedback playerFeedback)
        {
            PlayerFeedback = playerFeedback;
            PlayerImplementationLevel = playerImplementationlevel;
            PlayerTestLevel = playerTestLevel;
            GameState = GameStates.Unknown; // TODO: determine state from levels
            Time = DateTime.Now;

        }

        public Result(PlayerImplementationRunResult playerImplementationRunResult, PlayerTestsRunResult playerTestsRunResult, PlayerFeedback playerFeedback):
            this(playerImplementationRunResult.Level,playerTestsRunResult.Level,playerFeedback)
        {
            PlayerImplementationRunResult = playerImplementationRunResult;
            PlayerTestsRunResult = playerTestsRunResult;
        }

        public bool IsPlayerTestStateGreen()
        {
            return PlayerFeedback.PlayerTestState.Contains("Green");
        }

        public bool IsPlayerTestStateRed()
        {
            return PlayerFeedback.PlayerTestState.Contains("Red");
        }
    }
}
