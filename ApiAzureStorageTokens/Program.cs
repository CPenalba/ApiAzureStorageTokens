using ApiAzureStorageTokens.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<ServiceSaSToken>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()){

}app.MapOpenApi();app.UseSwaggerUI(options => {    options.SwaggerEndpoint("/openapi/v1.json", "Azure Minimal Api");    options.RoutePrefix = "";
});app.UseHttpsRedirection();

//app.MapGet("/testing", () =>
//{
//    return "sin nada";
//});

//app.MapGet("/parametros/{dato}", (string dato
//    , ObjetoInyectado service) =>
//{
//    return "con algo: " + dato;
//});

//NECESITAMOS UN METODO GET PARA ACCEDER AL TOKEN MEDIANTE EL CURSO
//EL METODO, AL DECLARARLO, LLEVA EL Routing DE ACCESO Y TAMBIEN PUEDE LLEVAR PARAMETROS
//NECESITAMOS RECUPERAR EL SERVICE DENTRO DE NUESTRO METODO DEL MINIMAL API
app.MapGet("/token/{curso}", (string curso
, ServiceSaSToken service) =>{    string token = service.GenerateToken(curso);
    return new { token = token };
});

app.Run();