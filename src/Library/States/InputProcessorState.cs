using System;
using Library.Core;
using Library.Core.Processing;
using Library.Utils;

namespace Library.States
{
    /// <summary>
    /// This class represents a State which basically works with an <see cref="InputProcessor{T}" />.
    /// </summary>
    /// <typeparam name="T">The type of the objects generated by the processor.</typeparam>
    public class InputProcessorState<T> : State
    {
        private InputProcessor<T> processor;
        private Func<T, (State?, string?)> nextState;
        private Func<(State?, string?)> exitState;

        /// <summary>
        /// Initializes an instance of <see cref="InputProcessorState{T}" />.
        /// </summary>
        /// <param name="processor">The inner processor on which the <see cref="InputProcessorState{T}" /> depends.</param>
        /// <param name="nextState">The function which determines the next state, given the resulting object from the processor.</param>
        /// <param name="exitState">The function which determines the next state, given that the processor returned an interrupt signal.</param>
        public InputProcessorState(InputProcessor<T> processor, Func<T, (State?, string?)> nextState, Func<(State?, string?)> exitState)
        {
            this.processor = processor;
            this.nextState = nextState;
            this.exitState = exitState;
        }

        /// <inheritdoc />
        public override (State?, string?) ProcessMessage(string id, string msg) =>
            this.processor.GenerateFromInput(msg) is Result<T, string> result
                    ? result.Map(
                        v =>
                        {
                            var (newState, res) = nextState(v);
                            if(newState is null && res is not null)
                            {
                                newState = this;
                                res = $"{res}\n{this.GetDefaultResponse()}";
                            }
                            
                            return (newState, res);
                        },
                        e => (this, e)
                    ) : exitState();

        /// <inheritdoc />
        public override string GetDefaultResponse() =>
            this.processor.GetDefaultResponse();
    }
}
