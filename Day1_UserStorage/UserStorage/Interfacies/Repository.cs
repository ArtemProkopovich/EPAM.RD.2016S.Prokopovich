using System;
using System.Collections.Generic;
using System.Linq;
using UserStorage.Entity;

namespace UserStorage.Interfacies
{
    /// <summary>
    /// Abstract implementation of IRepository with implemented validation logic
    /// </summary>
    /// <typeparam name="T">Type of objects in repository</typeparam>
    [Serializable]
    public abstract class Repository<T>: IRepository<T>
    {
        /// <summary>
        /// Delegates that checked validation of object
        /// </summary>
        protected Func<T, bool>[] validationFuncs { get; set; } = new Func<T, bool>[] { (x) => { return true; } };
        /// <summary>
        /// Sequence for generating Id.
        /// </summary>
        protected IEnumerable<int> idSequence { get; set; }
        /// <summary>
        /// Enumerator if idSequence
        /// </summary>
        protected IEnumerator<int> idEnumerator { get; set; }

        public Repository()
        {
            idSequence = DefaultIdSequence().ToList();
            idEnumerator = idSequence.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idSequence">Sequence for generating Id.</param>
        public Repository(IEnumerable<int> idSequence) : this()
        {
            if (idSequence != null)
            {
                this.idSequence = idSequence;
                idEnumerator = this.idSequence.GetEnumerator();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idSequence">Sequence for generating Id.</param>
        /// <param name="validationFuncs">Delegates that checked validation of object</param>
        public Repository(IEnumerable<int> idSequence, params Func<T, bool>[] validationFuncs) : this(idSequence)
        {
            this.validationFuncs = validationFuncs;
        }

        /// <summary>
        /// Add item in repository
        /// </summary>
        /// <param name="item">The item that needs add</param>
        /// <returns></returns>
        public int Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (IsValid(item))
                return AddItem(item);
            throw new InvalidArgumentException("The model is not valid.", nameof(item));
        }

        /// <summary>
        /// Search all elemetns that fir to criteria
        /// </summary>
        /// <param name="searchCriteria">Criteria for search</param>
        /// <returns></returns>
        public abstract IEnumerable<T> SearchAll(Func<User, bool> searchCriteria);
        /// <summary>
        /// Delete item from repository
        /// </summary>
        /// <param name="item"></param>
        public abstract void Delete(T item);
        /// <summary>
        /// Validate item
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool IsValid(T model)
        {
            return validationFuncs.All(e => e(model));
        }

        /// <summary>
        /// Add item in repository
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected abstract int AddItem(T item);

        /// <summary>
        /// Return fibonacci sequence to generating Id.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Clone repository
        /// </summary>
        /// <returns></returns>
        public abstract Repository<T> Clone();

        /// <summary>
        /// Clone repository
        /// </summary>
        /// <returns></returns>
        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
