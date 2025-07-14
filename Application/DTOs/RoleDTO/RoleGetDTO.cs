namespace Application.DTOs.RoleDTO;

public class RoleGetDTO
{
    public int RoleId { get; set; }

    public string Name { get; set; }

    public int[] PermissionsId { get; set; }
}
