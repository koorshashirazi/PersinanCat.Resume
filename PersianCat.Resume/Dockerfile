FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src
COPY ["PersianCat.Resume.csproj", "."]
RUN dotnet restore "PersianCat.Resume.csproj"
COPY . .
RUN dotnet publish "PersianCat.Resume.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=build /app/publish/wwwroot .
COPY ["nginx.conf", "/etc/nginx/nginx.conf"]
