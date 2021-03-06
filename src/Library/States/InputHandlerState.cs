using System;
using Library.Core;
using Library.Core.Processing;

namespace Library.States
{
    /// <summary>
    /// This class represents a State which basically works with an <see cref="InputHandler" />.
    /// </summary>
    public class InputHandlerState : State
    {
        private InputHandler inputHandler { get; }

        private Func<(State?, string?)> exitState { get; }

        private Func<(State?, string?)> nextState { get; }

        /// <summary>
        /// Initializes an instance of <see cref="InputHandlerState" />.
        /// </summary>
        /// <param name="inputHandler">The handler which determines the course of the state.</param>
        /// <param name="exitState">The function which determines the state to go when an interrupt signal is given.</param>
        /// <param name="nextState">The function which determines the state to go when a success signal is given.</param>
        public InputHandlerState(InputHandler inputHandler, Func<(State?, string?)> exitState, Func<(State?, string?)> nextState)
        {
            this.inputHandler = inputHandler;
            this.exitState = exitState;
            this.nextState = nextState;
        }

        /// <inheritdoc />
        public override (State?, string?) ProcessMessage(string id, string msg) =>
            inputHandler.ProcessInput(msg).Map<(State?, string?)>(
                success => (success ? (this.nextState)() : (this.exitState)()),
                s => (this, s)
            );

        /// <inheritdoc />
        public override string GetDefaultResponse() => this.inputHandler.GetDefaultResponse();
    }
}
