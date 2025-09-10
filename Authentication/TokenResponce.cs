using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace HR_Carrer.Authentication
{
    public class TokenResponce
    {
        public Guid? User_Id { get; set; }

        public string? Access_Token { get; set; }

        public string? Refresh_Token { get; set; }

    }
}