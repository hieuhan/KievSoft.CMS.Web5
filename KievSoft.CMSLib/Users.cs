using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KievSoft.DataAccess;

namespace KievSoft.CMSLib
{
    public class Users
    {
        #region Properties      
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Avatar { get; set; }
        public byte GenderId { get; set; }
        public byte UserStatusId { get; set; }
        public byte UserTypeId { get; set; }
        public short DefaultActionId { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Comments { get; set; }
        public DateTime CrDateTime { get; set; }
        public int RowCount { get; set; }
        public string ActionStatus { get; set; }
        public string ActionMessages { get; set; }
        public List<Users> ListUsers { get; set; }
        private DbHelper _dbHelper;
        //-----------------------------------------------------------------
        public Users()
        {
            _dbHelper = new DbHelper(GlobalConstants.CONNECTION_STRING_NAME);
        }
        //-----------------------------------------------------------------        
        public Users(string connectionString)
        {
            _dbHelper = new DbHelper(connectionString);
        }
        //-----------------------------------------------------------------
        ~Users()
        {

        }
        //-----------------------------------------------------------------
        public virtual void Dispose()
        {

        }
        //-----------------------------------------------------------------    
        #endregion

        #region Methods

        public async Task<Tuple<int, byte>> Insert(int actUserId)
        {
            try
            {
                Tuple<int, byte> tuple = await InsertOrUpdate(actUserId);
                return tuple;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Tuple<int, byte>> Update(int actUserId)
        {
            try
            {
                Tuple<int, byte> tuple = await InsertOrUpdate(actUserId);
                return tuple;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Tuple<int, byte>> InsertOrUpdate(int actUserId)
        {
            try
            {
                int affectedRows, userId = 0, sysMessageId = 0;
                byte sysMessageTypeId = 0;
                DbCommand dbCommand = _dbHelper.GetStoredProcCommand("Users_InsertOrUpdate");
                _dbHelper.AddInputOutputParameter(dbCommand, "@UserId", DbType.Int32, UserId);
                _dbHelper.AddInParameter(dbCommand, "@UserName", DbType.String, UserName);
                _dbHelper.AddInParameter(dbCommand, "@Password", DbType.String, Password);
                _dbHelper.AddInParameter(dbCommand, "@FullName", DbType.String, FullName);
                _dbHelper.AddInParameter(dbCommand, "@Address", DbType.String, Address);
                _dbHelper.AddInParameter(dbCommand, "@Email", DbType.String, Email);
                _dbHelper.AddInParameter(dbCommand, "@Mobile", DbType.String, Mobile);
                _dbHelper.AddInParameter(dbCommand, "@GenderId", DbType.Byte, GenderId);
                _dbHelper.AddInParameter(dbCommand, "@Avatar", DbType.String, Avatar);
                _dbHelper.AddInParameter(dbCommand, "@Salt", DbType.String, Salt);
                _dbHelper.AddInParameter(dbCommand, "@Comments", DbType.String, Comments);
                _dbHelper.AddInParameter(dbCommand, "@UserStatusId", DbType.Byte, UserStatusId);
                _dbHelper.AddInParameter(dbCommand, "@UserTypeId", DbType.Byte, UserTypeId);
                _dbHelper.AddInParameter(dbCommand, "@DefaultActionId", DbType.Int16, DefaultActionId);
                if (BirthDay.HasValue)
                    _dbHelper.AddInParameter(dbCommand, "@BirthDay", DbType.DateTime, BirthDay);
                _dbHelper.AddOutParameter(dbCommand, "@SysMessageId", DbType.Int32);
                _dbHelper.AddOutParameter(dbCommand, "@SysMessageTypeId", DbType.Byte);
                affectedRows = await _dbHelper.ExecuteNonQueryAsync(dbCommand);
                UserId = Convert.ToInt32(dbCommand.Parameters["@UserId"].Value ?? "0");
                sysMessageId = Convert.ToInt32(dbCommand.Parameters["@SysMessageId"].Value ?? "0");
                sysMessageTypeId = Convert.ToByte(dbCommand.Parameters["@SysMessageTypeId"].Value ?? "0");
                return new Tuple<int, byte>(sysMessageId, sysMessageTypeId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Tuple<List<Users>, int>> GetPage(string dateFrom, string dateTo, string orderBy, int pageSize, int pageNumber)
        {
            try
            {
                int rowCount = 0;
                DbCommand dbCommand = _dbHelper.GetStoredProcCommand("Users_GetPage");
                _dbHelper.AddInParameter(dbCommand, "@UserId", DbType.Int32, UserId);
                _dbHelper.AddInParameter(dbCommand, "@UserName", DbType.String, UserName);
                _dbHelper.AddInParameter(dbCommand, "@Password", DbType.String, Password);
                _dbHelper.AddInParameter(dbCommand, "@FullName", DbType.String, FullName);
                _dbHelper.AddInParameter(dbCommand, "@Address", DbType.String, Address);
                _dbHelper.AddInParameter(dbCommand, "@Email", DbType.String, Email);
                _dbHelper.AddInParameter(dbCommand, "@Mobile", DbType.String, Mobile);
                _dbHelper.AddInParameter(dbCommand, "@GenderId", DbType.Byte, GenderId);
                _dbHelper.AddInParameter(dbCommand, "@UserStatusId", DbType.Byte, UserStatusId);
                _dbHelper.AddInParameter(dbCommand, "@UserTypeId", DbType.Byte, UserTypeId);
                _dbHelper.AddInParameter(dbCommand, "@DefaultActionId", DbType.Int16, DefaultActionId);
                if (!string.IsNullOrEmpty(dateFrom))
                    _dbHelper.AddInParameter(dbCommand, "@DateFrom", DbType.DateTime, DbConvert.ToDateTime(dateFrom));
                if (!string.IsNullOrEmpty(dateTo))
                    _dbHelper.AddInParameter(dbCommand, "@DateTo", DbType.DateTime, DbConvert.ToDateTime(dateTo));
                _dbHelper.AddInParameter(dbCommand, "@OrderBy", DbType.String, orderBy);
                _dbHelper.AddInParameter(dbCommand, "@PageSize", DbType.Int32, pageSize);
                _dbHelper.AddInParameter(dbCommand, "@PageNumber", DbType.Int32, pageNumber);
                _dbHelper.AddOutParameter(dbCommand, "@RowCount", DbType.Int32);
                List<Users> list = await _dbHelper.ExecuteListAsync<Users>(dbCommand);
                rowCount = Convert.ToInt32(dbCommand.Parameters["@RowCount"].Value ?? "0");
                return new Tuple<List<Users>, int>(list, rowCount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Users> GetByUserId(int userId)
        {
            Users retVal = new Users();
            Tuple<List<Users>, int> user = await new Users { UserId = userId }.GetPage("", "", "", 1, 0);
            if (user.Item1 != null && user.Item1.Count > 0)
            {
                retVal = user.Item1[0];
            }
            return retVal;
        }

        public async Task<Users> GetByName(string userName)
        {
            Users retVal;
            try
            {
                DbCommand dbCommand = _dbHelper.GetStoredProcCommand("User_GetByName");
                _dbHelper.AddInParameter(dbCommand, "@UserName", DbType.String, userName);
                retVal = await _dbHelper.ExecuteObjectAsync<Users>(dbCommand) ?? new Users();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        #endregion
    }
}
