using UserStorage.Interfacies;

namespace UserStorage.Entity
{
    public class UserCriteria: ICriteria<User>
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalId { get; set; }
    }
}
