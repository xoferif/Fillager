FROM microsoft/aspnetcore:1.1
ARG source
WORKDIR /app
EXPOSE 80
ENV RABBITMQ_BACKUP_QUEUE = backup-queue-rabbitmq
ENV RABBITMQ_BACKUP_QUEUE_NAME = file_transfer_queue
ENV MINIO_ACCESS_KEY = 6b4535c9d0545e036d5b
ENV MINIO_SECRET_KEY = f50a73124f5699570beb9ad44cd941
ENV MINIO_NAME = minio
ENV MINIO_PORT = 9000
ENV IDENTITY_REDIS_HOST = identity-redis
ENV IDENTITY_REDIS_PORT = 6379
ENV SQL_CONNECTION_STRING = Server=mariadb;Database=somedb;Uid=someuser;Pwd=very_secret;

COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "Fillager.dll"]
