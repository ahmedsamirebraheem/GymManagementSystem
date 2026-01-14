using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.ViewModels.MemberSessionVM;

public class CreateMemberSessionVM
{
    public int SessionId { get; set; }
    public string? CategoryName { get; set; }
    public int SelectedMemberId { get; set; } // القيمة المختارة
    public SelectList? MembersList { get; set; } // القائمة المعروضة
}
