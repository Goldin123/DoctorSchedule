using DoctorSchedule.Application.Security.Interface;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Application.Security.Implementation
{
    public class ApplicationDataProtector: IApplicationDataProtector
    {
        private const string Purpose = "Doctor password protection";
        private readonly IDataProtectionProvider _provider;

        public ApplicationDataProtector(IDataProtectionProvider provider)
        {
            _provider = provider;
        }
        public string Encrypt(string plainText)
        {
            var protector = _provider.CreateProtector(Purpose);
            return protector.Protect(plainText);
        }

        public string Decrypt(string cipherText)
        {
            var protector = _provider.CreateProtector(Purpose);
            return protector.Unprotect(cipherText);
        }
    }
}
