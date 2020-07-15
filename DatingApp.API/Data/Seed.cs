using System.Collections.Generic;
using System.Linq;
using DatingApp.API.Models;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
    {
        public static void SeedUser(DataContext context) {

            if(!context.Users.Any())
            {
                var seedUsers = System.IO.File.ReadAllText("C:\\Users\\ionut\\Downloads\\UserSeedData.json");

                var userList = JsonConvert.DeserializeObject<List<User>>(seedUsers);

                foreach(var user in userList)
                {
                    byte[] passwordHash, passwordSalt;                
                    CreatePasswordHash("password", out passwordHash,out passwordSalt);
                    
                    user.Username = user.Username.ToLower();
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    context.Users.Add(user);
                }
                context.SaveChanges();
            }

            
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }            
        }
    }
}