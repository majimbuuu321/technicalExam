using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnicalExamAPI.Data;
using TechnicalExamAPI.Models;

namespace TechnicalExamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DataContext _context;
        public AccountController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("AddAccount")]
        public async Task<ActionResult<List<Account>>> AddAccount(Account accountParams)
        {
            Customer customer = new Customer();
            var resultCustomer = await _context.customer.FindAsync(customer.Id);
            if (resultCustomer == null)
            {
                return BadRequest("The customer does not exist.");
            }

            _context.account.Add(accountParams);
            await _context.SaveChangesAsync();

            return Ok(await _context.account.ToListAsync());

        }

        [HttpGet("GetAllAccount")]
        public async Task<ActionResult<List<Account>>> GetAllAccount()
        {
            //var resultData = (from acc in _context.account
            //                  join cust in _context.customer on acc.CustomerId equals cust.Id
            //                  select new
            //                  {
            //                      AccountId = acc.Id,
            //                      CustomerId = cust.Id,
            //                      //FullName = cust.FirstName + ' ' + cust.MiddleName + ' ' + cust.LastName,
            //                      AccountNumber = acc.AccountNumber,
            //                      AccountType = acc.AccountType,
            //                      BranchAddress = acc.BranchAddress,
            //                      InitialDeposit = acc.InitialDeposit
            //                  });

            //return Ok(resultData.ToListAsync());

            return Ok(await _context.account.Select(p => new {
                Id = p.Id,
                CustomerId = p.CustomerId,
                AccountNumber = p.AccountNumber,
                AccountType = p.AccountType,
                BranchAddress = p.BranchAddress,
                InitialDeposit = p.InitialDeposit
            }).ToListAsync());
        }

        [HttpGet("GetAllCustomerAccount")]
        public async Task<ActionResult<List<Account>>> GetAllCustomerAccount()
        {
            int year = DateTime.Now.Year;
            var resultData = (from acc in _context.account
                              join cust in _context.customer on acc.CustomerId equals cust.Id
                              select new
                              {
                                  CustomerId = cust.Id,
                                  FirstName = cust.FirstName,
                                  MiddleName = cust.MiddleName,
                                  LastName = cust.LastName,
                                  DateOfBirth = cust.DateOfBirth.ToShortDateString(),
                                  Age = year - cust.DateOfBirth.Year,
                                  isFilipino = cust.isFilipino,
                                  AccountId = acc.Id,
                                  AccountNumber = acc.AccountNumber,
                                  AccountType = acc.AccountType,
                                  BranchAddress = acc.BranchAddress,
                                  InitialDeposit = acc.InitialDeposit
                              });

            return Ok(resultData.ToListAsync());
        }

        [HttpPut("UpdateAccount")]
        public async Task<ActionResult<List<Account>>> UpdateAccount(Account accountParam)
        {
            var account = await _context.account.FindAsync(accountParam.Id);
            if (account == null)
            {
                return BadRequest("The account does not exist.");
            }
            account.Id = accountParam.Id;
            account.CustomerId = accountParam.CustomerId;
            account.AccountNumber = accountParam.AccountNumber;
            account.AccountType = accountParam.AccountType;
            account.BranchAddress = accountParam.BranchAddress;

            await _context.SaveChangesAsync();
            return Ok(await _context.account.ToListAsync());
            //return Ok(await _context.account.Select(p => new {
            //    Id = p.Id,
            //    CustomerId = p.CustomerId,
            //    AccountNumber = p.AccountNumber,
            //    AccountType = p.AccountType,
            //    BranchAddress = p.BranchAddress
            //}).ToListAsync());
        }

        [HttpDelete("DeleteAccount/{id}")]
        public async Task<ActionResult<List<Account>>> DeleteAccount(int id)
        {
            var account = await _context.account.FindAsync(id);
            if (account == null)
            {
                return BadRequest("The account does not exist.");
            }
            else
            {
                _context.account.Remove(account);
                await _context.SaveChangesAsync();
                return Ok(await _context.account.ToListAsync());
            }
        }








    }
}
