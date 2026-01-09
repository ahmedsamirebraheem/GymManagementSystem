using GymManagementDataAccessLayer.Data.Context;
using GymManagementDataAccessLayer.Entities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GymManagementDataAccessLayer.Data.DataSeeding;

public class Seeder
{
    // عملنا Cache للـ Options عشان الأداء (Performance)
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public static bool SeedData(GymDBContext dbContext, string webRootPath)
    {
        try
        {
            // التأكد من أن الجداول فاضية قبل البدء
            bool hasCategories = dbContext.Categories.Any();
            bool hasPlans = dbContext.Plans.Any();

            if (hasCategories && hasPlans) return false;

            if (!hasCategories)
            {
                var categories = LoadDataFromJsonFile<Category>(webRootPath, "categories.json");
                if (categories.Count != 0) dbContext.Categories.AddRange(categories);
            }

            if (!hasPlans)
            {
                var plans = LoadDataFromJsonFile<Plan>(webRootPath, "plans.json");
                if (plans.Count != 0) dbContext.Plans.AddRange(plans);
            }

            return dbContext.SaveChanges() > 0;
        }
        catch (Exception ex)
        {
            // في حالة الفشل بنطبع السبب في الـ Console
            Console.WriteLine($"[Seeding Error]: {ex.Message}");
            return false;
        }
    }

    private static List<T> LoadDataFromJsonFile<T>(string webRootPath, string fileName)
    {
        // بناء المسار باستخدام الـ webRootPath لضمان الوصول لمجلد wwwroot
        var filePath = Path.Combine(webRootPath, "Files", fileName);

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"[Seeding Warning]: File not found at {filePath}");
            return [];
        }

        try
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<T>>(jsonData, _jsonOptions) ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[JSON Error] in {fileName}: {ex.Message}");
            return [];
        }
    }
}