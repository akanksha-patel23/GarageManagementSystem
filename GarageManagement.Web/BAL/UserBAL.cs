using GarageManagement.Web.DAL;
using GarageManagement.Web.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Web;


namespace GarageManagement.Web.BAL
{
    public class UserBAL
    {
        private readonly UserDAL _dal = new UserDAL();

        private const int Iterations = 100000;
        private const int SaltSize = 16;
        private const int HashSize = 32;

        public int CreateUser(string username, string password, string fullName, string role)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username is required.");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password is required.");
            if (role != "Admin" && role != "Customer") throw new ArgumentException("Role must be Admin or Customer.");

            //byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(salt);

            //byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
            byte[] hash;
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                hash = pbkdf2.GetBytes(HashSize);
            }

            int userId = 0;
            _dal.InsertUser(username, Convert.ToBase64String(hash), Convert.ToBase64String(salt), fullName, role, ref userId);
            return userId;
        }

        /// <summary>
        /// Verifies credentials against tblUsers. Returns the matched User (with
        /// PasswordHash/Salt cleared) on success, or null on any failure —
        /// unknown username, wrong password, and inactive account all return the
        /// same null result so login pages can't distinguish "wrong password"
        /// from "unknown user" (avoids username enumeration).
        /// </summary>
        public User VerifyLogin(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return null;

            DataTable dt = _dal.GetUserByUsername(username);
            if (dt.Rows.Count == 0) return null;

            DataRow row = dt.Rows[0];
            bool isActive = Convert.ToBoolean(row["IsActive"]);
            if (!isActive) return null;

            byte[] salt = Convert.FromBase64String(row["PasswordSalt"].ToString());
            byte[] storedHash = Convert.FromBase64String(row["PasswordHash"].ToString());
            //byte[] computedHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
            byte[] computedHash;
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                computedHash = pbkdf2.GetBytes(HashSize);
            }


            //if (!CryptographicOperations.FixedTimeEquals(storedHash, computedHash))
            if (!FixedTimeEquals(storedHash, computedHash))
                return null;

            return new User
            {
                UserID = Convert.ToInt32(row["UserID"]),
                Username = row["Username"].ToString(),
                FullName = row["FullName"].ToString(),
                Role = row["Role"].ToString(),
                IsActive = true
            };
        }

        public List<User> GetAllUsers()
        {
            DataTable dt = _dal.GetAllUsers();
            var list = new List<User>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new User
                {
                    UserID = Convert.ToInt32(row["UserID"]),
                    Username = row["Username"].ToString(),
                    FullName = row["FullName"].ToString(),
                    Role = row["Role"].ToString(),
                    IsActive = Convert.ToBoolean(row["IsActive"])
                });
            }
            return list;
        }

        public void SetUserActive(int userId, bool isActive)
        {
            if (userId <= 0) throw new ArgumentException("Invalid UserID.", nameof(userId));
            _dal.UpdateUserActive(userId, isActive);
        }

        // The attributes discourage the compiler from optimizing away the loop steps, 
        // which ensures consistent processing time regardless of when a mismatch is found.
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static bool FixedTimeEquals(byte[] left, byte[] right)
        {
            if (left == null || right == null) return false;
            if (left.Length != right.Length) return false;

            int accum = 0;
            for (int i = 0; i < left.Length; i++)
            {
                accum |= left[i] ^ right[i];
            }
            return accum == 0;
        }

    }
}