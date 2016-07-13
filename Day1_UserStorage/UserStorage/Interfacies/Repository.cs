using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Entity;

namespace UserStorage.Interfacies
{
    public abstract class Repository<T>: IRepository<T>
    {
        private readonly Func<T, bool> validationDelegate = (x) => { return true; };
        protected IEnumerator<int> idSequence;

        public Repository()
        {
            idSequence = DefaultIdSequence().GetEnumerator();
        }

        public Repository(IEnumerable<int> idSequence) : this()
        {
            if (idSequence != null)
                this.idSequence = idSequence.GetEnumerator();
        }

        public Repository(IEnumerable<int> idSequence, params Func<T, bool>[] validationFuncs) : this(idSequence)
        {
            foreach (var func in validationFuncs)
                if (func != null)
                    validationDelegate += func;
                else
                    throw new ArgumentNullException(nameof(func));
        }

        public int Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (IsValid(item))
                return AddItem(item);
            throw new InvalidArgumentException("The model is not valid.", nameof(item));
        }

        public abstract void Delete(User user);

        public abstract IEnumerable<T> GetAll();

        public abstract T GetById(int id);

        public bool IsValid(T model)
        {
            foreach(var func in validationDelegate.GetInvocationList())
            {
                if (!(bool)func.DynamicInvoke(model))
                    return false;
            }
            return true;
        }

        public abstract IEnumerable<int> SearchAll(Func<T, bool> criteria);

        public abstract int SearchFirst(Func<T, bool> criteria);

        protected abstract int AddItem(T item);

        protected virtual IEnumerable<int> DefaultIdSequence()
        {
            int i = 1;
            while (i < int.MaxValue)
            {
                yield return i;
                i++;
            }
        }

        public abstract IEnumerable<int> SearchAll(params Func<T, bool>[] criterias);
    }
}
