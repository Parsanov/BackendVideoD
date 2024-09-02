using Core.Interfaces;
using Application;
using Application.VideoQ;
using YoutubeExplode;

var builder = WebApplication.CreateBuilder(args);

// Define your CORS policy name
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IVideoService, VideoService>();
builder.Services.AddScoped<IVideoQ, VideoQService>();
builder.Services.AddScoped<YoutubeClient>();
builder.Services.AddControllersWithViews();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173") // Update with your frontend URL
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials(); // Allow credentials if needed
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Apply the CORS policy to your app
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
