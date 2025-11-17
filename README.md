Веб-приложение для загрузки изображений
ASP.NET Core приложение для загрузки и отображения изображений с использованием Docker, Kubernetes и облачных сервисов Яндекс.Облака.

Функциональность
- Загрузка изображений через веб-форму
- Отображение списка загруженных изображений
- Хранение метаданных в PostgreSQL
- Хранение файлов в Yandex Object Storage
- Контейнеризация с Docker
- Развертывание в Kubernetes

Технологии
- ASP.NET Core 6.0
- Entity Framework Core
- PostgreSQL
- Docker
- Kubernetes
- Yandex Object Storage (S3)
- GitHub Actions

CI/CD пайплайн
Пайплайн автоматически:
Собирает Docker-образ при пуше в main ветку
Запускает тесты
Публикует образ в Yandex Container Registry
Развертывает приложение в Yandex Managed Kubernetes
