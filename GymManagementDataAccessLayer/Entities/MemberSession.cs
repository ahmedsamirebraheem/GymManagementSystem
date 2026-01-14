using System;
using System.Collections.Generic;
using System.Security.Cryptography.Pkcs;
using System.Text;

namespace GymManagementDataAccessLayer.Entities;

public class MemberSession : BaseEntity
{
    public bool IsAttended { get; set; }
    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;
    public int SessionId { get; set; }
    public Session Session { get; set; } = null!;
    public DateTime BookingDate { get; set; } = DateTime.Now;
}
