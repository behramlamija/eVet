using DotNetEnv;
using eVet.Messaging;
using eVet.Worker;
using eVet.Worker.Email;

Env.NoClobber().TraversePath().Load();

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddOptions<RabbitMqSettings>()
    .BindConfiguration(RabbitMqSettings.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOptions<SmtpSettings>()
    .BindConfiguration(SmtpSettings.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton<IEmailSender, SmtpEmailSender>();
builder.Services.AddHostedService<EmailConsumer>();

var host = builder.Build();
host.Run();
