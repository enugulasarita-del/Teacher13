using System;
using System.Data;

namespace TeacherDashboard
{
    public static class AppData
    {
        public static DataTable LeaveRequests { get; private set; }

        static AppData()
        {
            InitLeaveData();
        }

        private static void InitLeaveData()
        {
            LeaveRequests = new DataTable();
            LeaveRequests.Columns.Add("ReqID");
            LeaveRequests.Columns.Add("FacultyName");
            LeaveRequests.Columns.Add("Department");
            LeaveRequests.Columns.Add("Type");
            LeaveRequests.Columns.Add("StartDate", typeof(DateTime));
            LeaveRequests.Columns.Add("EndDate", typeof(DateTime));
            LeaveRequests.Columns.Add("Days", typeof(int));
            LeaveRequests.Columns.Add("Reason");
            LeaveRequests.Columns.Add("Status"); // Pending, Approved, Rejected

            // Seed Data
            LeaveRequests.Rows.Add("REQ-101", "Dr. Rajesh Kumar", "CSE", "Sick Leave", DateTime.Parse("2026-02-01"), DateTime.Parse("2026-02-02"), 2, "High Fever", "Approved");
            LeaveRequests.Rows.Add("REQ-102", "Prof. Anita Sharma", "IT", "Casual Leave", DateTime.Parse("2026-02-10"), DateTime.Parse("2026-02-10"), 1, "Personal Work", "Pending");
            LeaveRequests.Rows.Add("REQ-103", "Mr. Amit Verma", "Mech", "Duty Leave", DateTime.Parse("2026-02-12"), DateTime.Parse("2026-02-12"), 1, "Exam Duty", "Pending");
            LeaveRequests.Rows.Add("REQ-104", "Ms. Priya Singh", "BMS", "Earned Leave", DateTime.Parse("2026-03-01"), DateTime.Parse("2026-03-05"), 5, "Family Vacation", "Pending");
        }
    }
}
