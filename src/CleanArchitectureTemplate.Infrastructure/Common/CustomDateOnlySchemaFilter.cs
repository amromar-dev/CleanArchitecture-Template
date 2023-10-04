using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CleanArchitectureTemplate.Infrastructure.Common
{
	/// <summary>
	/// 
	/// </summary>
	public class CustomDateOnlySchemaFilter : ISchemaFilter
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="schema"></param>
		/// <param name="context"></param>
		public void Apply(OpenApiSchema schema, SchemaFilterContext context)
		{
			if (context.Type == typeof(DateOnly))
			{
				schema.Type = "string";
				schema.Format = "string";
				schema.Example = new OpenApiString("yyyy-MM-dd");
			}
		}
	}
}
