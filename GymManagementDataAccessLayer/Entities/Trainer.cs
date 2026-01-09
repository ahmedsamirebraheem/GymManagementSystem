using GymManagementDataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementDataAccessLayer.Entities;

public class Trainer : GymUser
{
    public Specialities Specialty { get; set; }

    public ICollection<Session> Sessions { get; set; } = [];
}
