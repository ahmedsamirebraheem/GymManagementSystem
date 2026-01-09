using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GymManagementDataAccessLayer.Entities;

public class Category : BaseEntity
{
    [JsonPropertyName("CategoryName")]
    public string Name { get; set; } = null!;
    public ICollection<Session> Sessions { get; set; } = [];
}
