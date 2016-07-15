using System;
using System.ComponentModel.DataAnnotations;

namespace Attributes
{
    // Should be applied to properties and fields.
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class StringValidatorAttribute : ValidationAttribute
    {
        public int MaxLength { get; set; }
        public StringValidatorAttribute(int maxLength)
        {
            MaxLength = maxLength;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;
            string str = (string)value;
            return str.Length <= MaxLength;
        }
    }
}
