using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GymManagementDataAccessLayer.Entities;

public class Membership : BaseEntity
{
    public DateTime EndDate { get; set; }
    public string Status { get
        {

            return EndDate > DateTime.Now ? "Active" : "Expired";
        }
    }
    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;

    public int PlanId { get; set; }
    public Plan Plan { get; set; } = null!;

}
