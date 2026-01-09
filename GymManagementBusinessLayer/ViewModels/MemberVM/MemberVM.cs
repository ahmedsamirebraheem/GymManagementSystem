using GymManagementDataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.ViewModels.MemberVM;

public class MemberVM
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Photo { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string  Gender { get; set; } = null!;

}
