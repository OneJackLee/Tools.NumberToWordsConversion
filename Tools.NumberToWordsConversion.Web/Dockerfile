FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5147
EXPOSE 7198

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Tools.NumberToWordsConversion.Web/Tools.NumberToWordsConversion.Web.csproj", "Tools.NumberToWordsConversion.Web/"]
COPY ["Tools.NumberToWordsConversion.Application/Tools.NumberToWordsConversion.Application.csproj", "Tools.NumberToWordsConversion.Application/"]
RUN dotnet restore "Tools.NumberToWordsConversion.Web/Tools.NumberToWordsConversion.Web.csproj"
COPY . .
WORKDIR "/src/Tools.NumberToWordsConversion.Web"
RUN dotnet build "Tools.NumberToWordsConversion.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Tools.NumberToWordsConversion.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tools.NumberToWordsConversion.Web.dll"]
