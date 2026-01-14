using GymManagementBusinessLayer.ViewModels.MemberSessionVM;
using GymManagementBusinessLayer.ViewModels.MemberVM;
using GymManagementBusinessLayer.ViewModels.SessionVM;
using GymManagementBusinessLayer.ViewModels.TrainerVM;
using GymManagementDataAccessLayer.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;


namespace GymManagementBusinessLayer.Configurations;

public class MappingConfiguration
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<Member, MemberVM>.NewConfig()
            .Map(dest => dest.Gender, src => src.Gender.ToString());

        TypeAdapterConfig<Member, GymManagementBusinessLayer.ViewModels.MemberVM.DetailsVM>.NewConfig()
            .Map(dest => dest.Gender, src => src.Gender.ToString())
            .Map(dest => dest.Address, src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}");

        TypeAdapterConfig<GymManagementBusinessLayer.ViewModels.MemberVM.UpdateVM, Member>.NewConfig()
             .Map(dest => dest.Address.BuildingNumber, src => src.BuildingNumber)
            .Map(dest => dest.Address.Street, src => src.Street)
            .Map(dest => dest.Address.City, src => src.City)
            .Ignore(dest => dest.Name!)
            .Ignore(dest => dest.Photo!);

        TypeAdapterConfig<GymManagementBusinessLayer.ViewModels.MemberVM.CreateVM, Member>.NewConfig()
            .Map(dest => dest.Address.BuildingNumber, src => src.BuildingNumber)
            .Map(dest => dest.Address.Street, src => src.Street)
            .Map(dest => dest.Address.City, src => src.City);




        TypeAdapterConfig<Member, MemberVM>.NewConfig()
            .Map(dest => dest.Gender, src => src.Gender.ToString());

        TypeAdapterConfig<Member, GymManagementBusinessLayer.ViewModels.MemberVM.UpdateVM>.NewConfig()
    .Map(dest => dest.BuildingNumber, src => src.Address.BuildingNumber)
    .Map(dest => dest.Street, src => src.Address.Street)
    .Map(dest => dest.City, src => src.Address.City);

        TypeAdapterConfig<GymManagementBusinessLayer.ViewModels.PlanVM.UpdateVM, Plan>.NewConfig()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.DurationDays, src => src.DurationDays)
            .Map(dest => dest.Price, src => src.Price)
            .IgnoreNonMapped(true);

        TypeAdapterConfig<Session, SessionVM>.NewConfig()
    .Map(dest => dest.CategoryName, src => src.Category.Name)
    .Map(dest => dest.TrainerName, src => src.Trainer.Name)
    .Map(dest => dest.AvailableSlot, src => src.Capacity - (src.SessionMembers != null ? src.SessionMembers.Count : 0));
        // --- Trainer Mappings ---

        TypeAdapterConfig<GymManagementBusinessLayer.ViewModels.TrainerVM.CreateVM, Trainer>.NewConfig()
            .Map(dest => dest.Address.BuildingNumber, src => src.BuildingNumber)
            .Map(dest => dest.Address.Street, src => src.Street)
            .Map(dest => dest.Address.City, src => src.City)
            .Map(dest => dest.Specialty, src => src.Specialization);

        TypeAdapterConfig<GymManagementBusinessLayer.ViewModels.TrainerVM.UpdateVM, Trainer>.NewConfig()
            .Map(dest => dest.Address.BuildingNumber, src => src.BuildingNumber)
            .Map(dest => dest.Address.Street, src => src.Street)
            .Map(dest => dest.Address.City, src => src.City)
            .Map(dest => dest.Specialty, src => src.Specialization)
            .IgnoreNullValues(true);

        TypeAdapterConfig<Trainer, GymManagementBusinessLayer.ViewModels.TrainerVM.UpdateVM>.NewConfig()
            .Map(dest => dest.Specialization, src => src.Specialty)
            .Map(dest => dest.BuildingNumber, src => src.Address.BuildingNumber)
            .Map(dest => dest.Street, src => src.Address.Street)
            .Map(dest => dest.City, src => src.Address.City);

        TypeAdapterConfig<Trainer, TrainerVM>.NewConfig()
            .Map(dest => dest.Specialization, src => src.Specialty.ToString())
            .Map(dest => dest.SpecializationId, src => (int)src.Specialty)
            .Map(dest => dest.Gender, src => src.Gender.ToString());

        TypeAdapterConfig<Trainer, GymManagementBusinessLayer.ViewModels.TrainerVM.DetailsVM>.NewConfig()
            .Map(dest => dest.Speciality, src => src.Specialty.ToString())
            .Map(dest => dest.DateOfBirth, src => src.DateOfBirth.ToString("yyyy-MM-dd"))
            .Map(dest => dest.Address, src => $"{src.Address.BuildingNumber} {src.Address.Street}, {src.Address.City}");

        TypeAdapterConfig<Session, MemberSessionVM>.NewConfig()
     .Map(dest => dest.SessionId, src => src.Id)
     .Map(dest => dest.CategoryName, src => src.Category != null ? src.Category.Name : "No Category")
     .Map(dest => dest.TrainerName, src => src.Trainer != null ? src.Trainer.Name : "Trainer Not Assigned")
     .Map(dest => dest.MaxCapacity, src => src.Capacity)
     .Map(dest => dest.EnrolledCount, src => src.SessionMembers != null ? src.SessionMembers.Count : 0)
     .Map(dest => dest.StartDate, src => src.StartDate)
     .Map(dest => dest.EndDate, src => src.EndDate);

        TypeAdapterConfig<Session, SessionMembersVM>.NewConfig()
    .Map(dest => dest.SessionId, src => src.Id)
    .Map(dest => dest.CategoryName, src => src.Category.Name)
    .Map(dest => dest.Members, src => src.SessionMembers); // Mapster هيتعامل مع القائمة تلقائياً لو الأسماء متوافقة

        TypeAdapterConfig<MemberSession, MemberBookingVM>.NewConfig()
            .Map(dest => dest.MemberId, src => src.MemberId)
            .Map(dest => dest.MemberName, src => src.Member.Name)
            .Map(dest => dest.BookingDate, src => src.BookingDate)
        .Map(dest => dest.IsAttended, src => src.IsAttended);

        TypeAdapterConfig<Trainer, TrainerSelectVM>.NewConfig()
    .Map(dest => dest.SpecializationId, src => (int)src.Specialty);

    }
}
