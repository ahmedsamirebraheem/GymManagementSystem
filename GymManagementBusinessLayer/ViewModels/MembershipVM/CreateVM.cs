using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymManagementBusinessLayer.ViewModels.MembershipVM;

public class CreateVM
{
   
        [Required(ErrorMessage = "Please select a member")]
        [Display(Name = "Member")]
        public int MemberId { get; set; }

        [Required(ErrorMessage = "Please select a subscription plan")]
        [Display(Name = "Plan")]
        public int PlanId { get; set; }
    
}
