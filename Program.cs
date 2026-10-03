using ABM_Productos.Repositories.Implementations;
using ABM_Productos.Repositories.Interfaces;
using ABM_Productos.Services.Implementations;
using ABM_Productos.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
