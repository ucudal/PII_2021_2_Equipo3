using System;
using System.Collections.Generic;
using System.Linq;
using Library.Utils;

namespace Library.Core.Distribution
{
    /// <summary>
    /// This class handles the creation and selection of user sessions.
    /// This class use the Creator pattern, because it is the responsible of
    /// creating an instance of user session.
    /// </summary>
    public class SessionManager
    {
        /// <summary>
        /// Gets a set of functions to remove users
        /// from their correspondent storages.
        /// </summary>
        private IList<Action<string>> removers = new List<Action<string>>();

        /// <summary>
        /// Adds a remover into the set.
        /// </summary>
        /// <param name="remover">The remover.</param>
        public void AddRemover(Action<string> remover)
        {
            if (!this.removers.Contains(remover))
                this.removers.Add(remover);
        }

        /// <summary>
        /// The list of current sessions.
        /// The class <see cref="List{T}" /> is used instead of the interface <see cref="IList{T}" />
        /// because the method <see cref="List{T}.RemoveAll(System.Predicate{T})" /> is necessary for the method <see cref="SessionManager.RemoveUser(string)" />.
        /// </summary>
        private List<UserSession> sessions = new List<UserSession>();

        /// <summary>
        /// Returns the <see cref="UserSession" /> whose id equals to the given one.
        /// </summary>
        /// <param name="id">The given id.</param>
        /// <returns>Its corresponding <see cref="UserSession" />, or null if there isn't.</returns>
        public UserSession? GetById(string id) =>
            this.sessions.Where(session => session.MatchesId(id)).FirstOrDefault();

        /// <summary>
        /// Returns the <see cref="UserSession" /> whose name equals to the given one.
        /// </summary>
        /// <param name="name">The given name.</param>
        /// <returns>Its corresponding <see cref="UserSession" />, or null if there isn't.</returns>
        public UserSession? GetByName(string name) => 
            this.sessions.Where(session => session.MatchesName(name)).FirstOrDefault();

        /// <summary>
        /// Adds a new user into the platform.
        /// </summary>
        /// <param name="id">The user's id.</param>
        /// <param name="userData">The user's data.</param>
        /// <param name="state">The user's initial state.</param>
        /// <returns>The resulting <see cref="UserSession" />, or null if
        /// there's already one with the same id or name.</returns>
        public UserSession? NewUser(string id, UserData userData, State state)
        {
            if (this.GetById(id) is not null || this.GetByName(userData.Name) is not null)
            {
                return null;
            }

            UserSession result = new UserSession(id, userData, state);
            this.sessions.Add(result);
            return result;
        }

        /// <summary>
        /// Removes the user with a concrete id.
        /// </summary>
        /// <param name="id">The user's id.</param>
        /// <returns>Whether there was an user with the given id.</returns>
        public bool RemoveUser(string id)
        {
            foreach (Action<string> remover in this.removers.ToList())
            {
                remover(id);
            }
            return this.sessions.RemoveAll(session => session.Id == id) > 0;
        }

        /// <summary>
        /// Removes the user with a concrete name.
        /// </summary>
        /// <param name="name">The user's name.</param>
        /// <returns>Whether there was an user with the given name.</returns>
        public bool RemoveUserByName(string name) =>
            this.GetByName(name) is UserSession session
                ? this.RemoveUser(session.Id)
                : false;

        /// <summary>
        /// Loads all user sessions from JSON.
        /// </summary>
        /// <param name="path">The main directory's path.</param>
        public void LoadUserSessions(string path)
        {
            IEnumerable<UserSession> sessions = SerializationUtils.DeserializeJson<UserSession[]>($"{path}/sessions.json");
            this.sessions = sessions.ToList();
        }

        /// <summary>
        /// Saves all user sessions into JSON.
        /// </summary>
        /// <param name="path">The main directory's path.</param>
        public void SaveUserSessions(string path)
        {
            UserSession[] sessions = this.sessions.ToArray();
            SerializationUtils.SerializeJson<UserSession[]>($"{path}/sessions.json", sessions);
        }
    }
}
