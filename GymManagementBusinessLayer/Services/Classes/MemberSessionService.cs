using GymManagementBusinessLayer.Services.Interfaces;
using GymManagementBusinessLayer.ViewModels.MemberSessionVM;
using GymManagementDataAccessLayer.Entities;
using GymManagementDataAccessLayer.Repositories.Interfaces;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.Services.Classes;

public class MemberSessionService(IUnitOfWork unitOfWork) : IMemberSessionService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<MemberSessionIndexVM> GetAllAsync()
    {
        // جلب الحصص التي لم يتم حذفها فقط
        var sessions = await _unitOfWork.Sessions.GetAllAsync(
            filter: s => s.IsDeleted == false,
            includeProperties: "Category,Trainer,SessionMembers"
        );

        var now = DateTime.Now;

        // الفلترة بناءً على الوقت
        var upcomingList = sessions.Where(s => s.StartDate > now).ToList();
        var ongoingList = sessions.Where(s => s.StartDate <= now && s.EndDate >= now).ToList();

        return new MemberSessionIndexVM
        {
            UpcomingSessions = upcomingList.Adapt<IEnumerable<MemberSessionVM>>(),
            OngoingSessions = ongoingList.Adapt<IEnumerable<MemberSessionVM>>()
        };
    }
    public async Task<SessionMembersVM> GetAsync(int sessionId)
    {
        var session = await _unitOfWork.Sessions.GetAsync(
            filter: s => s.Id == sessionId,
            includeProperties: "Category,SessionMembers.Member"
        );

        if (session == null) return null!;

        // المابنج هنا هيتم أوتوماتيكياً للـ StartDate لأن الاسم متطابق في الـ Entity والـ VM
        var result = session.Adapt<SessionMembersVM>();

        // تأكد إن الـ SessionId متسكن صح
        result.SessionId = session.Id;

        return result;
    }

    public async Task<bool> CancelBookingAsync(int sessionId, int memberId)
    {
        // 1. البحث عن السجل في جدول الربط
        var booking = await _unitOfWork.MemberSessions.GetAsync(
            filter: ms => ms.SessionId == sessionId && ms.MemberId == memberId
        );

        if (booking == null) return false;

        // 2. حذف السجل
        _unitOfWork.MemberSessions.Delete(booking);

        // 3. تحديث الـ Capacity (اختياري لو عندك خاصية EnrolledCount)
        // لو الـ EnrolledCount بيتحسب ديناميكي بالـ Count مش هتحتاج الخطوة دي
        // لكن لو بتطرح من الـ MaxCapacity يدوي يبقى لازم تحدثها هنا

        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<CreateMemberSessionVM> PrepareCreateViewModelAsync(int sessionId)
    {
        // 1. هات كل الحجوزات للسيشن دي بدون أي فلاتر مسبقة
        var allSessions = await _unitOfWork.MemberSessions.GetAllAsync();

        // فلتر يدوي هنا بالـ ID عشان نضمن إننا ماسكينهم
        var enrolledIds = allSessions
            .Where(ms => ms.SessionId == sessionId)
            .Select(ms => ms.MemberId)
            .ToList();

        // 2. هات كل الأعضاء
        var allMembers = await _unitOfWork.Members.GetAllAsync();

        // الفلتر النهائي يدوي تماماً بعيداً عن الـ Repository logic
        var availableMembers = allMembers
            .Where(m => !enrolledIds.Contains(m.Id) && m.IsDeleted == false)
            .ToList();

        return new CreateMemberSessionVM
        {
            SessionId = sessionId,
            MembersList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(availableMembers, "Id", "Name")
        };
    }
    public async Task<bool> CreateAsync(CreateMemberSessionVM model)
    {
        try
        {
            // 1. البحث عن السجل مع تجاهل الفلتر (IgnoreQueryFilters)
            // ملاحظة: لو الـ Repository بتاعك مافيهوش IgnoreQueryFilters
            // لازم تضيف ميثود في الـ Repository أو الـ UnitOfWork بتدعم ده.

            var existingBooking = await _unitOfWork.MemberSessions.GetWithDeletedAsync(ms =>
                ms.SessionId == model.SessionId && ms.MemberId == model.SelectedMemberId);

            if (existingBooking != null)
            {
                // 2. لو لقاه ممسوح (Soft Deleted).. رجعه حي
                if (existingBooking.IsDeleted)
                {
                    existingBooking.IsDeleted = false;
                    existingBooking.BookingDate = DateTime.Now;
                    // existingBooking.UpdatedAt = DateTime.Now; // لو عندك الحقل ده

                    await _unitOfWork.SaveChangesAsync();
                    return true;
                }

                // لو موجود ومش ممسوح
                return false;
            }

            // 3. لو مش موجود خالص.. ضيف جديد
            var newBooking = new MemberSession
            {
                SessionId = model.SessionId,
                MemberId = model.SelectedMemberId,
                BookingDate = DateTime.Now,
                IsDeleted = false
            };

            await _unitOfWork.MemberSessions.AddAsync(newBooking);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public async Task<bool> ToggleAttendanceAsync(int sessionId, int memberId)
    {
        var booking = await _unitOfWork.MemberSessions.GetAsync(
            filter: ms => ms.SessionId == sessionId && ms.MemberId == memberId
        );

        if (booking == null) return false;

        // عكس الحالة الحالية
        booking.IsAttended = !booking.IsAttended;

        await _unitOfWork.SaveChangesAsync();
        return booking.IsAttended; // بنرجع الحالة الجديدة عشان نحدث الـ UI
    }

    
}
