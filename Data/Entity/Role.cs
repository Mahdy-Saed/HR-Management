using System.Text.Json.Serialization;

namespace HR_Carrer.Data.Entity
{
    public class Role
    {

        public int Id { get; set; }


        public string? Name { get; set; }

        // Navigation Relationship  


        [JsonIgnore]
        public ICollection <User> Users { get; set; }  // many-to-one relationship

    }
}
