﻿using System.ComponentModel;
using System;
namespace Attributes
{
    [InstantiateUser("Alexander", "Alexandrov")]
    [InstantiateUser(2, "Semen", "Semenov")]
    [InstantiateUser(3, "Petr", "Petrov")]
    public class User: IEquatable<User>
    {
        [IntValidator(1, 1000)]
        private int _id;

        [DefaultValue(1)]
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        [StringValidator(30)]
        public string FirstName { get; set; }

        [StringValidator(20)]
        public string LastName { get; set; }

        [MatchParameterWithProperty("id", "Id")]
        public User(int id)
        {
            _id = id;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var castObj = obj as User;
            return castObj != null ? Equals(castObj) : false;
        }

        public bool Equals(User other)
        {
            if (other == null)
                return false;
            return this.Id == other.Id && this.FirstName == other.FirstName && this.LastName == other.LastName;
        }
    }
}
