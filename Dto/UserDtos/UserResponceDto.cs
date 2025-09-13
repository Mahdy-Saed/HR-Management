namespace HR_Carrer.Dto.UserDtos
{
    public class UserResponceDto
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? ImagePath { get; set; }
        public bool? Status { get; set; }
        public RoleResponseDto? Role { get; set; }



    }

    public class RoleResponseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

}
