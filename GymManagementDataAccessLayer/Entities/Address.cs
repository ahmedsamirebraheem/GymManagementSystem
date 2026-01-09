using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementDataAccessLayer.Entities;

[Owned]
public class Address
{
    public int BuildingNumber { get; set; }
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
   
}
