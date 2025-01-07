using DotNetEnv;
using Quartz;
using quartz.db;
using quartz.services;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the containe
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<QuartzDbContext>();
builder.Services.AddScoped<CapacityUpdateService>();

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    var jobKey = new JobKey("CapacityUpdateJob", "DefaultGroup");
    q.AddJob<CapacityUpdateService>(opts => opts.WithIdentity(jobKey));

    using var scope = builder.Services.BuildServiceProvider().CreateScope();
    var service = scope.ServiceProvider.GetRequiredService<CapacityUpdateService>();
    var nextRunTime = service.FetchTriggerStartTime();

    DateTimeOffset triggerStartTime = new DateTimeOffset(nextRunTime);

    q.AddTrigger(opts =>
        opts.ForJob(jobKey)
            .WithIdentity("CapacityUpdateJob", "DefaultGroup")
            .StartAt(triggerStartTime)
            .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever())
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
