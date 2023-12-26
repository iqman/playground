using B2C_visualizer.Model;
using System.Text.Json;

namespace B2C_visualizer.ServicePrincipalReading
{
    internal interface IServicePrincipalDeserializer
    {
        IEnumerable<ServicePrincipal> Deserialize(IEnumerable<string> stringifiedServicePrincipals);
    }

    class ServicePrincipalDeserializer : IServicePrincipalDeserializer
    {
        public IEnumerable<ServicePrincipal> Deserialize(IEnumerable<string> stringifiedServicePrincipals)
        {
            IList<ServicePrincipal> sps = new List<ServicePrincipal>();

            foreach (var c in stringifiedServicePrincipals)
            {
                sps.Add(JsonSerializer.Deserialize<ServicePrincipal>(c));
            }

            return sps;
        }
    }
}
