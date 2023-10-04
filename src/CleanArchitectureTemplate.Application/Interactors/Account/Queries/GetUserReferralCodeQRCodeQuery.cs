using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using CleanArchitectureTemplate.Application.Common.FileStorageServices;
using CleanArchitectureTemplate.Application.Interactors.Account.Outputs;
using CleanArchitectureTemplate.Domain.Customers.Interfaces;
using CleanArchitectureTemplate.SharedKernel.Contracts;
using CleanArchitectureTemplate.SharedKernel.Types;
using CleanArchitectureTemplate.SharedKernel.Utilities;

namespace CleanArchitectureTemplate.Application.Interactors.Account.Queries
{
    public class GetUserReferralCodeQRCodeQuery : IInteractorBase<ReferralCodeQROutput>
    {

    }

    public class GetUserReferralCodeQRCodeHandler : IInteractorHandlerBase<GetUserReferralCodeQRCodeQuery, ReferralCodeQROutput>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IHttpContextAccessor context;
        private readonly IConfiguration configuration;
        private readonly IFileStorageService fileStorageService;

        public GetUserReferralCodeQRCodeHandler(ICustomerRepository customerRepository, IHttpContextAccessor context, IConfiguration configuration, IFileStorageService fileStorageService)
        {
            this.customerRepository = customerRepository;
            this.context = context;
            this.configuration = configuration;
            this.fileStorageService = fileStorageService;
        }

        public async Task<ResponseBase<ReferralCodeQROutput>> Handle(GetUserReferralCodeQRCodeQuery request, CancellationToken cancellationToken)
        {
            var userId = context.GetUserId();
            var cusomter = await customerRepository.FindAsync(userId);
            if (cusomter == null)
                throw new BusinessException("User not found");

            if (cusomter.ReferralCodeQRUrl == null)
            {
                var baseUrl = configuration["BaseUrls:Web"];
                var bytes = QRGenerator.Generate($"{baseUrl}?referralCode={cusomter.ReferralCode}");
                var stream = new MemoryStream(bytes);
                var response = await fileStorageService.UploadAsync(stream, $"{Guid.NewGuid()}.png", "referralcode");
                
                cusomter.UpdateReferralCodeQR(response.AbsolutePath);
                await customerRepository.CommitAsync(cancellationToken);
            }

            return new ResponseBase<ReferralCodeQROutput>(new ReferralCodeQROutput() { QRCodeUrl = cusomter.ReferralCodeQRUrl });
        }
    }
}
