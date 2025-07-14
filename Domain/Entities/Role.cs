﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities;

[Table("role")]
public class Role
{
    [Column("role_id")]
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RoleId { get; set; }

    [Column("name")]
    public string Name { get; set; }

    public virtual ICollection<Permission> Permissions { get; set; }

    [JsonIgnore]
    public virtual ICollection<User> Users { get; set; }
}
