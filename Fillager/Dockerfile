FROM microsoft/aspnetcore:1.1
ARG source
WORKDIR /app
EXPOSE 80
ENV RABBITMQ_BACKUP_QUEUE = backup-queue-rabbitmq
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "Fillager.dll"]
