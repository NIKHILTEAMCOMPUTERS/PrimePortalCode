using Microsoft.AspNetCore.Mvc;
using RMS.Data.DTOs;
using RMS.Data.Models;
using RMS.Entity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Service.Interfaces.Timesheets
{
    public interface ITimesheetRepository : IGenericRepository<Timesheet>
    {


        public Task<GenericResponse<Timesheetheader>> AddHeaderAsync(string monthyear, JwtLoginDetailDto userdetails);
        public Task<GenericResponse<TimesheetDTO>> UpsertHeaderDetailItemsAsync(TimeSheetItemDto Value, JwtLoginDetailDto userdetails);
        public Task<GenericResponse<List<Timesheetheader>>> GetHeaderAsync(JwtLoginDetailDto userdetails);
        public Task<GenericResponse<List<TimesheetDTO>>> GetListofTimesheetDetailByHeaderid(int HeaderId);
        public Task<GenericResponse<string>> SubmitTimeSheetDetailItemsAsync(List<int> timesheetDetailIds, JwtLoginDetailDto userdetails);
        public Task<GenericResponse<string>> DeleteTimeSheetDetailItemsAsync(int timesheetDetailId, JwtLoginDetailDto userdetails);
        public Task<GenericResponse<List<object>>> GetTimesheetByManagerId(string monthYear, int projectId, int employeeId, JwtLoginDetailDto userdetails);
        public Task<List<Project>> GetProjectsWithDetailsByTimesheet(int Employeeid);
        public Task<GenericResponse<Timesheetdetail>> Timesheetdecline(List<TimeSheetItemDto> obj, JwtLoginDetailDto userdetails);
        public Task<GenericResponse<List<Categoryofactivity>>> Getcategoryofactivity();
        public Task<IEnumerable<TimesheetHRExcelDto>> GetTdeTimesheetCount(string monthYear);

    }
}
