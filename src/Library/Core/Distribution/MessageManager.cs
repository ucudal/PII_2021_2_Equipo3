using Library.Core.Invitations;
using Library.States.Entrepreneurs;

namespace Library.Core.Distribution
{
    /// <summary>
    /// This class represents the highest level of encapsulation in message processing.
    /// Created because of SRP, in that way the information is encapsulated. At the same
    /// time it uses the OCP principle because this class is open to extensions but closed
    /// to modifications.
    /// </summary>
    public class MessageManager
    {
        /// <summary>
        /// Processes a received message, returning the text of the response message.
        /// </summary>
        /// <param name="msg">The received message.</param>
        /// <returns>The response message's text.</returns>
        public string ProcessMessage(Message msg)
        {
            if (Singleton<SessionManager>.Instance.GetById(msg.Id) is UserSession session)
            {
                return session.ProcessMessage(msg.Text);
            }
            else
            {
                return processMessageFromUnknownUser(msg);
            }
        }

        private static string processMessageFromUnknownUser(Message msg)
        {
            string[] args = msg.Text.Split(' ');

            if (
                args.Length != 2 ||
                args[0] != "/start" ||
                string.IsNullOrWhiteSpace(args[1]))
            {
                return "Por favor, envía el mensaje \"/start <código>\" para registrarse como empresa o \"/start -e\" o \"/start --entrepreneur\" para registrarse como emprendedor.";
            }

            string arg = args[1].Trim();
            if (arg == "-e" || arg == "--entrepreneur")
            {
                State newState = new NewEntrepreneurState(msg.Id);

                Singleton<SessionManager>.Instance.NewUser(msg.Id, new UserData(string.Empty, false, UserData.Type.ENTREPRENEUR, null, null), newState);

                return newState.GetDefaultResponse();
            }

            string invitationCode = arg;
            return Singleton<InvitationManager>.Instance.ValidateInvitation(invitationCode, msg.Id) is string result
                ? result
                : "Invalid invitation code";
        }
    }
}
