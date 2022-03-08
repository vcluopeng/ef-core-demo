using System;
using System.Collections.Generic;
using System.Text;
using Bogus;
using EfCoreDemo.Core.Entities;
using System.Linq;

namespace EfCoreDemo.Core.Seeds
{
    public class UserSeedData
    {
        public static IEnumerable<User> Build(int count)
        {
            var builds = new Faker<User>("zh_CN").StrictMode(false)
                //Basic rules using built-in generators 
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
                //Compound property with context, use the first/last name properties
                .RuleFor(u => u.FullName, (f, u) => u.FirstName + " " + u.LastName)
                .RuleFor(u => u.UserName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                .RuleFor(u => u.Age, (f, u) => f.Random.Number(18, 88))
                .RuleFor(u => u.NickName, (f, u) => f.Company.CompanyName())
                .RuleFor(u => u.Phone, (f, u) => f.Phone.PhoneNumber());
            return builds.Generate(count);
        }
        public static User Build()
        {
            return UserSeedData.Build(1).First();
        }
    }
}
