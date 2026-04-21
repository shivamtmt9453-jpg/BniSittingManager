using System;

namespace BniSittingManager.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        // OPD
        public int TotalGraduateProgram { get; set; }
        public int TotalUndergradProgram { get; set; }
        public int TotalTestPrepProgram { get; set; }
        public int TotalJoinSignalInteractive { get; set; }
        public int TotalJoinAtlasInteractive { get; set; }
        public int TotalJoinForgeSchool { get; set; }
        public int TotalAdvisoryConsultation { get; set; }
        public int TotalAfricanSchools { get; set; }
        public int TodaysGlobalSchools { get; set; }
        public int TotalCollaborationContribution { get; set; }
        public int TotalGeneralCollaboration { get; set; }
        public int TotalContactChief { get; set; }
        public int TotalContactUsEnquiry { get; set; }
        public int TotalStudentInteractive { get; set; }

         

        // User
        public string? UserRole { get; set; }
    }
}
