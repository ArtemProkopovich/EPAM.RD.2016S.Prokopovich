using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.Entity
{
    [Serializable]
    public class User: IEquatable<User>, ICloneable
    {
        public int Id { get; set; }
        public string PersonalId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender? Gender { get; set; }
        public Visa[] Visas { get; set; }

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

        public override string ToString()
        {
            return "User ID: " + Id + " FirstName: " + FirstName + " LastName: " + LastName + ".";
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
