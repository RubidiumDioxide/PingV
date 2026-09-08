<p align="right">
  <a href="README.md">English</a> | 
  <strong>Русский</strong>
</p>

# PingV

## Развертывание

Для развертывания через Docker в Windows:

1. Создайте `localhost.pem` и `localhost-key.pem`.
2. Переместите оба файла в `C:\Users\YOUR_USERNAME\.angular\https`.
3. При необходимости измените путь к папке с сертификатом в `docker-compose.yml`.
4. `docker compose up --build`
5. Приложение должно быть доступно на https://localhost .

При запуске в базе данных создаются четыре тестовых пользователя.  