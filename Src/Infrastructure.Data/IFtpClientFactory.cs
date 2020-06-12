using Domain.Entities;
using FluentFTP;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public interface IFtpClientFactory
    {
        IFtpClient Create(CustomerEntity customerEntity);
    }

    public class FtpClientFactory : IFtpClientFactory
    {
        private readonly IConfiguration _configuration;

        public FtpClientFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IFtpClient Create(CustomerEntity customerEntity)
        {
            var ftpIdentify = customerEntity.FtpIdentifier;

            switch (ftpIdentify)
            {
                case "SL":
                    return new FtpClient(
                        _configuration.GetValue<string>("ftpHost"),
                        _configuration.GetValue<string>("ftpUser"),
                        _configuration.GetValue<string>("ftpPass")
                        );
                case "KMD":
                    return new FtpClient(
                        _configuration.GetValue<string>("ftpHost"),
                        _configuration.GetValue<string>("ftpUser"),
                        _configuration.GetValue<string>("ftpPass")
                        );
                default:
                    throw new NotImplementedException($"FtpIdentifier unknown: {ftpIdentify}");
            }
        }
    }
}
