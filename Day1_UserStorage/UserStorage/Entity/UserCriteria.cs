using UserStorage.Interfacies;

namespace UserStorage.Entity
{
    /// <summary>
    /// Criteria for searching user
    /// </summary>
    public class UserCriteria: ICriteria<User>
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalId { get; set; }
    }
}
