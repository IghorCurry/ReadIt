using Microsoft.EntityFrameworkCore;
using ReadIt.DAL.Persistance.Settings;

namespace ReadIt.DAL.Persistance.Seeds;

internal static partial class DataSeed
{
    public static void AddTestableData(this ModelBuilder modelBuilder, DefaultAdminSettings defaultAdminSettings)
    {
        modelBuilder.AddUserManagementSeed(defaultAdminSettings);
    }
}
