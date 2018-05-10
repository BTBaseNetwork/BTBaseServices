using System;
using System.Linq;
using BahamutCommon.Utils;
using BTBaseServices.DAL;
using BTBaseServices.Models;

namespace BTBaseServices.Services
{
    public class AccountService
    {

        public BTAccount CreateNewAccount(BTBaseDbContext dbContext, BTAccount newAccount)
        {
            newAccount.PswHash = PasswordHash.PasswordHash.CreateHash(newAccount.Password);
            newAccount.AccountTypes = BTAccount.ACCOUNT_TYPE_GAME_PLAYER.ToString();
            newAccount.SignDateTs = DateTimeUtil.UnixTimeSpanSec;
            var res = dbContext.BTAccount.Add(newAccount).Entity;
            dbContext.SaveChanges();
            return res;
        }

        public bool UpdatePassword(BTBaseDbContext dbContext, string accountId, string originPassword, string newPassword)
        {
            var account = dbContext.BTAccount.Find(long.Parse(accountId));
            if (account != null && account.Password == originPassword)
            {
                account.Password = newPassword;
                dbContext.BTAccount.Update(account);
                return true;
            }
            return false;
        }

        public bool UpdateNick(BTBaseDbContext dbContext, string accountId, string newNick)
        {
            var account = dbContext.BTAccount.Find(long.Parse(accountId));
            if (account != null)
            {
                account.Nick = newNick;
                dbContext.BTAccount.Update(account);
                return true;
            }
            return false;
        }

        public bool IsUsernameAvaiable(BTBaseDbContext dbContext, string username)
        {
            return dbContext.BTAccount.Count(x => x.UserName == username) == 0;
        }

        public BTAccount GetProfile(BTBaseDbContext dbContext, string accountId)
        {
            return dbContext.BTAccount.Find(long.Parse(accountId));
        }

        public BTAccount ValidateProfile(BTBaseDbContext dbContext, string userstring, string password)
        {
            BTAccount account = null;
            if (account == null && CommonRegexTestUtil.TestPattern(userstring, CommonRegexTestUtil.PATTERN_ACOUNT_ID))
            {
                try { account = dbContext.BTAccount.First(a => a.Password == password && a.AccountId == userstring); } catch (System.Exception) { }
            }

            if (account == null && CommonRegexTestUtil.TestPattern(userstring, CommonRegexTestUtil.PATTERN_EMAIL))
            {
                try { account = dbContext.BTAccount.First(a => a.Password == password && a.Email == userstring); } catch (System.Exception) { }
            }

            if (account == null && CommonRegexTestUtil.TestPattern(userstring, CommonRegexTestUtil.PATTERN_USERNAME))
            {
                try { account = dbContext.BTAccount.First(a => a.Password == password && a.UserName == userstring); } catch (System.Exception) { }
            }

            if (account == null && CommonRegexTestUtil.TestPattern(userstring, CommonRegexTestUtil.PATTERN_PHONE_NO))
            {
                try { account = dbContext.BTAccount.First(a => a.Password == password && a.Mobile == userstring); } catch (System.Exception) { }
            }

            // Save server cpu resources without validate password and pswhash
            // if (account != null && PasswordHash.PasswordHash.ValidatePassword(account.Password, account.PswHash))
            // {
            //     account = null;
            // }

            return account;
        }
    }
}