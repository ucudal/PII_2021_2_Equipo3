namespace Library.Core.Invitations
{
    /// <summary>
    /// This class represents invitations, through which admins can invite other non-registered users into the platform.
    /// </summary>
    public abstract class Invitation
    {
        /// <summary>
        /// The invitation's code.
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// Validates the invitation, returning the response string for that activity.
        /// </summary>
        /// <param name="userId">The id of the user who validated the invitation.</param>
        /// <returns>The response string.</returns>
        public abstract string Validate(UserId userId);
    }
}