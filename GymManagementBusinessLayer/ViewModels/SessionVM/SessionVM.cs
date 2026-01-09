using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.ViewModels.SessionVM;

public class SessionVM
{
    public int Id { get; set; }
    public string CategoryName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string TrainerName { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Capacity { get; set; }
    public int AvailableSlot { get; set; }
    public string DisplayDate => $"{StartDate: MMM dd,yyyy} - {EndDate: MMM dd,yyyy}";
    public string TimeRange => $"{StartDate: hh:mm tt} - {EndDate: hh:mm tt}";
    public TimeSpan Duration => EndDate - StartDate;
    public string Status
    {
        get
        {
            if(StartDate>DateTime.Now)
            {
                return "Upcoming";
            }
            else if(EndDate < DateTime.Now)
            {
                return "Completed";
            }
            else
            {
                return "Ongoing";
            }
        }
    }
}
