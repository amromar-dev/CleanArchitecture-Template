using Microsoft.Extensions.Configuration;
using CleanArchitectureTemplate.Application.Common.Encryptions;
using CleanArchitectureTemplate.SharedKernel.Types;
using System.Web;

namespace CleanArchitectureTemplate.Application.Common.EmailTemplates
{
    public class EmailTemplateHelper : IEmailTemplateHelper
    {
        private readonly IConfiguration configuration;
        private string emailContent;
        private string emailTemplateFileName;
        private bool enablePreviewLink;
        private string previewLinkQueryParameters;

        public EmailTemplateHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
            Initial();
        }

        public void PrepareTemplate(string emailTemplateFileName, bool enablePreviewLink = true)
        {
            Initial();

            this.emailTemplateFileName = emailTemplateFileName;
            this.enablePreviewLink = enablePreviewLink;

            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Common", "EmailTemplates", "Templates", emailTemplateFileName);

            if (File.Exists(filePath) == false)
                throw new BusinessException("Template not exist");

            emailContent = File.ReadAllText(filePath);
        }

        public void AddTemplateParameter(string parameterName, string parameterValue)
        {
            if (string.IsNullOrEmpty(this.emailContent))
                throw new BusinessException("PrepareHtmlContent should be called first");

            emailContent = emailContent.Replace($"{{{parameterName}}}", parameterValue, StringComparison.InvariantCultureIgnoreCase);

            var webUrl = configuration["BaseUrls:Web"];
            emailContent = emailContent.Replace("{baseWebUrl}", webUrl, StringComparison.InvariantCultureIgnoreCase);

            if (enablePreviewLink)
                previewLinkQueryParameters = $"{previewLinkQueryParameters}&{parameterName}={parameterValue}";
        }

        public string GetEmailContent()
        {
            if (enablePreviewLink)
                AddPreviewLink();
            else
                RemovePreviewLink();

            return emailContent;
        }

        #region Private Methods
       
        private void AddPreviewLink()
        {
            var templateName = emailTemplateFileName.Replace(".html", "", StringComparison.InvariantCultureIgnoreCase);
            previewLinkQueryParameters = $"templateName={templateName}{previewLinkQueryParameters}";

            var encryptedQueryParameters = EncryptionHelper.Encrypte(previewLinkQueryParameters);
            encryptedQueryParameters = HttpUtility.UrlEncode(encryptedQueryParameters);

            var uri = configuration["BaseUrls:API"];
            var previewUri = $"{uri}/api/Account/PreviewEmail?data={encryptedQueryParameters}";

            emailContent = emailContent.Replace("{emailPreviewUri}", previewUri);
        }

        private void RemovePreviewLink()
        {
            emailContent = emailContent.Replace("View this email in your browser", "");
        }

        private void Initial()
        {
            this.emailContent = string.Empty;
            this.emailTemplateFileName = string.Empty;
            this.enablePreviewLink = false;
            this.previewLinkQueryParameters = string.Empty;
        }

        #endregion
    }
}
