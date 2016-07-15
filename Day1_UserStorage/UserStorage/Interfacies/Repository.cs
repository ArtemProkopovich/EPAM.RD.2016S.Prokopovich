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
        protected readonly Func<T, bool>[] validationFuncs = new Func<T, bool>[] { (x) => { return true; } };
        protected readonly IEnumerable<int> idSequence;
        protected readonly IEnumerator<int> idEnumerator;

        public Repository()
        {
            idSequence = DefaultIdSequence();
            idEnumerator = idSequence.GetEnumerator();
        }

        public Repository(IEnumerable<int> idSequence) : this()
        {
            if (idSequence != null)
            {
                this.idSequence = idSequence;
                idEnumerator = this.idSequence.GetEnumerator();
            }
        }

        public Repository(IEnumerable<int> idSequence, params Func<T, bool>[] validationFuncs) : this(idSequence)
        {
            this.validationFuncs = validationFuncs;
        }

        public int Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (IsValid(item))
                return AddItem(item);
            throw new InvalidArgumentException("The model is not valid.", nameof(item));
        }

        public abstract IEnumerable<T> SearchAll(Func<T, bool> criteria);
        public abstract IEnumerable<T> SearchAll(params Func<T, bool>[] criterias);
        public abstract T SearchFirst(Func<T, bool> criteria);
        public abstract void Delete(T user);

        public abstract IEnumerable<T> GetAll();

        public abstract T GetById(int id);

        public bool IsValid(T model)
        {
            return validationFuncs.All(e => e(model));
        }

        protected abstract int AddItem(T item);

        protected virtual IEnumerable<int> DefaultIdSequence()
        {
            int prev = 1;
            int current = 1;
            while (true)
            {
                yield return current;
                if (int.MaxValue - current < prev)
                    break;
                int temp = current + prev;
                prev = current;
                current = temp;
            }
        }

        public abstract Repository<T> Clone();

        object ICloneable.Clone()
        {
            return Clone();
        }

        public abstract void Save();
    }
}
