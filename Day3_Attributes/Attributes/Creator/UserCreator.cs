using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Attributes.Creator
{
    public class UserCreator
    {
        public IEnumerable<AdvancedUser> CreateAdvancedUserFromAssembly()
        {
            List<AdvancedUser> result = new List<AdvancedUser>();
            var assembly = Assembly.GetExecutingAssembly();
            var type = typeof(AdvancedUser);
            var attributes = (IEnumerable<InstantiateAdvancedUserAttribute>)assembly.GetCustomAttributes(typeof(InstantiateAdvancedUserAttribute));
            foreach(var attr in attributes)
            {
                if (attr.ID == null)
                    attr.ID = GetIdFromAttributes(type, "id");
                if (attr.ExternalID == null)
                    attr.ExternalID = GetIdFromAttributes(type, "externalId");
                result.Add(new AdvancedUser((int)attr.ID, (int)attr.ExternalID) { FirstName = attr.FirstName, LastName = attr.LastName });
            }
            return result;
        }

        public IEnumerable<User> CreateUserFromClass()
        {
            List<User> result = new List<User>();
            var type = typeof(User);
            var attributes = (IEnumerable<InstantiateUserAttribute>)type.
                GetCustomAttributes(typeof(InstantiateUserAttribute));
            foreach (var attr in attributes)
            {
                if (attr.ID == null)
                    attr.ID = GetIdFromAttributes(type, "id");
                result.Add(new User((int)attr.ID) { FirstName = attr.FirstName, LastName = attr.LastName });
            }
            return result;
        }

        public bool ValidateUser(User user, out List<ValidationResult> results)
        {
            bool result = true;
            results = new List<ValidationResult>();
            var type = user.GetType();
            var props = type.GetProperties().Where(e => e.GetCustomAttributes(typeof(ValidationAttribute)).Count() != 0);
            var fields = type.GetFields(BindingFlags.NonPublic|BindingFlags.Instance).Where(e => e.GetCustomAttributes(typeof(ValidationAttribute)).Count() != 0);
            foreach (var prop in props)
            {
                result = result && ValidateProperty(user, prop, ref results);
            }
            foreach (var field in fields)
            {
                result = result && ValidateField(user, field, ref results);
            }
            return result;
        }

        private int x = 0;

        private bool ValidateProperty(User user, PropertyInfo info, ref List<ValidationResult> results)
        {
            object value = info.GetValue(user);
            var context = new ValidationContext(value, null, null);
            var tempResults = new List<ValidationResult>();
            var attributes = (IEnumerable<ValidationAttribute>)info.GetCustomAttributes(typeof(ValidationAttribute));

            if (!Validator.TryValidateValue(value, context, results, attributes))
            {
                results.AddRange(tempResults);
                return false;
            }
            else
            {
                return true;
            }

        }

        private bool ValidateField(User user, FieldInfo info, ref List<ValidationResult> results)
        {
            object value = info.GetValue(user);
            var context = new ValidationContext(value, null, null);
            var tempResults = new List<ValidationResult>();
            var attributes = (IEnumerable<ValidationAttribute>)info.GetCustomAttributes(typeof(ValidationAttribute));

            if (!Validator.TryValidateValue(value, context, results, attributes))
            {
                results.AddRange(tempResults);
                return false;
            }
            else
            {
                return true;
            }
        }

        public int GetIdFromAttributes(Type type, string fieldName)
        {
            var ctors = type.GetConstructors();
            var attr = (MatchParameterWithPropertyAttribute)ctors.
                FirstOrDefault(e => e.GetCustomAttributes(typeof(MatchParameterWithPropertyAttribute)) != null).
                GetCustomAttributes(typeof(MatchParameterWithPropertyAttribute)).
                FirstOrDefault(e => ((MatchParameterWithPropertyAttribute)e).FieldName == fieldName);
            int value = (int)((DefaultValueAttribute)(type.GetProperty(attr.PropertyName).
                GetCustomAttribute(typeof(DefaultValueAttribute)))).Value;
            return value;
        } 
    }
}
