using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var keycloadAdminUser = builder.AddParameter("KeyCloakUsername", "admin");
var keycloakAdminPassword = builder.AddParameter("KeyCloakPassword", "admin", secret: true);

var keycloak = builder.AddKeycloak("keycloak", 8080, keycloadAdminUser, keycloakAdminPassword)
//var keycloak = builder.AddKeycloak("keycloak", 8080)
    //.WithHttpEndpoint()
    // .WithEnvironment("KC_HOSTNAME", "http://keycloak:8080") // we are in a devcontainer -> localhost does not work
    // .WithEnvironment("KC_HOSTNAME_STRICT", "false")
    // .WithEnvironment("KC_HOSTNAME", "http://localhost:8080")
    // .WithEnvironment("KC_HOSTNAME_BACKCHANNEL_DYNAMIC", "true")
    .WithDataVolume()
    .WithOtlpExporter()
    //.WithDataBindMount(@"./../keycloak");
    .WithRealmImport(@"./../keycloak/realm-export.json");

var postgresUser = builder.AddParameter("PostgresUsername", "postgres");
var postgresPassword = builder.AddParameter("PostgresPassword", "postgres", secret: true);

var postgres = builder.AddPostgres("postgres", userName: postgresUser, password: postgresPassword)
    .WithHostPort(5432)
    .WithDataVolume()
    // .WithPgAdmin()
    .WithLifetime(ContainerLifetime.Persistent);

var notesDatabase = postgres.AddDatabase("notes");

var notesService = builder.AddProject<Projects.NotesService>("notesServcice")
    .WithReference(notesDatabase)
    .WithReference(keycloak)
    // .WaitFor(keycloak) // KeyCloak does not report health so we would wait indefintely
    .WaitFor(notesDatabase);
    

var apiService = builder.AddProject<Projects.HelloAspire_ApiService>("apiservice")
    .WithHttpHealthCheck("/health", endpointName: "http");

builder.AddProject<Projects.HelloAspire_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health", endpointName: "http")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
