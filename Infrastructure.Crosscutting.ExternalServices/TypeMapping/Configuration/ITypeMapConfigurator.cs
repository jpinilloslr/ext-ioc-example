namespace ExtIocExample.Infrastructure.Crosscutting.ExternalServices.TypeMapping.Configuration
{
    /// <summary>
    ///     Permite configurar el mapeo entre diferentes
    ///     tipos, por ejemplo, entre una entidad y su DTO.
    /// </summary>
    public interface ITypeMapConfigurator
    {
        void Configure();
    }
}