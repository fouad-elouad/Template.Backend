using AutoMapper;
using Template.Backend.Api.AutoMapper;

namespace Template.Backend.Api
{
    /// <summary>
    /// AutoMapper Configuration
    /// </summary>
    public class AutoMapperConfig
    {
        /// <summary>
        /// Gets a value indicating whether AutoMapper is initialized.
        /// </summary>
        /// <value>
        ///   <c>true</c> if AutoMapper is initialized; otherwise, <c>false</c>.
        /// </value>
        public static bool IsInitialized { get; private set; }

        /// <summary>
        /// Configures AutoMapper.
        /// </summary>
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<ApiToDomainMappingProfile>();
            });

            Mapper.Configuration.AssertConfigurationIsValid();
            IsInitialized = true;
        }//
    }
}