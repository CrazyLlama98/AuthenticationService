using AuthenticationService.Dtos;
using AuthenticationService.Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AuthenticationService.Test.MapperTests.DataSets
{
    public class AccountDtoToUserTestDataSet : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] 
            {
                new AccountDto
                {
                    Email = "test@test.com",
                    Username = "test1"
                },
                new User
                {
                    Email = "test@test.com",
                    UserName = "test1"
                }
            };

            yield return new object[]
            {
                new AccountDto
                {
                    Email = "",
                    Username = "test2",
                    Password = Guid.NewGuid().ToString()
                },
                new User
                {
                    Email = "",
                    UserName = "test2",
                }
            };

            yield return new object[]
            {
                new AccountDto
                {
                    Email =  null,
                    Username = ""
                },
                new User
                {
                    Email = null,
                    UserName = ""
                }
            };

            yield return new object[]
            {
                new AccountDto
                {
                    Email = null,
                    Username = null
                },
                new User
                {
                    Email = null,
                    UserName = null
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
