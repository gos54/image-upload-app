FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["ImageUploadApp.csproj", "."]
RUN dotnet restore "ImageUploadApp.csproj"
COPY . .
RUN dotnet build "ImageUploadApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ImageUploadApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ImageUploadApp.dll"]