using System;

namespace Attributes
{
    // Should be applied to assembly only.
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple =true)]
    public class InstantiateAdvancedUserAttribute : InstantiateUserAttribute
    {
        public int? ExternalID { get; set; }

        public InstantiateAdvancedUserAttribute(string firstName, string lastName) : base(firstName, lastName)
        {

        }

        public InstantiateAdvancedUserAttribute(int id, string firstName, string lastName, int extId) : base(id, firstName, lastName)
        {
            ExternalID = extId;
        }
    }
}
