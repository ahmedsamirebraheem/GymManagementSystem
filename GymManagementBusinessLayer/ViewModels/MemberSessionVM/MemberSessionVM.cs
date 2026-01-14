using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.ViewModels.MemberSessionVM;

public class MemberSessionVM
{
    public int SessionId { get; set; }
    public string CategoryName { get; set; }  = null!;
    public string Description { get; set; }   = null!;
    public string TrainerName { get; set; } = null!;
    public DateTime StartDate { get; set; }    
    public DateTime EndDate { get; set; }      

    public TimeSpan Duration => EndDate - StartDate;

    public int MaxCapacity { get; set; }     

    public int EnrolledCount { get; set; }
}
