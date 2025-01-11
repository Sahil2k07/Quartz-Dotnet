using DotNetEnv;
using Quartz;
using quartz.db;
using quartz.enums;
using quartz.services;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the containe
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<QuartzDbContext>();
builder.Services.AddScoped<ProductionCapacityUpdateService>();

builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey(QuartzJobs.ProductionCapacityUpdate.ToString());
    q.AddJob<ProductionCapacityUpdateService>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts =>
        opts.ForJob(jobKey)
            .WithIdentity(QuartzJobs.ProductionCapacityUpdate.ToString())
            // .WithCronSchedule("0 0 0 * * ?")
            .WithCronSchedule("0 * * * * ?")
    );
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
