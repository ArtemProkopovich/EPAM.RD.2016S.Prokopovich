using System;

namespace Attributes
{
    // Should be applied to .ctors.
    // Matches a .ctor parameter with property. Needs to get a default value from property field, and use this value for calling .ctor.
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = true)]
    public class MatchParameterWithPropertyAttribute : Attribute
    {
        public string FieldName { get; set; }
        public string PropertyName { get; set; }
        public MatchParameterWithPropertyAttribute(string fieldName, string propertyName)
        {
            FieldName = fieldName;
            PropertyName = propertyName;
        }
    }
}
