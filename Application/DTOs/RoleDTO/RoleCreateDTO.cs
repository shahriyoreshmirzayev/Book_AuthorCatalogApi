namespace Application.DTOs.RoleDTO;

public class RoleCreateDTO
{
    public string Name { get; set; }

    public int[] PermissionsId { get; set; }
}
