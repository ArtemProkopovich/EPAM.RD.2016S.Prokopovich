using System;
using System.ComponentModel.DataAnnotations;

namespace Attributes
{
    // Should be applied to properties and fields.
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class IntValidatorAttribute : ValidationAttribute
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        public IntValidatorAttribute(int minValue, int maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public override bool IsValid(object value)
        {
            int val = (int)value;
            return val >= MinValue && val <= MaxValue;
        }
    }
}
