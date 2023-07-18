
using FullStack.API.Data;
using FullStack.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace FullStack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesListController : Controller
    {
        private readonly EmployeesSiteDbContext _employeesContext;

        public EmployeesListController(EmployeesSiteDbContext employeesContext)
        {
            _employeesContext = employeesContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            Console.WriteLine("GET EMPLOYEE LIST");

            var employees = await _employeesContext.Employees.ToListAsync();
            return Ok(employees);

        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody]Employee employeeRequest ) 
        {
            Console.WriteLine("ADD EMPLOYEE");
          employeeRequest.Id= Guid.NewGuid();
          await _employeesContext.Employees.AddAsync(employeeRequest);
           await _employeesContext.SaveChangesAsync();
            return Ok(employeeRequest);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetEmployee([FromRoute]Guid id)

        { 
         var employee=
           await _employeesContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
           if(employee == null)
            {
                return NotFound();

            }

           return Ok(employee);
        }


        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> updatemployee([FromRoute] Guid id, Employee updateEmployeeRequest)
        {
          var employee = await  _employeesContext.Employees.FindAsync(id);
            if(employee==null) 
            
            { 
              return NotFound();
            
            }

            employee.name = updateEmployeeRequest.name;
            employee.phone= updateEmployeeRequest.phone;
            employee.Email = updateEmployeeRequest.Email;
            employee.department = updateEmployeeRequest.department;
            employee.salary= updateEmployeeRequest.salary;
            await _employeesContext.SaveChangesAsync();
            return Ok(employee);    
             
        
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> deleteEmployee([FromRoute] Guid id)
        {
            var employee = await _employeesContext.Employees.FindAsync(id);
            if (employee == null)

            {
                return NotFound();

            }

            _employeesContext.Employees.Remove(employee);
            await _employeesContext.SaveChangesAsync();
            return Ok(employee);

        }
    }
}