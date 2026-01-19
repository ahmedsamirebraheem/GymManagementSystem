using GymManagementDataAccessLayer.Entities;
using Mapster;

namespace GymManagementBusinessLayer.Configurations;

public class MappingConfiguration
{
    public static void RegisterMappings()
    {
        // --- 1. Member Mappings ---

        TypeAdapterConfig<Member, GymManagementBusinessLayer.ViewModels.MemberVM.MemberVM>.NewConfig()
            .Map(dest => dest.Gender, src => src.Gender.ToString());

        TypeAdapterConfig<Member, GymManagementBusinessLayer.ViewModels.MemberVM.DetailsVM>.NewConfig()
            .Map(dest => dest.Gender, src => src.Gender.ToString())
            .Map(dest => dest.Address, src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}");

        TypeAdapterConfig<Member, GymManagementBusinessLayer.ViewModels.MemberVM.UpdateVM>.NewConfig()
            .Map(dest => dest.BuildingNumber, src => src.Address.BuildingNumber)
            .Map(dest => dest.Street, src => src.Address.Street)
            .Map(dest => dest.City, src => src.Address.City);

        // Mapping from VM to Entity (Create)
        TypeAdapterConfig<GymManagementBusinessLayer.ViewModels.MemberVM.CreateVM, Member>.NewConfig()
            .Map(dest => dest.Address.BuildingNumber, src => src.BuildingNumber)
            .Map(dest => dest.Address.Street, src => src.Street)
            .Map(dest => dest.Address.City, src => src.City)
            .Ignore(dest => dest.Photo);

        // Mapping from VM to Entity (Update)
        TypeAdapterConfig<GymManagementBusinessLayer.ViewModels.MemberVM.UpdateVM, Member>.NewConfig()
            .Map(dest => dest.Address.BuildingNumber, src => src.BuildingNumber)
            .Map(dest => dest.Address.Street, src => src.Street)
            .Map(dest => dest.Address.City, src => src.City)
            .Ignore(dest => dest.Photo)
            .Ignore(dest => dest.Name!);

        // --- 2. Plan Mappings ---

        TypeAdapterConfig<GymManagementBusinessLayer.ViewModels.PlanVM.UpdateVM, Plan>.NewConfig()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.DurationDays, src => src.DurationDays)
            .Map(dest => dest.Price, src => src.Price)
            .IgnoreNonMapped(true);

        // --- 3. Session Mappings ---

        TypeAdapterConfig<Session, GymManagementBusinessLayer.ViewModels.SessionVM.SessionVM>.NewConfig()
            .Map(dest => dest.CategoryName, src => src.Category.Name)
            .Map(dest => dest.TrainerName, src => src.Trainer.Name)
            .Map(dest => dest.AvailableSlot, src => src.Capacity - (src.SessionMembers != null ? src.SessionMembers.Count : 0));

        // --- 4. Trainer Mappings ---

        TypeAdapterConfig<Trainer, GymManagementBusinessLayer.ViewModels.TrainerVM.TrainerVM>.NewConfig()
            .Map(dest => dest.Specialization, src => src.Specialty.ToString())
            .Map(dest => dest.SpecializationId, src => (int)src.Specialty)
            .Map(dest => dest.Gender, src => src.Gender.ToString());

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

        // تصحيح الخطأ اللي كان في اسم الـ UpdateVM للمدرب
        TypeAdapterConfig<Trainer, GymManagementBusinessLayer.ViewModels.TrainerVM.UpdateVM>.NewConfig()
            .Map(dest => dest.Specialization, src => src.Specialty)
            .Map(dest => dest.BuildingNumber, src => src.Address.BuildingNumber)
            .Map(dest => dest.Street, src => src.Address.Street)
            .Map(dest => dest.City, src => src.Address.City);

        // --- 5. MemberSession Mappings ---

        TypeAdapterConfig<MemberSession, GymManagementBusinessLayer.ViewModels.MemberSessionVM.MemberBookingVM>.NewConfig()
            .Map(dest => dest.MemberId, src => src.MemberId)
            .Map(dest => dest.MemberName, src => src.Member.Name)
            .Map(dest => dest.BookingDate, src => src.BookingDate)
            .Map(dest => dest.IsAttended, src => src.IsAttended);
    }
}