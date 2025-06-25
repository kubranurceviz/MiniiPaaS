// MiniiPaaS.Application/Models/RoleDto.cs
namespace MiniiPaaS.Application.Models
{
    public static class RoleDto
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string CompanyAdmin = "CompanyAdmin";
        public const string CompanyUser = "CompanyUser";

        public static List<string> GetAll() => new() { SuperAdmin, CompanyAdmin, CompanyUser };
    }
}
