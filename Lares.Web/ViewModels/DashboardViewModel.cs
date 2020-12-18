using Lares.Entities;

namespace Lares.ViewModels
{
    public class DashboardViewModel
    {
        // Current User
        public virtual User CurrentUser { get; set; }

        // Currently selected Property
        public virtual Property CurrentProperty { get; set; }

    }
}
