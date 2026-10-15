<p align="right">
  <a href="README.md">English</a> | 
  <strong>Русский</strong>
</p>

# PingV

## Развертывание

Для развертывания через Docker в Windows:

1. Создайте сертификат (dotnet dev-certs https).
2. Скопируйте сертификат в `~\.aspnet\https`.
3. Создайте локальный .env с необходимыми значениями. 
4. `docker compose up --build` или запустите из Visual Studio.  
5. Приложение должно быть доступно на https://localhost:8080 . 