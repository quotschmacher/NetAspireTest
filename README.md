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