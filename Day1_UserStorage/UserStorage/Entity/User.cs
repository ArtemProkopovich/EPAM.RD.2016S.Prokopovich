using System;

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

        /// <summary>
        /// Determenies when this object and parametr object are equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj != null) {
                var user = obj as User;
                return Equals(user);
            }
            return false;
        }

        /// <summary>
        /// Return hashcode of user object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int fNameHash = FirstName != null ? FirstName.GetHashCode() : 0;
            int lNameHash = LastName != null ? LastName.GetHashCode() : 0;
            return fNameHash.GetHashCode() ^ lNameHash.GetHashCode();
        }

        /// <summary>
        /// Determenies when this user and other user are equal.
        /// Equality checked like equality of first and last name.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Return string object of user
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "User ID: " + Id + " FirstName: " + FirstName + " LastName: " + LastName + ".";
        }

        /// <summary>
        /// Return new object of user with same properties
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Return new object of user with same properties.
        /// </summary>
        /// <returns></returns>
        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
