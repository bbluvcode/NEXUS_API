using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEXUS_API.Data;
using NEXUS_API.DTOs;
using NEXUS_API.Models;

namespace NEXUS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public AccountController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        //get next customer sequence
        public async Task<int> GetNextCustomerSequence(string type, string regionCode)
        {
            var lastAccount = await _dbContext.Accounts
                .Where(a => a.Type == type && a.AccountId.StartsWith($"{type}-{regionCode}-"))
                .OrderByDescending(a => a.AccountId)
                .FirstOrDefaultAsync();
            if (lastAccount == null)
            {
                return 1;
            }
            var lastSequence = int.Parse(lastAccount.AccountId.Split('-')[2]);
            return lastSequence + 1;
        }
        //generate AccountId
        public string GenerateAccountId(string type, string regionCode, int customerSequence)
        {
            return $"{type}-{regionCode}-{customerSequence:D7}";
        }
        //Create Account
        [HttpPost]
        public async Task<string> CreateAccount(AccountDTO accountDTO)
        {
            int customerSequence = await GetNextCustomerSequence(accountDTO.Type, accountDTO.RegionCode);
            string accountId = GenerateAccountId(accountDTO.Type, accountDTO.RegionCode, customerSequence);
            var account = new Account
            {
                AccountId = accountId,
                CustomerId = accountDTO.CustomerId,
                Type = accountDTO.Type,
            };
            _dbContext.Accounts.Add(account);
            await _dbContext.SaveChangesAsync();
            return accountId;
        }
    }
}