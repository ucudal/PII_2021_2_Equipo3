namespace Library.Core.Processing
{
    /// <summary>
    /// Represents the functionality of handling one or more messages input until realizing a certain operation successfully,
    /// or until the user indicates to stop trying.
    /// We created this class because of DIP, that way the other classes depend of abractions.
    /// Also we applied the Expert pattern, this specific class is the one in charge of handling messages input.
    /// </summary>
    public abstract class InputHandler
    {
        /// <summary>
        /// Handles a received input message, returning a success signal,
        /// a response string (indicating it's not done yet), or an interrupt signal.
        /// </summary>
        /// <param name="msg">The input message.</param>
        /// <returns>
        /// Result.Err(response), being response a response string, <br />
        /// Result.Ok(true) for a success signal, or <br />
        /// Result.Ok(false) for an interrupt signal.
        /// </returns>
        public abstract Result<bool, string> ProcessInput(string msg);

        /// <summary>
        /// Returns the first message the object uses to indicate what kind of input it wants.
        /// </summary>
        /// <returns>A string.</returns>
        public abstract string GetDefaultResponse();

        /// <summary>
        /// Resets the processor, so it can be used again.
        /// </summary>
        public abstract void Reset();
    }
}
