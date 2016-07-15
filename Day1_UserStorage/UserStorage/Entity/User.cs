using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.Entity
{
    public class User: IEquatable<User>, IComparable<User>, ICloneable
    {
        public int Id { get; set; }
        public string PersonalId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender? Gender { get; set; }
        public Visa[] Visas { get; set; }
        private readonly Comparison<User> comparison;

        public User() {
            comparison = Compare;
        }
        public User(Comparison<User> comparison)
        {
            if (comparison != null)
                this.comparison = comparison;
            else
                comparison = Compare;
                
        }

        public override bool Equals(object obj)
        {
            if (obj != null) {
                var user = obj as User;
                return Equals(user);
            }
            return false;
        }

        public override int GetHashCode()
        {
            int fNameHash = FirstName != null ? FirstName.GetHashCode() : 0;
            int lNameHash = LastName != null ? LastName.GetHashCode() : 0;
            return fNameHash.GetHashCode() ^ lNameHash.GetHashCode();
        }

        public int CompareTo(User other)
        {
            return comparison(this, other);
        }

        private int Compare(User x, User y)
        {
            if (ReferenceEquals(x, y))
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;
            int lNameCompare = x.LastName.CompareTo(y.LastName);
            if (lNameCompare != 0)
                return lNameCompare;
            return x.FirstName.CompareTo(y.FirstName);
        }

        public bool Equals(User other)
        {
            if (other != null)
            {
                if (ReferenceEquals(this,other))
                    return true;
                return this.FirstName == other.FirstName && this.LastName == other.LastName;

            }
            return false;
        }

        public User Clone()
        {
            return new User()
            {
                Id = Id,
                PersonalId = PersonalId,
                FirstName = FirstName,
                LastName = LastName,
                BirthDate = BirthDate,
                Gender = Gender,
                Visas = Visas
            };
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
