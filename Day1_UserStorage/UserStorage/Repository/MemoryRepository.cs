﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Interfacies;
using UserStorage.Entity;

namespace UserStorage.Repository
{
    public class MemoryRepository : Repository<User>
    {
        private readonly List<User> userList = new List<User>();

        public MemoryRepository() : base()
        {
        }

        public MemoryRepository(IEnumerable<User> users)
        {
            userList = users.Select(e => e.Clone()).ToList();
            ConfigureIdSequence();
        }

        public MemoryRepository(IEnumerable<int> idSequence) : base(idSequence)
        {
        }

        public MemoryRepository(IEnumerable<User> users, IEnumerable<int> idSequence) : this(idSequence)
        {
            userList = users.Select(e => e.Clone()).ToList();
            ConfigureIdSequence();
        }

        public MemoryRepository(IEnumerable<int> idSequence, params Func<User, bool>[] validationFuncs) : base(idSequence, validationFuncs)
        {
        }

        public MemoryRepository(IEnumerable<User> users, IEnumerable<int> idSequence, params Func<User, bool>[] validationFuncs) : this(idSequence, validationFuncs)
        {
            userList = users.Select(e => e.Clone()).ToList();
            ConfigureIdSequence();
        }

        public override Repository<User> Clone()
        {
            return new MemoryRepository(userList, idSequence, validationFuncs);
        }

        public override void Delete(User user)
        {
            userList.RemoveAll(e => e.Equals(user));
        }

        public override IEnumerable<User> GetAll()
        {
            var array = new User[userList.Count];
            userList.CopyTo(array);
            return array.ToList();
        }

        public override User GetById(int id)
        {
            return userList.FirstOrDefault(e => e.Id == id);
        }

        public override void Save()
        {
            
        }

        public override IEnumerable<User> SearchAll(params Func<User, bool>[] criterias)
        {
            IEnumerable<User> result = new List<User>();
            foreach (var func in criterias)
            {
                result = result.Union(SearchAll(func));
            }
            return result;
        }

        public override IEnumerable<User> SearchAll(Func<User, bool> criteria)
        {
            return userList.Where(criteria);
        }

        public override User SearchFirst(Func<User, bool> criteria)
        {
            return userList.FirstOrDefault(criteria);
        }

        protected override int AddItem(User item)
        {
            if (idEnumerator.MoveNext())
            {
                var newItem = item.Clone();
                newItem.Id = idEnumerator.Current;
                userList.Add(newItem);
                return newItem.Id;
            }
            throw new EndedIdValuesException();
        }

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
                throw new EndedIdValuesException();
            }
        }
    }
}