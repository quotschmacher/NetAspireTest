using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithHostPort(5432)
    .WithDataVolume()
    // .WithPgAdmin()
    .WithLifetime(ContainerLifetime.Persistent);

var notesDatabase = postgres.AddDatabase("notes");

var notesService = builder.AddProject<Projects.NotesService>("notesServcice")
    .WithReference(notesDatabase)
    .WaitFor(notesDatabase);

var apiService = builder.AddProject<Projects.HelloAspire_ApiService>("apiservice")
    .WithHttpHealthCheck("/health", endpointName: "http");

builder.AddProject<Projects.HelloAspire_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health", endpointName: "http")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
