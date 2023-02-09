FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY ../. /app
ENTRYPOINT ["dotnet", "Outbox.Job.Host.dll"]
