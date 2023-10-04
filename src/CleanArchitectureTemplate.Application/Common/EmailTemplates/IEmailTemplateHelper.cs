namespace CleanArchitectureTemplate.Application.Common.EmailTemplates
{
    public interface IEmailTemplateHelper
    {
        void PrepareTemplate(string emailTemplateFileName, bool enablePreviewLink = true);

        void AddTemplateParameter(string parameterName, string parameterValue);

        string GetEmailContent();
    }
}
