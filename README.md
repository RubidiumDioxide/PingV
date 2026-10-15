<p align="right">
  <strong>English</strong> | 
  <a href="README.ru.md">Русский</a> | 
</p>

# PingV

## Deployment

To deploy via Docker on Windows:

1. Generate a cert (dotnet dev-certs https).
2. Copy the cert into `~\.aspnet\https`. 
3. `docker compose up --build` or run from Visual Studio
5. The app should be available on https://localhost:8080 . 