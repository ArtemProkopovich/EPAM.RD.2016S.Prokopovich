using System;

namespace UserStorage.Entity
{
    [Serializable]
    public struct Visa
    {
        public string Country { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
