using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GymManagementDataAccessLayer.Entities;

public class Session : BaseEntity
{
    public string Description { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Capacity { get; set; }

    [ForeignKey("Category")]
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    [ForeignKey("Trainer")]
    public int TrainerId { get; set; }
    public Trainer Trainer { get; set; } = null!;

    public ICollection<MemberSession> SessionMembers { get; set; } = [];
}
