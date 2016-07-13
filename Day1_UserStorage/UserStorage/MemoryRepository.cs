using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Interfacies;
using UserStorage.Entity;

namespace UserStorage
{
    public class MemoryRepository : Repository<User>
    {
        private readonly List<User> userList = new List<User>();  

        public MemoryRepository() : base()
        {
        }

        public MemoryRepository(IEnumerable<int> idSequence) : base(idSequence)
        {
        }

        public MemoryRepository(IEnumerable<int> idSequence, params Func<User, bool>[] validationFuncs) : base(idSequence, validationFuncs)
        {
        }

        public override void Delete(User user)
        {
            userList.RemoveAll(e => e.Equals(user));
        }

        public override IEnumerable<User> GetAll()
        {
            var array = new User[userList.Count];
            userList.CopyTo(array);
            return array;
        }

        public override User GetById(int id)
        {
            return userList.FirstOrDefault(e => e.PersonalId == id);
        }

        public override IEnumerable<int> SearchAll(params Func<User, bool>[] criterias)
        {
            IEnumerable<int> result = new List<int>();
            foreach(var func in criterias)
            {
                result = result.Union(SearchAll(func));
            }
            return result;
        }

        public override IEnumerable<int> SearchAll(Func<User, bool> criteria)
        {
            return userList.Where(criteria).Select(e => e.PersonalId);
        }

        public override int SearchFirst(Func<User, bool> criteria)
        {
            return userList.FirstOrDefault(criteria)?.PersonalId ?? 0;
        }

        protected override int AddItem(User item)
        {
            var enumerator = idSequence;
            if(enumerator.MoveNext())
            {
                var newItem = item.Clone();
                newItem.PersonalId = enumerator.Current;
                userList.Add(newItem);
                return newItem.PersonalId;
            }
            throw new EndedIdValuesException();
        }
    }
}
