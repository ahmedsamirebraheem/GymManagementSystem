using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.ViewModels.MemberSessionVM;

public class MemberBookingVM
{
    public int MemberId { get; set; }
    public string MemberName { get; set; } = null!;
    public DateTime BookingDate { get; set; }
    public bool IsAttended { get; set; } // تأكد من وجود هذا السطر
}
