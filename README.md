# NetAspireTest

- following https://aspire.dev/get-started/dev-containers/
- created dev-container
  - https://github.com/dotnet/aspire-devcontainer

- create project
  - ran `aspire new aspire-starter -n HelloAspire`
- start project
  - open `HelloAspire.AppHost/AppHost.cs` and hit "run"
  - somewhere in the debug console appears something like `Login to the dashboard at https://localhost:17206/login?t=b40e3f0db4ff1684f37fbca9886e89bb`
  - follow this link to access aspire dashboard

- update packages
  - `aspire update`

## Run Aspire App

- alternative 1
  - select "Run and Debug" tab
  - select "HelloAspire.AppHost"-project
  - hit F5
- alternative 2
  - got to "HelloAspire/HelloAspire.AppHost/AppHost.cs"
  - hit F5

## Fehler bei PostgresDB `password authentication failed for user "postgres"`

```bash
docker ps -a
docker volume ls
docker rm -f <postgres-container>
docker volume rm <postgres-volume>
```

## Delete buildartifacts

- `find . -type d \( -name bin -o -name obj \) -exec rm -rf {} +`

## Add a new Project (API)

- `dotnet new webapi --name NotesService --output HelloAspire.NotesService`
- `dotnet sln add "./HelloAspire.NotesService/NotesService.csproj"`
- `cd HelloAspire.AppHost`
- `dotnet add reference ../HelloAspire.NotesService/NotesService.csproj`
- `cd ../HelloAspire.NotesService`
- `dotnet add references ..\HelloAspire.ServiceDefaults\HelloAspire.ServiceDefaults.csproj`
- then you can add `var notesService = builder.AddProject<Projects.NotesService>("notesServcice");` to the AppHosts.cs  
  - if the NotesService does not appear:
    - Press CTRL+Shift+P
    - `Developer: Reload Window`

### Scaffold new Controller

- https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-10.0&tabs=visual-studio-code#scaffold-a-controller
```bash
cd api-project
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet tool uninstall -g dotnet-aspnet-codegenerator # might throw error, if not installed previously
dotnet tool install -g dotnet-aspnet-codegenerator
dotnet tool update -g dotnet-aspnet-codegenerator
dotnet-aspnet-codegenerator controller --help # show parameters
dotnet-aspnet-codegenerator controller -name UserController -async -api -outDir Controllers
```

## Work with a Database
dotnet tool
- Install EFCore-Tools
  - `dotnet new tool-manifest`
  - `dotnet tool install --global dotnet-ef`
  - `dotnet tool restore`
- Migration erstellen
  - `cd HelloAspire.NotesService` in entsprechendes Projekt wechseln
  - `dotnet ef migrations add Create_Database -o Data/Migrations`

## #5 Add authentification

- guideline: https://www.youtube.com/watch?v=HAvCoQ0tOTs
- go to porject folder
  - `aspire add keycloak`
- go to consuming project
  - `dotnet add package Aspire.Keycloak.Authentication --version 13.1.3-preview.1.26166.8`
- realm "notes" is imported at start
  - create user
    - name:
      - `test@test.com`
    - passwod:
      - `123`

### Test with bruno

- run Keycloak - Get JWT with Username and Password
  - token is stored as a variable
- run Notes Api - Get All Notes