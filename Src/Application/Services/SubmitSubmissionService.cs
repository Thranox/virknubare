using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Specifications;
using FluentFTP;
using Serilog;

namespace Application.Services
{
    public class SubmitSubmissionService : ISubmitSubmissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IFtpClientFactory _ftpClientFactory;

        public SubmitSubmissionService(IUnitOfWork unitOfWork, ILogger logger, IFtpClientFactory ftpClientFactory)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _ftpClientFactory = ftpClientFactory;
        }
        public async Task SubmitAsync(PolApiContext polApiContext)
        {
            string fileName;
            var submissions = _unitOfWork.Repository.List(new SubmissionsReady());

            // Should be better upload performance, but needs a solution for individual SubmissionTime?
            // Performance increase will most likely not matter, depends on the amount of files uploaded
            //var submissionsPaths = new List<string>();
            //foreach (var submissionEntity in submissions)
            //{
            //    submissionsPaths.Add(submissionEntity.PathToFile);
            //}
            //await ftp.UploadFilesAsync(submissionsPaths, "remoteDir");

            var token = new CancellationToken();

            // Submit all or just the first?!
            // Move ftp information into method parameters?
            //using (var ftp = new FtpClient("ftp01.improvento.com", "alj", "ALJ040620"))

            foreach (var submissionEntity in submissions)
            {
                var ftpClient = _ftpClientFactory.Create(submissionEntity.Customer);
                await ftpClient.ConnectAsync(token);
                // Ftp upload file.
                // Will overwrite file if file with the same name exists, will not create remote directory if it does not exist, 
                // does not require checksum verification to assume a successful upload
                fileName = Path.GetFileName(submissionEntity.PathToFile);
                // TODO
                var directory = submissionEntity.Customer.Id.ToString();
                var status = await ftpClient.UploadFileAsync(submissionEntity.PathToFile, $"/homes/ALJ/Politikerafregning/{directory}/{fileName}", FtpRemoteExists.Overwrite, createRemoteDir: true);

                // UploadFileAsync returns an enum FtpStatus, where 0 = Failed, 1 = Success and 2 = Skipped
                // We only record the SubmissionTime if the submission went well
                switch (status)
                {
                    case FtpStatus.Failed:
                        // ?
                        _logger.Error($"Upload af {fileName} fejlede.");
                        break;
                    case FtpStatus.Success:
                        submissionEntity.SubmissionTime = DateTime.Now;
                        break;
                    default:
                        throw new System.NotImplementedException();
                }
                await ftpClient.DisconnectAsync();
            }

            await _unitOfWork.CommitAsync();
        }
    }
}