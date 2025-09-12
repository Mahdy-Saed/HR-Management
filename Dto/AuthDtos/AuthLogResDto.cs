namespace HR_Carrer.Dto.AuthDtos
{
    public class AuthLogResDto
    {
        public Guid Id { get; set; }
        public string? AccessToken { get; set; }

        public string? RefreshToken { get; set; }



    }
}
