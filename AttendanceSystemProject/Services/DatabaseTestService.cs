using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AttendanceSystemProject.Models;

namespace AttendanceSystemProject.Services
{
    public class DatabaseTestService
    {
        public static bool TestDatabaseConnection()
        {
            try
            {
                using (var context = new AttendanceSystemContext())
                {
                    // Test the connection by trying to access the database
                    var connectionTest = context.Database.SqlQuery<int>("SELECT 1").SingleOrDefault();
                    return connectionTest == 1;
                }
            }
            catch (Exception ex)
            {
                // Log error if needed
                System.Diagnostics.Debug.WriteLine("Database connection error: " + ex.Message);
                return false;
            }
        }

        public static void SeedSampleData()
        {
            try
            {
                using (var context = new AttendanceSystemContext())
                {
                    // Check if departments already exist
                    if (!context.Departments.Any())
                    {
                        var departments = new List<Department>
                        {
                            new Department { Name = "Khoa Công nghệ Thông tin", Code = "CNTT", Description = "Khoa đào tạo về công nghệ thông tin", IsActive = true },
                            new Department { Name = "Khoa Kinh tế", Code = "KT", Description = "Khoa đào tạo về kinh tế và quản lý", IsActive = true },
                            new Department { Name = "Khoa Ngoại ngữ", Code = "NN", Description = "Khoa đào tạo về ngoại ngữ", IsActive = true }
                        };

                        context.Departments.AddRange(departments);
                        context.SaveChanges();
                    }

                    // Check if users already exist
                    if (!context.Users.Any())
                    {
                        var users = new List<User>
                        {
                            new User 
                            { 
                                FirstName = "Nguyễn", 
                                LastName = "Văn Admin",
                                Email = "admin@school.edu.vn",
                                Phone = "0123456789",
                                Role = "Admin",
                                IsActive = true,
                                DepartmentId = context.Departments.First().DepartmentId
                            },
                            new User 
                            { 
                                FirstName = "Trần", 
                                LastName = "Thị Giảng viên",
                                Email = "teacher@school.edu.vn",
                                Phone = "0987654321",
                                Role = "Teacher",
                                IsActive = true,
                                DepartmentId = context.Departments.First().DepartmentId
                            }
                        };

                        context.Users.AddRange(users);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error seeding data: " + ex.Message);
                throw;
            }
        }
    }
}