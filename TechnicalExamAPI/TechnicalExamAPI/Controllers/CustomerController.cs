using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Net;
using TechnicalExamAPI.Data;
using TechnicalExamAPI.Models;

namespace TechnicalExamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private readonly DataContext _context;
        public CustomerController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("AddCustomer")]
        public async Task<ActionResult<List<Customer>>> AddCustomer(Customer customerParam)
        {
            _context.customer.Add(customerParam);
            await _context.SaveChangesAsync();

            return Ok(await _context.customer.ToListAsync());

        }

        [HttpGet("GetAllCustomer")]
        public async Task<ActionResult<List<Customer>>> GetAllCustomer()
        {
            int year = DateTime.Now.Year;
            return Ok(await _context.customer.Select(p => new {
                Id = p.Id,
                FirstName = p.FirstName,
                MiddleName = p.MiddleName,
                LastName = p.LastName,
                FullName = p.FirstName + " " + p.MiddleName + " " + p.LastName,
                DateOfBirth = p.DateOfBirth.ToShortDateString(),
                Age = year - p.DateOfBirth.Year,
                isFilipino = p.isFilipino
            }).ToListAsync());
        }


        [HttpGet("GetCustomerDetail/{id}")]
        public async Task<ActionResult<List<Customer>>> GetCustomerDetail(int id)
        {
            var customer = await _context.customer.FindAsync(id);
            if (customer == null)
            {
                return BadRequest("The customer does not exist.");
            }
            else
            {
                return Ok(customer);
            }
        }

        [HttpPut("UpdateCustomer")]
        public async Task<ActionResult<List<Customer>>> UpdateCustomer(Customer customerParam)
        {
            var customer = await _context.customer.FindAsync(customerParam.Id);
            if (customer == null)
            {
                return BadRequest("The customer does not exist.");
            }

            customer.FirstName = customerParam.FirstName;
            customer.LastName = customerParam.LastName;
            customer.MiddleName = customerParam.MiddleName;
            customer.DateOfBirth = customerParam.DateOfBirth;
            customer.isFilipino = customerParam.isFilipino;

            await _context.SaveChangesAsync();
            return Ok(await _context.customer.ToListAsync());

        }

        [HttpDelete("DeleteCustomer/{id}")]
        public async Task<ActionResult<List<Customer>>> DeleteCustomer(int id)
        {
            var customer = await _context.customer.FindAsync(id);
            if (customer == null)
            {
                return BadRequest("The customer does not exist.");
            }
            else
            {
                _context.customer.Remove(customer);
                await _context.SaveChangesAsync();
                return Ok(await _context.customer.ToListAsync());
            }
        }
    }
}
