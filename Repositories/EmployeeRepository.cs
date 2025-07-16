using System.Globalization;
using System.IO.Compression;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Ordering_App.Context;
using Ordering_App.Interfaces;
using Ordering_App.Models;
using Ordering_App.Models.Domain;
using Ordering_App.Models.DTOs;

namespace Ordering_App.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public EmployeeRepository(DataContext dataContext, IMapper mapper)
        {

            this.dataContext = dataContext;
            this.mapper = mapper;
        }
        public async Task<Employee> CreateAsync(AddEmployeeDTO employeeDTO)
        {
            var employeeDM = mapper.Map<Employee>(employeeDTO);
            await dataContext.Employees.AddAsync(employeeDM);
            return employeeDM;
        }

        public Task<Employee> DeleteAsync(Employee employee)
        {

            throw new NotImplementedException();
        }

        public async Task<PagedList<Employee>> GetAllAsync(UserParams userParams)
        {
            var query = dataContext.Employees.AsQueryable();
            //Balance filtering
            if (userParams.MinBalance > 0) query = query.Where(x => x.Balance >= userParams.MinBalance);
            if (userParams.MaxBalance < decimal.MaxValue) query = query.Where(x => x.Balance <= userParams.MaxBalance);
            //Last deposit month
            if (!userParams.LastDepositMonth.IsNullOrEmpty())
            {
                query = query.Where(x => x.LastDepositMonth == userParams.LastDepositMonth);
            }
            // query = userParams.OrderBy switch
            // {
            //     "created" => query.OrderByDescending(x => x.CreatedAt),
            //     _ => query.OrderByDescending(x => x.LastActive)
            // };

            return await PagedList<Employee>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await dataContext.Employees.FindAsync(id);
        }

        public async Task<Employee> UpdateAsync(UpdateEmployeeDTO updateEmployeeDTO)
        {
            var employeeDM = await GetByEmpoyeeNoAsync(updateEmployeeDTO.EmployeeNumber);
            employeeDM.Balance += updateEmployeeDTO.Balance;
            employeeDM.LastDepositMonth = updateEmployeeDTO.LastDepositMonth;
            dataContext.Employees.Update(employeeDM);
            return employeeDM;
        }
        public async Task<bool> UserExistsAsync(string name)
        {
            return await dataContext.Employees.AnyAsync(x => x.Name.ToLower() == name.ToLower());
        }
        public async Task<bool> SaveAllAsync()
        {
            return await dataContext.SaveChangesAsync() > 0;
        }

        public async Task<string> ConvertMonthAsync(string input)
        {
            await Task.Delay(0);
            DateTime parsedDate = DateTime.ParseExact(input, "yyyy-MM", CultureInfo.InvariantCulture);
            return parsedDate.ToString("MMMM yyyy"); 
        }

        public async Task<Employee?> GetByEmpoyeeNoAsync(Guid employeeNo)
        {
            return await dataContext.Employees.FirstOrDefaultAsync(x => x.EmployeeNumber == employeeNo);
        }
    }
}
