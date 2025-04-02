using Azure.Data.Tables.Sas;
using Azure.Data.Tables;

namespace ApiAzureStorageTokens.Services
{
    public class ServiceSaSToken
    {
        private TableClient tablaAlumnos;

        public ServiceSaSToken(IConfiguration configuration)
        {
            string azureKeys = configuration.GetValue<string>("AzureKeys:StorageAccount");
            TableServiceClient serviceClient = new TableServiceClient(azureKeys);
            this.tablaAlumnos = serviceClient.GetTableClient("alumnos");
        }

        public string GenerateToken(string curso)
        {
            //NECESITAMOS LOS PERMISOS DE ACCESO, POR AHORA SOLAMNETE PERMITIREMOS LECTURA
            TableSasPermissions permisos = TableSasPermissions.Read | TableSasPermissions.Add;

            //EL ACCESO AL TOKEN ESTA DELIMITADO POR UN TIEMPO
            //NECESITAMOS INCLUIR EL TIEMPO QUE TENDRA ACCESO A LEER LOS ELEMENTOS
            TableSasBuilder builder = this.tablaAlumnos.GetSasBuilder(permisos, DateTime.UtcNow.AddMinutes(30));

            //COMO ROW KEY Y PARTITION KEY SON STRING, PODEMOS LIMITAR EL ACCESO DE FORMA ALFABETICA 
            //A LOS DATOS, YA SEA POR ROW KEY, PARTITION KEY O LOS DOS
            builder.PartitionKeyStart = curso;
            builder.PartitionKeyEnd = curso;

            //CON ESTO YA PODEMOS GENERAR EL TOKEN QUE ES UN ACCESO POR URI
            Uri uriToken = this.tablaAlumnos.GenerateSasUri(builder);
            string token = uriToken.AbsoluteUri;
            return token;
        }
    }
}
