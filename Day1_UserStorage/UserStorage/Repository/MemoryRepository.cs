using System;
using System.Collections.Generic;
using System.Linq;
using UserStorage.Interfacies;
using UserStorage.Entity;

namespace UserStorage.Repository
{
    [Serializable]
    public class MemoryRepository : Repository<User>
    {
        private readonly List<User> userList = new List<User>();

        public MemoryRepository() : base()
        {
        }
        /// <summary>
        /// Create memory repository object with this state of users
        /// </summary>
        /// <param name="users">Users that init repository</param>
        public MemoryRepository(IEnumerable<User> users)
        {
            userList = users.Select(e => e.Clone()).ToList();
            ConfigureIdSequence();
        }

        /// <summary>
        /// Create memory repository
        /// </summary>
        /// <param name="idSequence">Sequence for generating id's</param>
        public MemoryRepository(IEnumerable<int> idSequence) : base(idSequence)
        {
        }

        /// <summary>
        /// Create memory repository
        /// </summary>
        /// <param name="users">Users that init repository</param>
        /// <param name="idSequence">Sequence for generating id's</param>
        public MemoryRepository(IEnumerable<User> users, IEnumerable<int> idSequence) : this(idSequence)
        {
            userList = users.Select(e => e.Clone()).ToList();
            ConfigureIdSequence();
        }

        /// <summary>
        /// Create memory repository
        /// </summary>
        /// <param name="idSequence">Sequence for generating id's</param>
        /// <param name="validationFuncs">Functions for validation adding users</param>
        public MemoryRepository(IEnumerable<int> idSequence, params Func<User, bool>[] validationFuncs) : base(idSequence, validationFuncs)
        {
        }
        /// <summary>
        /// Create memory repository
        /// </summary>
        /// <param name="users">Users that init repository</param>
        /// <param name="idSequence">Sequence for generating id's</param>
        /// <param name="validationFuncs">Functions for validation adding users</param>
        public MemoryRepository(IEnumerable<User> users, IEnumerable<int> idSequence, params Func<User, bool>[] validationFuncs) : this(idSequence, validationFuncs)
        {
            userList = users.Select(e => e.Clone()).ToList();
            ConfigureIdSequence();
        }

        /// <summary>
        /// Clone state of repository
        /// </summary>
        /// <returns></returns>
        public override Repository<User> Clone()
        {
            return new MemoryRepository(userList, idSequence, validationFuncs);
        }

        /// <summary>
        /// Delete user from repository
        /// </summary>
        /// <param name="user"></param>
        public override void Delete(User user)
        {
            userList.RemoveAll(e => e.Equals(user));
        }

        /// <summary>
        /// Search all elements in repository that fir to criteria
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public override IEnumerable<User> SearchAll(Func<User, bool> searchCriteria)
        {
            return userList.Where(searchCriteria).ToList();
        }

        /// <summary>
        /// Add item in repsoitory
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override int AddItem(User item)
        {
            if (idEnumerator.MoveNext())
            {
                var newItem = item.Clone();
                newItem.Id = idEnumerator.Current;
                userList.Add(newItem);
                return newItem.Id;
            }
            throw new EndedIdValuesException("The id sequence is over");
        }

        /// <summary>
        /// Init current state of id sequence
        /// </summary>
        private void ConfigureIdSequence()
        {
            if (userList.Count > 0)
            {
                int id = userList.Max(e => e.Id);
                while (idEnumerator.MoveNext())
                {
                    if (idEnumerator.Current >= id)
                        return;
                }
                throw new EndedIdValuesException("The id sequence is over");
            }
        }
    }
}
