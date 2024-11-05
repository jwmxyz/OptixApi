FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
# Expose the standard HTTP and HTTPS ports
EXPOSE 8080
EXPOSE 8081
EXPOSE 443

# Use the .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Optix.Api/Optix.Api.csproj", "Optix.Api/"]
RUN dotnet restore "Optix.Api/Optix.Api.csproj"
COPY . .
WORKDIR "/src/Optix.Api"
RUN dotnet build "Optix.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Optix.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set the ASP.NET Core URL to use HTTPS
ENV ASPNETCORE_URLS=https://+:443;http://+:8080

# Set the entry point for the container
ENTRYPOINT ["dotnet", "Optix.Api.dll"]