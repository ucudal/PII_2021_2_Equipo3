using System;
using Library.Core;
using Library.Core.Processing;
using Library.Utils;

namespace Library.States
{
    /// <summary>
    /// This class represents a State which basically works with an <see cref="InputProcessor{T}" />.
    /// </summary>
    public class InputProcessorState : State
    {
        private Func<string, (State?, string?)>? nextStateGetter;
        private Func<string>? initialResponseGetter;

        /// <summary>
        /// Initializes an instance of <see cref="InputProcessorState" />.
        /// </summary>
        public InputProcessorState()
        {
        }

        /// <summary>
        /// Creates a valid instance of <see cref="InputProcessorState" />.
        /// </summary>
        /// <param name="processor">The inner processor on which the <see cref="InputProcessorState" /> depends.</param>
        /// <param name="nextState">The function which determines the next state, given the resulting object from the processor.</param>
        /// <param name="exitState">The function which determines the next state, given that the processor returned an interrupt signal.</param>
        /// <typeparam name="T">The type of the objects generated by the processor.</typeparam>
        /// <returns>An <see cref="InputProcessorState" />.</returns>
        public static InputProcessorState CreateInstance<T>(InputProcessor<T> processor, Func<T, (State?, string?)> nextState, Func<(State?, string?)> exitState)
        {
            InputProcessorState r = new InputProcessorState();
            r.nextStateGetter =
                msg => processor.GenerateFromInput(msg) is Result<T, string> result
                    ? result.Map(
                        v => nextState(v),
                        e => (r, e)
                    ) : exitState();
            r.initialResponseGetter = processor.GetDefaultResponse;
            return r;
        }

        /// <inheritdoc />
        public override (State?, string?) ProcessMessage(string id, ref UserData data, string msg) =>
            (this.nextStateGetter.Unwrap())(msg);

        /// <inheritdoc />
        public override string GetDefaultResponse() =>
            (this.initialResponseGetter.Unwrap())();
    }
}
