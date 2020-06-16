using Application.Interfaces;
using Domain.Entities;
using FluentFTP;
using System;

namespace Application
{
    public class FtpClientFactory : IFtpClientFactory
    {
        private readonly string _host;
        private readonly string _user;
        private readonly string _password;

        public FtpClientFactory(string host, string user, string password)
        {
            _host = host;
            _user = user;
            _password = password;
        }
        public IFtpClient Create(CustomerEntity customerEntity)
        {
            var ftpIdentify = customerEntity.FtpIdentifier;

            switch (ftpIdentify)
            {
                case "SL":
                    return new FtpClient(_host, _user, _password);
                case "KMD":
                    return new FtpClient(_host, _user, _password);
                default:
                    throw new NotImplementedException($"FtpIdentifier unknown: {ftpIdentify}");
            }


        }
    }
}
