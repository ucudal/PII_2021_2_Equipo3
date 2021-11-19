using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Library;
using Library.Core.Distribution;
using Library.States.Admins;

namespace UnitTests
{
    /// <summary>
    /// This class represents unit tests related to admin states.
    /// </summary>
    public class AdminStatesTest
    {
        /// <summary>
        /// Tests the class <see cref="AdminInitialMenuState" />'s /createcompany option.
        /// </summary>
        [Test]
        public void TestAdminCreateInvitation()
        {
            Console.WriteLine();
            Singleton<SessionManager>.Instance.NewUser("___", new Library.Core.UserData(), new AdminInitialMenuState());
            ProgramaticPlatform platform = new ProgramaticPlatform(
                "___",
                new string[]
                {
                    "/invitecompany"
                }
            );

            platform.Run();

            foreach(string msg in platform.ReceivedMessages)
            {
                Console.WriteLine("\t--------");
                Console.WriteLine(msg);
                Console.WriteLine("\t--------");
            }

            Regex expected = new Regex(
                "The new invitation's code is (?<invitationcode>\\w+).\\n"
              + "What do you want to do\\?\\n"
              + "        /invitecompany: Create a company invitation and get its code\\n"
              + "        /removecompany: Remove a company and its users\\n"
              + "        /removeuser: Remove a user",
                RegexOptions.Compiled
            );

            

            Match match = expected.Match(platform.ReceivedMessages[0]);
            Assert.That(match.Success, Is.True);

            string invitationCode = match.Groups["invitationcode"].Value;
            Console.WriteLine($"Invitation code: {invitationCode}");
        }
    }
}