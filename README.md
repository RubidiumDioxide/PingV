<p align="right">
  <strong>English</strong> | 
  <a href="README.ru.md">Русский</a> | 
</p>

# PingV

## Deployment

To deploy via Docker on Windows:

1. Generate `localhost.pem` and `localhost-key.pem`.
2. Place both files in `C:\Users\YOUR_USERNAME\.angular\https`.
3. If needed, update the path to the certificate folder in `docker-compose.yml`. 
4. `docker compose up --build`
5. The app should be available on https://localhost .

The database will be populated with four test users on startup. 