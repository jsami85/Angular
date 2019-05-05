using System.Collections.Generic;
using System.Linq;
using Dapper;
using CoreNg.Models;

namespace CoreNg.Services
{
    public interface IUserService
    {
        User Add(UserCreate userCreate);
        List<UserSummary> List();
        User Verify(string email, string password);
    }

    public class UserService : IUserService
    {
        private readonly IConectionService _conectionService;
        private readonly IMd5Service _md5Service;

        public UserService(IMd5Service md5Service, IConectionService conectionService)
        {
            _md5Service = md5Service;
            _conectionService = conectionService;
        }

        public User Add(UserCreate userCreate)
        {
            using (var db = _conectionService.GetAnOpenConnection())
            {
                var user = new User
                {
                    Email = userCreate.Email,
                    Name = userCreate.Name,
                    HashedPassword = _md5Service.Hash(userCreate.Password),
                };

                user.Id = db.Insert(user).Value;

                return user;
            }
        }

        public List<UserSummary> List()
        {
            using (var db = _conectionService.GetAnOpenConnection())
            {
                return db.GetList<User>()
                    .Select(x => new UserSummary()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Email = x.Email,
                    }).ToList(); ;
            }
        }

        public User Verify(string email, string password)
        {
            using (var db = _conectionService.GetAnOpenConnection())
            {
                var user = db.GetList<User>("WHERE Email=@Email", new {Email = email}).FirstOrDefault();
                if (user == null) return null;

                return user.HashedPassword == _md5Service.Hash(password) ? user : null;
            }
        }
    }
}
