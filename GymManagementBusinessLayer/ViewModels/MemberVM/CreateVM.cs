using GymManagementDataAccessLayer.Entities.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymManagementBusinessLayer.ViewModels.MemberVM;

public class CreateVM
{
    [Required(ErrorMessage = "Photo Is Required!")]
    [DisplayName("ProfilePhoto")]
    public IFormFile PhotoFile { get; set; } = null!;

    [Required(ErrorMessage = "Name Is Required!")]
    [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessage = "Name Must Be Between 2 and 50 Chars!")]
    [RegularExpression(@"[a-zA-Z\s]*$", ErrorMessage = "Name Can Contain Only Letters And Spaces!")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Email Is Required!")]
    [EmailAddress(ErrorMessage = "Invalid Email!")]

    [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessage = "Email Must Be Between 5 and 100 Chars!")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Phone Is Required!")]
    [Phone(ErrorMessage = "Invalid Phone Number!")]
    [RegularExpression(@"^(01)[0-2|5]\d{8}$", ErrorMessage = "Phone Number Must Be 11 digits starting with 01!")]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; } = null!;

    [Required(ErrorMessage = "Date Of Birth Is Required!")]
    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; } = default;

    [Required(ErrorMessage = "Gender Is Required!")]
    public Gender Gender { get; set; }

    [Required(ErrorMessage = "Building Number Is Required!")]
    [Range(minimum: 1, maximum: 9000, ErrorMessage = "Building Number Must Be Between 1 and 9000!")]
    public int BuildingNumber { get; set; } = default;

    [Required(ErrorMessage = "Street Is Required!")]
    [StringLength(maximumLength: 30, MinimumLength = 2, ErrorMessage = "Street Must Be Between 2 and 30 Chars!")]
    public string Street { get; set; } = null!;

    [Required(ErrorMessage = "City Is Required!")]
    [StringLength(maximumLength: 30, MinimumLength = 2, ErrorMessage = "City Must Be Between 2 and 30 Chars!")]
    [RegularExpression(@"[a-zA-Z\s]*$", ErrorMessage = "City Can Contain Only Letters And Spaces!")]
    public string City { get; set; } = null!;

    [Required(ErrorMessage = "Health Record Is Required!")]
    public HealthRecordVM HealthRecord { get; set; } = null!;
}
