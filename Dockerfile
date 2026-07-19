#Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/BookingApi/BookingApi.csproj", "src/BookingApi/"]
RUN dotnet restore "src/BookingApi/BookingApi.csproj"
COPY . .
RUN dotnet publish "src/BookingApi/BookingApi.csproj" -c Release -o /app/publish

#Stage 2: Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "BookingApi.dll"]
