using HelpDeskApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// add cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Add Db Context
builder.Services.AddDbContext<HelpDeskAppDbContext>(options => options.UseSqlServer());

// Add services to the container.
builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
// Activate Cors ( REMEMBER!!)
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();

